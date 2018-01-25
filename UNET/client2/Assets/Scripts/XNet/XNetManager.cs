using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Lidgren.Network;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

public class XNetManager : MonoBehaviour
{
    private const string PVP_ADDRESS = "127.0.0.1";
    private const int PVP_PORT = 14242;

    private static readonly string s_LobbySceneName = "LobbyScene";

    public const int MAX_CONNECTIONS = 4;

	// C2H = client to host
	// H2C = host to client
	public class DataType
	{
        public const int MATCH = 0;
        public const int C2H_Connect = 1;
        public const int H2C_Accept = 2;
        public const int Sync = 3;
        public const int GetMatchList = 4;
        public const int JoinMatch = 5;
        //public const int MatchReady = 6;
        //public const int MatchUnready = 7;
    }

    public class NetCode
    {
        public const int OK = 0;
        // match
        public const int MATCH_ROOMCREATE_SUCCESS = 100;
        public const int MATCH_ROOMCREATE_FAIL = 101;
        public const int MATCH_ROOMJOIN_SUCCESS = 102;
        public const int MATCH_ROOMJOIN_FAIL = 103;
    }

    public static XNetManager instance = null;

	protected Lidgren.Network.NetClient netClient = null;
	// private HostTopology hostTopology = null;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<GameObject> netPrefabs = new List<GameObject>();
    //[SerializeField] private Tanks.Networking.NetworkPlayer m_NetworkPlayerPrefab;

    // client to server connection
    // for host: local client to host
    // for remote: remote client to host
    private NetworkClient myClient = null;
    private List<NetworkConnection> connClients = new List<NetworkConnection>();

    private List<XNetMatchInfoSnapshot> matchInfoList = new List<XNetMatchInfoSnapshot>();

