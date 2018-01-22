using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Lidgren.Network;
using UnityEngine.Networking.NetworkSystem;

public class XNetManager : MonoBehaviour 
{
	private const int MAX_CONNECTIONS = 2;

	// C2H = client to host
	// H2C = host to client
	public class DataType
	{
        public const int C2H_Connect = 0;
        public const int H2C_Accept = 1;
        public const int Sync = 2;
	}

	public static XNetManager instance = null;

	private Lidgren.Network.NetClient netClient = null;
	private HostTopology hostTopology = null;

    private GameObject playerPrefab;
    private List<GameObject> netPrefabs;

    // client to server connection
    // for host: local client to host
    // for remote: remote client to host
    private NetworkClient myClient = null;
    private List<NetworkConnection> connClients = new List<NetworkConnection>();

	private void Awake()
	{
		if (instance != null) 
		{
			Debug.LogError ("More than one XNetManager instance was found.");
			this.enabled = false;
			return;
		}

		instance = this;
	}

	private void OnDestroy()
	{
		instance = null;

		if (netClient != null) 
		{
			netClient.Disconnect ("disconnect from destroy");
			netClient = null;
		}
	}

	private void Update()
	{
		if (netClient != null) 
		{
			this.PeekMessages ();
		}
	}

	private void OnConnected()
	{
		
	}

	private void OnDisconnected()
	{
		
	}

	private void OnData(NetIncomingMessage im)
	{
		int dataType = im.ReadInt32();
		if (dataType == DataType.C2H_Connect)
		{
			string clientId = im.ReadString();

			var remoteClientToHostConn = new XNetConnection(clientId);
            remoteClientToHostConn.ForceInitialize(hostTopology);

			NetworkServer.AddExternalConnection(remoteClientToHostConn);
            AddConnection(remoteClientToHostConn);

			// send accept
			NetOutgoingMessage om = netClient.CreateMessage();
			om.Write(DataType.H2C_Accept);
			om.Write(clientId);
			netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 0);
		}
		else if (dataType == DataType.H2C_Accept)
		{
            string clientId = im.ReadString();

			var conn = new XNetConnection(clientId);
            Debug.Assert(myClient == null, "CHECK: myClient is not null");
            myClient = new XNetClient(conn);

            // Setup and connect
            myClient.RegisterHandler(MsgType.Connect, OnRemoteClientConnect);
            myClient.SetNetworkConnectionClass<XNetConnection>();
            myClient.Configure(hostTopology);
            ((XNetClient)myClient).Connect();
		}
		else if (dataType == DataType.Sync)
		{
            string clientId = im.ReadString();

            NetworkConnection conn = GetClientConnection(clientId);
			if (conn != null)
			{
                int byteLength = im.ReadInt32();
                byte[] byteArray = im.ReadBytes(byteLength);

                conn.TransportReceive(byteArray, byteLength, 0);
			}
		}
	}

	private void PeekMessages()
	{
		NetIncomingMessage im;
		while ((im = netClient.ReadMessage()) != null)
		{
			// handle incoming message
			switch (im.MessageType)
			{
			case NetIncomingMessageType.DebugMessage:
			case NetIncomingMessageType.ErrorMessage:
			case NetIncomingMessageType.WarningMessage:
			case NetIncomingMessageType.VerboseDebugMessage:
				{
					string text = im.ReadString ();
					Debug.Log ("output: " + text);
				}
				break;
			case NetIncomingMessageType.StatusChanged:
				{
					NetConnectionStatus status = (NetConnectionStatus)im.ReadByte ();

					string reason = im.ReadString ();
					Debug.Log (status.ToString () + ": " + reason);

					if (status == NetConnectionStatus.Connected)
						this.OnConnected ();
					else if (status == NetConnectionStatus.Disconnected)
						this.OnDisconnected ();
				}
				break;
			case NetIncomingMessageType.Data:
				{
					this.OnData (im);
				}
				break;
			default:
				Debug.Log("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
				break;
			}
			netClient.Recycle(im);
		}
	}

	private void InitUnetHost()
	{
		Debug.Log ("init unet host");

		ConnectionConfig config = new ConnectionConfig ();
		config.AddChannel (QosType.ReliableSequenced);
		config.AddChannel (QosType.Unreliable);
		hostTopology = new HostTopology (config, MAX_CONNECTIONS);

		// Listen for player spawn request messages 
		// NetworkServer.RegisterHandler(NetworkMessages.SpawnRequestMsg, OnSpawnRequested);

		// Start UNET server
		NetworkServer.Configure(hostTopology);
		NetworkServer.dontListen = true;
		NetworkServer.Listen(0);

		// Create a local client-to-server connection to the "server"
		// Connect to localhost to trick UNET's ConnectState state to "Connected", which allows data to pass through TransportSend
		myClient = ClientScene.ConnectLocalServer();
		myClient.Configure(hostTopology);
		myClient.Connect("localhost", 0);
		myClient.connection.ForceInitialize(hostTopology);

		// Add local client to server's list of connections
		// Here we get the connection from the NetworkServer because it represents the server-to-client connection
		var localClientToHostConn = NetworkServer.connections[0];
		AddConnection (localClientToHostConn);

        RegisterNetworkPrefabs();

        // Spawn self
        ClientScene.Ready(localClientToHostConn);
        SpawnPlayer(localClientToHostConn);
	}

	private void AddConnection(NetworkConnection conn)
	{
        connClients.Add(conn);
	}

	private void RegisterNetworkPrefabs()
	{
		for (int i = 0; i < netPrefabs.Count; i++) 
		{
			ClientScene.RegisterPrefab (netPrefabs [i]);
		}
	}

    private bool SpawnPlayer(NetworkConnection conn)
    {
        if (playerPrefab == null)
            return false;

        NetworkServer.SetClientReady(conn);
        var player = GameObject.Instantiate(playerPrefab);

        return NetworkServer.SpawnWithClientAuthority(player, conn);
    }

    private void OnRemoteClientConnect(NetworkMessage msg)
    {
        // Set to ready and spawn player
        Debug.Log("Connected to UNET server.");
        myClient.UnregisterHandler(MsgType.Connect);

        RegisterNetworkPrefabs();

        var conn = myClient.connection;
        if (conn != null)
        {
            ClientScene.Ready(conn);
            Debug.Log("Requesting spawn");
            myClient.Send(XNetMessage.SpawnRequestMsg, new StringMessage(""));
        }
    }

    public bool Send(byte[] bytes, int numBytes, int channelId = 0)
    {
        NetOutgoingMessage om = netClient.CreateMessage();
        om.Write(numBytes);
        om.Write(bytes);
        netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered, channelId);

        return true;
    }

    private NetworkConnection GetClientConnection(string clientId)
    {
        // if host
        if (NetworkServer.active && NetworkServer.connections.Count > 0)
            return NetworkServer.connections[0];

        // find remote client
        for (int i = 0; i < connClients.Count; ++i)
        {
            var xnetConn = connClients[i] as XNetConnection;
            if (xnetConn != null && xnetConn.ClientId == clientId)
                return xnetConn;
        }

        Debug.Log("client not found");
        return null;
    }
}
