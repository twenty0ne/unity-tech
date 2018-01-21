using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Lidgren.Network;

public class XNetManager : MonoBehaviour 
{
	private const int MAX_CONNECTIONS = 2;

	// C2H = client to host
	// H2C = host to client
	public enum DataType
	{
		C2H_Connect = 0,
		H2C_Accept,
		SYNC,
	}

	public static XNetClient instance = null;

	private NetClient netClient = null;
	private HostTopology hostTopology = null;

	// client to server connection
	private NetworkClient myClient = null;

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
			netClient.Disconnect ();
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

			var conn = new XNetConnection();
			conn.ForceInitialize();

			NetworkServer.AddExternalConnection(newConn);
			connRemote = newConn;
			//});

			// send accept
			NetOutgoingMessage om = _netClient.CreateMessage();
			om.Write(DATA_TYPE.DAT_ACCEPT);
			om.Write(client_id);
			_netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 0);
		}
		else if (dataType == DataType.H2C_Accept)
		{
			// Create connection to host player's steam ID
			var conn = new MyNetworkConnection();
			var mySteamClient = new MyNetworkClient(conn);
			myClient = mySteamClient;

			// Setup and connect
			mySteamClient.RegisterHandler(MsgType.Connect, OnConnect);
			mySteamClient.SetNetworkConnectionClass<MyNetworkConnection>();
			mySteamClient.Configure(MyNetworkManager.hostTopology);
			mySteamClient.Connect();
		}
		else if (dataType == DataType.SYNC)
		{
			int byteLength = im.ReadInt32();
			byte[] byteArray = im.ReadBytes(byteLength);

			NetworkConnection conn = isHost ? connRemote : myClient.connection;

			if (conn != null)
			{
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
		myClient.connection.ForceInitialize();

		// Add local client to server's list of connections
	}
}