    private AsyncOperation s_LoadingSceneAsync;
    public static event Action<bool> playerJoined;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one XNetManager instance was found.");
            this.enabled = false;
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

    }

    private void OnDestroy()
    {
        instance = null;

        if (netClient != null)
        {
            netClient.Disconnect("disconnect from destroy");
            netClient = null;
        }
    }

    private void Update()
    {
        if (netClient != null)
        {
            this.PeekMessages();
        }

        if (s_LoadingSceneAsync != null && s_LoadingSceneAsync.isDone)
        {
            s_LoadingSceneAsync.allowSceneActivation = true;
            s_LoadingSceneAsync = null;

            // finish load scene
            if (IsHost())
            {
                NetworkServer.SpawnObjects();
                SpawnPlayer(NetworkServer.connections[0]);
            }
            else
            {

            }
        }
    }

    //public static XNetManager Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //            instance = new XNetManager();
    //        return instance;
    //    }
    //}

    //public void Init()
    //{

    //}

    //public void Release()
    //{
    //    if (netClient != null)
    //    {
    //        netClient.Disconnect("disconnect from destroy");
    //        netClient = null;
    //    }

    //    instance = null;
    //}

    private System.Action<bool> cbServerConnect;
    public void ConnectServer(string address, int port, System.Action<bool> onConnect)
    {
        cbServerConnect = onConnect;

        // Connect to pvp game server
        NetPeerConfiguration config = new NetPeerConfiguration("pvp");
        config.EnableMessageType(NetIncomingMessageType.ConnectionLatencyUpdated);
        // config.AutoFlushSendQueue = false;
        netClient = new NetClient(config);

        netClient.Start();
        //NetOutgoingMessage hail = netClient.CreateMessage("0wI4g5Q8");
        netClient.Connect(address, port);
    }

	private void OnServerConnected()
	{
        if (cbServerConnect != null)
            cbServerConnect(true);
	}

    private void OnServerDisconnected()
	{
        if (cbServerConnect != null)
            cbServerConnect(false);
	}

    private void OnServerData(NetIncomingMessage im)
	{
		int dataType = im.ReadInt32();
//		if (dataType == DataType.C2H_Connect)
//		{
//			string clientId = im.ReadString();
//
//			var remoteClientToHostConn = new XNetConnection(clientId);
//            remoteClientToHostConn.ForceInitialize(hostTopology);
//
//			NetworkServer.AddExternalConnection(remoteClientToHostConn);
//            AddConnection(remoteClientToHostConn);
//
//			// send accept
//			NetOutgoingMessage om = netClient.CreateMessage();
//			om.Write(DataType.H2C_Accept);
//			om.Write(clientId);
//			netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 0);
//		}
//		else if (dataType == DataType.H2C_Accept)
//		{
//            string clientId = im.ReadString();
//
//			var conn = new XNetConnection(clientId);
//            Debug.Assert(myClient == null, "CHECK: myClient is not null");
//            myClient = new XNetClient(conn);
//
//            // Setup and connect
//            myClient.RegisterHandler(MsgType.Connect, OnRemoteClientConnect);
//            myClient.SetNetworkConnectionClass<XNetConnection>();
//            myClient.Configure(hostTopology);
//            ((XNetClient)myClient).Connect();
//		}
		if (dataType == DataType.Sync)
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
        else if (dataType == DataType.MATCH)
        {
            //Debug.Assert(cbMatch != null, "CHECK");

            int code = im.ReadInt32();

            bool success = (code == NetCode.MATCH_ROOMCREATE_SUCCESS || code == NetCode.MATCH_ROOMJOIN_SUCCESS);
            bool isHost = (code == NetCode.MATCH_ROOMCREATE_SUCCESS || code == NetCode.MATCH_ROOMCREATE_FAIL);

            if (cbMatch != null)
            {
                XNetMatchInfo matchInfo = new XNetMatchInfo();
                matchInfo.isHost = isHost;
                cbMatch(success, matchInfo);
            }

            if (success)
            {
                StartHost();
                StartLocalClient();
            }
        }
//        else if (dataType == DataType.GetMatchList)
//        {
//            matchInfoList.Clear();
//
//            int roomCount = im.ReadInt32();
//            for (int i = 0; i < roomCount; ++i)
//            {
//                XNetMatchInfoSnapshot mi = new XNetMatchInfoSnapshot();
//                mi.networkId = im.ReadString();
//                mi.name = im.ReadString();
//                mi.maxSize = im.ReadInt32();
//                mi.currentSize = im.ReadInt32();
//
//                matchInfoList.Add(mi);
//            }
//        }
        else if (dataType == DataType.JoinMatch)
        {
            bool success = im.ReadBoolean();
            string clientId = im.ReadString();
            Debug.Log("cmd joinmatchretrun>" + success.ToString() + ">" + clientId);

			if (success)
			{
				if (IsHost())
				{
					var remoteClientToHostConn = new XNetConnection(clientId);
					remoteClientToHostConn.ForceInitialize(hostTopology);

					NetworkServer.AddExternalConnection(remoteClientToHostConn);
					AddConnection(remoteClientToHostConn);
				}
				else
				{
					var conn = new XNetConnection(clientId);
					Debug.Assert(myClient == null, "CHECK: myClient is not null");
					myClient = new XNetClient(conn);

					// Setup and connect
					myClient.RegisterHandler(MsgType.Connect, OnClientConnectInternal);
                    myClient.RegisterHandler(MsgType.Scene, OnClientSceneInternal);

					myClient.SetNetworkConnectionClass<XNetConnection>();
					myClient.Configure(hostTopology);
					((XNetClient)myClient).Connect();
				}
			}

            if (playerJoined != null)
            {
                playerJoined(success);
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
						OnServerConnected ();
					else if (status == NetConnectionStatus.Disconnected)
						OnServerDisconnected ();
				}
				break;
			case NetIncomingMessageType.Data:
				{
					this.OnServerData (im);
				}
				break;
			default:
				Debug.Log("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
				break;
			}
			netClient.Recycle(im);
		}
	}

    private static HostTopology m_hostTopology = null;
    public static HostTopology hostTopology
    {
        get
        {
            if (m_hostTopology == null)
            {
                ConnectionConfig config = new ConnectionConfig();
                config.AddChannel(QosType.ReliableSequenced);
                config.AddChannel(QosType.Unreliable);
                m_hostTopology = new HostTopology(config, 2);
            }

            return m_hostTopology;
        }
    }

    // NOTE
    // when match create
    public void StartHost()
	{
		//ConnectionConfig config = new ConnectionConfig ();
		//config.AddChannel (QosType.ReliableSequenced);
		//config.AddChannel (QosType.Unreliable);
		//hostTopology = new HostTopology (config, MAX_CONNECTIONS);

		// Listen for player spawn request messages 
		NetworkServer.RegisterHandler(XNetMessage.SpawnRequestMsg, OnSpawnRequested);

		// Start UNET server
		NetworkServer.Configure(hostTopology);
		NetworkServer.dontListen = true;
		NetworkServer.Listen(0);
		
		OnStartHost();
	}

    void OnSpawnRequested(NetworkMessage msg)
    {
        Debug.LogWarning("Spawn request received");

        var conn = msg.conn;
        if (conn != null)
        {
            SpawnPlayer(conn);
        }
    }


    public virtual void OnStartHost()
	{
		
	}	
	
	public void StartLocalClient()
	{
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

        // myClient.RegisterHandler(MsgType.AddPlayer, OnServerAddPlayerMessageInternal);

        // Spawn self
        ClientScene.Ready(localClientToHostConn);
        // ClientScene.AddPlayer(0);
        // SpawnPlayer(localClientToHostConn);

        OnStartLocalClient();
	}
	
	public virtual void OnStartLocalClient()
	{
		
	}

    public void StartRemoteClient()
    {

    }

    public virtual void OnStartRemoteClient()
    {

    }

    internal void OnServerAddPlayerMessageInternal(NetworkMessage netMsg)
    {
        //if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnServerAddPlayerMessageInternal"); }

        //Tanks.Networking.NetworkPlayer newPlayer = Instantiate<Tanks.Networking.NetworkPlayer>(playerPrefab);
        //DontDestroyOnLoad(newPlayer);
        //NetworkServer.AddPlayerForConnection(netMsg.conn, newPlayer.gameObject, playerControllerId);
        //NetworkServer.SpawnWithClientAuthority(newPlayer.gameObject, netMsg.conn);
        SpawnPlayer(netMsg.conn);
    }

    //
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
        //DontDestroyOnLoad(player);

        //return NetworkServer.SpawnWithClientAuthority(player, conn);
        return NetworkServer.AddPlayerForConnection(conn, player, 0);
    }

    private void OnClientConnectInternal(NetworkMessage msg)
    {
        // Set to ready and spawn player
        Debug.Log("Connected to UNET server.");
        myClient.UnregisterHandler(MsgType.Connect);

        RegisterNetworkPrefabs();

        ClientScene.Ready(myClient.connection);

        //        var conn = myClient.connection;
        //        if (conn != null)
        //        {
        //            ClientScene.Ready(conn);
        //            Debug.Log("Requesting spawn");
        //            myClient.Send(XNetMessage.SpawnRequestMsg, new StringMessage(""));
        //        }
    }

    private bool IsClientConnected()
    {
        return true;
    }

    private void OnClientSceneInternal(NetworkMessage netMsg)
    {
        string newSceneName = netMsg.reader.ReadString();
        if (IsClientConnected() && !NetworkServer.active)
        {
            ClientChangeScene(newSceneName);
        }
    }

    public bool Send(byte[] bytes, int numBytes, int channelId = 0)
    {
        NetOutgoingMessage om = netClient.CreateMessage();
        om.Write(DataType.Sync);
        om.Write(netClient.ServerConnection.RemoteUniqueIdentifier.ToString());
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

    public bool IsHost()
    {
        return NetworkServer.active;
    }

    public bool IsConnectServer()
    {
        return netClient != null && netClient.ConnectionStatus == NetConnectionStatus.Connected;
    }

    public NetClient GetNetClient()
    {
        return netClient;
    }

    public void Disconnect()
    {
    }

    private System.Action<bool, XNetMatchInfo> cbMatch;
    public void StartMatchmaking(string roomName, System.Action<bool, XNetMatchInfo> onMatch)
    {
        cbMatch = onMatch;

        if (IsConnectServer() == false)
        {
            ConnectServer(PVP_ADDRESS, PVP_PORT, (success) =>
            {
                // TODO
                // refactor > user shouldn't know NetOutgoingMessage exit
                //XNetMessage msg = new XNetMessage();
                //msg.Write(XNetManager.DataType.MATCH);
                //msg.Write(roomName);

                NetOutgoingMessage om = netClient.CreateMessage();
                om.Write(DataType.MATCH);
                om.Write(roomName);
                netClient.SendMessage(om, Lidgren.Network.NetDeliveryMethod.ReliableOrdered, 0);

                // XNetManager.Instance.SendMessage(msg);

                //XNetMatchInfo matchInfo = new XNetMatchInfo();
                //onMatch(true, matchInfo);
            });
        }
        else
        {
            NetOutgoingMessage om = netClient.CreateMessage();
            om.Write(DataType.MATCH);
            om.Write(roomName);
            netClient.SendMessage(om, Lidgren.Network.NetDeliveryMethod.ReliableOrdered, 0);
        }
    }

    public bool hasSufficientPlayers
    {
        get
        {
            // return isSinglePlayer ? playerCount >= 1 : playerCount >= 2;
            return false;
        }
    }

    public void CreateHost()
    {
        // StartHost();

        ConnectServer(PVP_ADDRESS, PVP_PORT, (success)=> 
        {
            // start match
            NetOutgoingMessage om = netClient.CreateMessage();
            om.Write(DataType.MATCH);
            om.Write("0wI4g5Q8");
            netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 0);

            //
            MenuControl.instance.SetStatusText("host create success");
        });
    }

    public void CreateClient()
    {
        ConnectServer(PVP_ADDRESS, PVP_PORT, (success) =>
        {
            // join match
            NetOutgoingMessage om = netClient.CreateMessage();
            om.Write(DataType.JoinMatch);
            om.Write("0wI4g5Q8");
            netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 0);

            //
            MenuControl.instance.SetStatusText("client join success");
        });
    }

    public void StartGame()
    {
        // change scene
        ServerChangeScene("InGame");
    }

    private void ServerChangeScene(string newSceneName)
    {
        if (string.IsNullOrEmpty(newSceneName))
        {
            Debug.LogError("ServerChangeScene empty scene name"); 
            return;
        }

        Debug.Log("ServerChangeScene " + newSceneName); 
        NetworkServer.SetAllClientsNotReady();

        s_LoadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);

        StringMessage msg = new StringMessage(newSceneName);
        // NetworkServer.SendToAll(MsgType.Scene, msg);
        for (int index = 0; index < NetworkServer.connections.Count; ++index)
        {
            NetworkConnection connection = NetworkServer.connections[index];
            if (connection != null)
                connection.Send(MsgType.Scene, msg);
        }
    }

    private void ClientChangeScene(string newSceneName)
    {
        s_LoadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);
    }
}
