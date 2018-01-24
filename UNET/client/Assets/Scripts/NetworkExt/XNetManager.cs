using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Lidgren.Network;
using UnityEngine.Networking.NetworkSystem;
using Tanks.UI;
using UnityEngine.Networking.Types;
using System;
using UnityEngine.Networking.Match;
using Tanks.Map;
using Tanks;
using UnityEngine.SceneManagement;
using Tanks.Networking;

public class XNetManager : MonoBehaviour
{
    private const string PVP_ADDRESS = "127.0.0.1";
    private const int PVP_PORT = 14242;

    private static readonly string s_LobbySceneName = "LobbyScene";

    private const int MAX_CONNECTIONS = 2;

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
        public const int JoinMatchReturn = 6;
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

    protected GameSettings m_Settings;
    public List<Tanks.Networking.NetworkPlayer> connectedPlayers
    {
        get;
        private set;
    }

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

        connectedPlayers = new List<Tanks.Networking.NetworkPlayer>();
    }

    private void Start()
    {
        m_Settings = GameSettings.s_Instance;
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
        // NetOutgoingMessage hail = netClient.CreateMessage();
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
        else if (dataType == DataType.MATCH)
        {
            Debug.Assert(cbMatch != null, "CHECK");

            int code = im.ReadInt32();

            bool success = (code == NetCode.MATCH_ROOMCREATE_SUCCESS || code == NetCode.MATCH_ROOMJOIN_SUCCESS);
            bool isHost = (code == NetCode.MATCH_ROOMCREATE_SUCCESS || code == NetCode.MATCH_ROOMCREATE_FAIL);
            
			XNetMatchInfo matchInfo = new XNetMatchInfo();
			matchInfo.isHost = isHost;
			cbMatch(success, matchInfo);
			
			if (success)
			{
                if (isHost)
                {
                    StartHost();
                    StartLocalClient();
                }
                else
                {
                    StartRemoteClient();
                }
			}
        }
        else if (dataType == DataType.GetMatchList)
        {
            matchInfoList.Clear();

            int roomCount = im.ReadInt32();
            for (int i = 0; i < roomCount; ++i)
            {
                XNetMatchInfoSnapshot mi = new XNetMatchInfoSnapshot();
                mi.networkId = im.ReadString();
                mi.name = im.ReadString();
                mi.maxSize = im.ReadInt32();
                mi.currentSize = im.ReadInt32();

                matchInfoList.Add(mi);
            }

            isGetMatchListDone = true;
            if (cbListMatchs != null)
            {
                cbListMatchs(true, "", matchInfoList);
                cbListMatchs = null;
            }
        }
        else if (dataType == DataType.JoinMatchReturn)
        {
            bool success = im.ReadBoolean();
            string clientId = im.ReadString();
            Debug.Log("cmd joinmatchretrun>" + success.ToString() + ">" + clientId);

            if (IsHost())
            {
                var remoteClientToHostConn = new XNetConnection(clientId);
                remoteClientToHostConn.ForceInitialize(hostTopology);

                NetworkServer.AddExternalConnection(remoteClientToHostConn);
                AddConnection(remoteClientToHostConn);
            }

            // if send request client
            if (m_NextMatchJoinedCallback != null)
            {
                if (success)
                {
                    state = NetworkState.InLobby;
                }
                else
                {
                    state = NetworkState.Pregame;
                }

                m_NextMatchJoinedCallback(success, null);
                m_NextMatchJoinedCallback = null;

                var conn = new XNetConnection(clientId);
                Debug.Assert(myClient == null, "CHECK: myClient is not null");
                myClient = new XNetClient(conn);

                // Setup and connect
                myClient.RegisterHandler(MsgType.Connect, OnRemoteClientConnect);
                myClient.SetNetworkConnectionClass<XNetConnection>();
                myClient.Configure(hostTopology);
                ((XNetClient)myClient).Connect();
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
						this.OnServerConnected ();
					else if (status == NetConnectionStatus.Disconnected)
						this.OnServerDisconnected ();
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

        //myClient.RegisterHandler(MsgType.AddPlayer, OnServerAddPlayerMessageInternal);

        // Spawn self
        ClientScene.Ready(localClientToHostConn);
        //ClientScene.AddPlayer(0);
        SpawnPlayer(localClientToHostConn);

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

    //public void SendMessage(XNetMessage msg)
    //{
    //    // TODO
    //    // hard code
    //    Debug.Assert(netClient != null, "CHECK: netClient is null");

    //    NetOutgoingMessage om = netClient.CreateMessage(msg.Data.Length);
    //    System.Array.Copy(om.Data,  msg.Data, msg.Data.Length);
    //    netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 0);
    //}

    //public void SendMessage(NetOutgoingMessage msg)
    //{
    //    // TODO
    //    // hard code
    //    Debug.Assert(netClient != null, "CHECK: netClient is null");

    //    //NetOutgoingMessage om = netClient.CreateMessage(msg.Data.Length);
    //    //System.Array.Copy(om.Data, msg.Data, msg.Data.Length);
    //    netClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, 0);
    //}

    public NetClient GetNetClient()
    {
        return netClient;
    }

    public void Disconnect()
    {
    }

    #region MatchMaking

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
    #endregion

    public event System.Action<Tanks.Networking.NetworkPlayer> playerJoined;
    public event System.Action<Tanks.Networking.NetworkPlayer> playerLeft;
    public event System.Action serverPlayersReadied;
    public event System.Action<NetworkConnection> clientDisconnected;
    public event System.Action<NetworkConnection, int> clientError;
    public event System.Action<NetworkConnection, int> serverError;
    public event System.Action matchDropped;
    public event Action gameModeUpdated;
    public event Action clientStopped;

    private Action<bool, XNetMatchInfo> m_NextMatchJoinedCallback;

    public bool hasSufficientPlayers
    {
        get
        {
            // return isSinglePlayer ? playerCount >= 1 : playerCount >= 2;
            return false;
        }
    }

    public static bool s_IsServer
    {
        get
        {
            return NetworkServer.active;
        }
    }

    public virtual void OnPlayerSetReady(Tanks.Networking.NetworkPlayer player)
    {
        if (AllPlayersReady() && serverPlayersReadied != null)
        {
            serverPlayersReadied();
        }
    }

    protected virtual void UpdatePlayerIDs()
    {
        for (int i = 0; i < connectedPlayers.Count; ++i)
        {
            connectedPlayers[i].SetPlayerId(i);
        }
    }

    public void RegisterNetworkPlayer(Tanks.Networking.NetworkPlayer newPlayer)
    {
        MapDetails currentMap = m_Settings.map;
        Debug.Log("Player joined");

        connectedPlayers.Add(newPlayer);
        newPlayer.becameReady += OnPlayerSetReady;

        if (s_IsServer)
        {
            UpdatePlayerIDs();
        }

        // Send initial scene message
        string sceneName = SceneManager.GetActiveScene().name;
        if (currentMap != null && sceneName == currentMap.sceneName)
        {
            newPlayer.OnEnterGameScene();
        }
        else if (sceneName == s_LobbySceneName)
        {
            newPlayer.OnEnterLobbyScene();
        }

        if (playerJoined != null)
        {
            playerJoined(newPlayer);
        }

        newPlayer.gameDetailsReady += FireGameModeUpdated;
    }

    protected void FireGameModeUpdated()
    {
        if (gameModeUpdated != null)
        {
            gameModeUpdated();
        }
    }

    public void DeregisterNetworkPlayer(Tanks.Networking.NetworkPlayer removedPlayer)
    {

    }

    public void ProgressToGameScene()
    {
        //// Clear all client's ready states
        //ClearAllReadyStates();

        //// Remove us from matchmaking lists
        //UnlistMatch();

        //// Update will change scenes once loading screen is visible
        //m_SceneChangeMode = SceneChangeMode.Game;

        //// Tell NetworkPlayers to show their loading screens
        //for (int i = 0; i < connectedPlayers.Count; ++i)
        //{
        //    NetworkPlayer player = connectedPlayers[i];
        //    if (player != null)
        //    {
        //        player.RpcPrepareForLoad();
        //    }
        //}
    }

    public void ReturnToMenu(MenuPage returnPage)
    {
    }

    public bool AllPlayersReady()
    {
        return false;
    }

    public void ClearAllReadyStates()
    {
    }

    public void DisconnectAndReturnToMenu() { }

    public void JoinMatchmakingGame(string networkId, Action<bool, XNetMatchInfo> onJoin)
    {
        if (gameType != NetworkGameType.Matchmaking ||
            state != NetworkState.Pregame)
        {
            throw new InvalidOperationException("Game not in matching state. Make sure you call StartMatchmakingClient first.");
        }

        state = NetworkState.Connecting;

        m_NextMatchJoinedCallback = onJoin;
        // matchMaker.JoinMatch(networkId, string.Empty, string.Empty, string.Empty, 0, 0, OnMatchJoined);

        NetOutgoingMessage om = netClient.CreateMessage();
        om.Write(DataType.JoinMatch);
        om.Write(networkId);
        netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 0);
    }

    public bool isSinglePlayer
    {
        get
        {
            return gameType == NetworkGameType.Singleplayer;
        }
    }

    public bool isNetworkActive
    {
        get { return false;  }
    }

    public Tanks.Networking.NetworkGameType gameType
    {
        get;
        protected set;
    }

    private bool isGetMatchListDone = false;
    public delegate void DataResponseDelegate<T>(bool success, string extendedInfo, T responseData);
    DataResponseDelegate<List<XNetMatchInfoSnapshot>> cbListMatchs = null;
    // TODO:
    // use coroutine is better
    public void ListMatches(int startPageNumber, int resultPageSize, string matchNameFilter, bool filterOutPrivateMatchesFromResults, int eloScoreTarget, int requestDomain, DataResponseDelegate<List<XNetMatchInfoSnapshot>> callback)
    {
        if (isGetMatchListDone)
        {
            callback(true, "", matchInfoList);
        }
        else
        {
            cbListMatchs = callback;
        }
    }

    public enum NetworkState
    {
        Inactive,
        Pregame,
        Connecting,
        InLobby,
        InGame
    }

    public NetworkState state
    {
        get;
        protected set;
    }

    public void StartMatchingmakingClient()
    {
        if (state != NetworkState.Inactive)
        {
            throw new InvalidOperationException("Network currently active. Disconnect first.");
        }

        // minPlayers = 2;
        // maxPlayers = multiplayerMaxPlayers;

        state = NetworkState.Pregame;
        gameType = NetworkGameType.Matchmaking;
        StartMatchMaker();
    }

    public void StartMatchMaker()
    {
        isGetMatchListDone = false;

        if (IsConnectServer() == false)
        {
            ConnectServer(PVP_ADDRESS, PVP_PORT, (success) =>
            {
                NetOutgoingMessage om = netClient.CreateMessage();
                om.Write(DataType.GetMatchList);
                netClient.SendMessage(om, Lidgren.Network.NetDeliveryMethod.ReliableOrdered, 0);
            });
        }
        else
        {
            NetOutgoingMessage om = netClient.CreateMessage();
            om.Write(DataType.GetMatchList);
            netClient.SendMessage(om, Lidgren.Network.NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}
