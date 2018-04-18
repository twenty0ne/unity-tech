//using UnityEngine;
//using System.Collections;
//using UnityEngine.Networking;
//using System.Collections.Generic;
//using System.Net;
//using UnityEngine.Networking.NetworkSystem;
//using System;
//using Steamworks;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Threading;
//using System.Net.Sockets;
//using System.Text;

//using LiteNetLib;
//using LiteNetLib.Utils;

//public class MyNetworkManager : MonoBehaviour, INetEventListener
//{
//    public const int MAX_USERS = 2;
//    // public const string GAME_ID = "spacewave-unet-p2p-example"; // Unique identifier for matchmaking so we don't match up with other Spacewar games

//    public enum SessionConnectionState
//    {
//        UNDEFINED,
//        CONNECTING,
//        CANCELLED,
//        CONNECTED,
//        FAILED,
//        DISCONNECTING,
//        DISCONNECTED
//    }

//    public static MyNetworkManager Instance;

//    [SerializeField]
//    private MyUNETServerController UNETServerController;
//    public List<GameObject> networkPrefabs;

//    // Client-to-server connection
//    public NetworkClient myClient;

//    // steam state vars
//    public SessionConnectionState lobbyConnectionState { get; private set; }
//    // [HideInInspector]
//    // public CSteamID steamLobbyId;

//    // callbacks
//    //private Callback<LobbyEnter_t> m_LobbyEntered;
//    //private Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;
//    //private Callback<LobbyChatUpdate_t> m_LobbyChatUpdate;
//    //private CallResult<LobbyMatchList_t> m_LobbyMatchList;

//    private static HostTopology m_hostTopology = null;
//    public static HostTopology hostTopology
//    {
//        get
//        {
//            if (m_hostTopology == null)
//            {
//                ConnectionConfig config = new ConnectionConfig();
//                config.AddChannel(QosType.ReliableSequenced);
//                config.AddChannel(QosType.Unreliable);
//                m_hostTopology = new HostTopology(config, MAX_USERS);

//            }

//            return m_hostTopology;
//        }
//    }

//    public static int GetChannelCount()
//    {
//        return hostTopology.DefaultConfig.Channels.Count;
//    }

//    int socketId;
//    int connectionId;
//    int myReliableChannelId;

//    //以下默认都是私有的成员  
//    Socket socket; //目标socket  
//    EndPoint serverEnd; //服务端  
//    IPEndPoint ipEnd; //服务端端口  
//    string recvStr; //接收的字符串  
//    string sendStr; //发送的字符串  
//    byte[] recvData = new byte[1024]; //接收的数据，必须为字节  
//    byte[] sendData = new byte[1024]; //发送的数据，必须为字节  
//    int recvLen; //接收的数据长度  
//    Thread connectThread; //连接线程  


//    //初始化  
//    void InitSocket()
//    {
//        //定义连接的服务器ip和端口，可以是本机ip，局域网，互联网  
//        ipEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 41234);
//        //定义套接字类型,在主线程中定义  
//        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
//        //定义服务端  
//        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
//        serverEnd = (EndPoint)sender;
//        print("waiting for sending UDP dgram");

//        //建立初始连接，这句非常重要，第一次连接初始化了serverEnd后面才能收到消息  
//        SocketSend("connect");

//        //开启一个线程连接，必须的，否则主线程卡死  
//        connectThread = new Thread(new ThreadStart(SocketReceive));
//        connectThread.Start();
//    }

//    public void SocketSend(string sendStr)
//    {
//        //清空发送缓存  
//        sendData = new byte[1024];
//        //数据类型转换  
//        sendData = Encoding.ASCII.GetBytes(sendStr);
//        //发送给指定服务端  
//        socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
//    }

//    //服务器接收  
//    public void SocketReceive()
//    {
//        //进入接收循环  
//        while (true)
//        {
//            //对data清零  
//            recvData = new byte[1024];
//            //获取客户端，获取服务端端数据，用引用给服务端赋值，实际上服务端已经定义好并不需要赋值  
//            recvLen = socket.ReceiveFrom(recvData, ref serverEnd);
//            print("message from: " + serverEnd.ToString()); //打印服务端信息  
//            //输出接收到的数据  
//            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
//            print(recvStr);
//        }
//    }

//    //连接关闭  
//    void SocketQuit()
//    {
//        //关闭线程  
//        if (connectThread != null)
//        {
//            connectThread.Interrupt();
//            connectThread.Abort();
//        }
//        //最后关闭socket  
//        if (socket != null)
//            socket.Close();
//    }

//    private NetManager _netClient;

//    void Start()
//    {
//        // init
//        Instance = this;
//        DontDestroyOnLoad(this);

//        LogFilter.currentLogLevel = LogFilter.Info;

//        //if (SteamManager.Initialized)
//        //{
//        //    m_LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
//        //    m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
//        //    m_LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
//        //    m_LobbyMatchList = CallResult<LobbyMatchList_t>.Create(OnLobbyMatchList);
//        //}

//        UNETServerController.Init();

//        // check if game started via friend invitation
//        //string[] args = System.Environment.GetCommandLineArgs();
//        //string input = "";
//        //for (int i = 0; i < args.Length; i++)
//        //{
//        //    if (args[i] == "+connect_lobby" && args.Length > i + 1)
//        //    {
//        //        input = args[i + 1];
//        //    }
//        //}

//        //if (!string.IsNullOrEmpty(input))
//        //{
//        //    // Invite accepted, launched game. Join friend's game
//        //    ulong lobbyId = 0;

//        //    if (ulong.TryParse(input, out lobbyId))
//        //    {
//        //        JoinLobby(new CSteamID(lobbyId));
//        //    }

//        //}

//        _netClient = new NetManager(this);
//        _netClient.Start();
//        _netClient.UpdateTime = 15;
//        _netClient.Connect("127.0.0.1", 41234, "test");

//        //InitSocket();
//        return;

//        NetworkTransport.Init();
//        // 设置最大数据包为500
//        GlobalConfig globalconfig = new GlobalConfig();
//        globalconfig.MaxPacketSize = 500;
//        NetworkTransport.Init(globalconfig);

//        ConnectionConfig config = new ConnectionConfig();
//        // QosType.Reliable 可靠连接，确保信息传输安全
//        byte reliableByteId = config.AddChannel(QosType.Reliable);
//        // QosType.Unreliable 不可靠连接，传输速度快，不能保证信息传递
//        byte unreliableByteId = config.AddChannel(QosType.Unreliable);
//        // 网络拓扑的定义， （连接使用配置信息，允许连接数）
//        HostTopology topology = new HostTopology(config, 10);

//        // 创建主机服务(端口号：8888)
//        int intHostID = NetworkTransport.AddHost(topology, 8888);

//        // 先将不同的命令发送主机并检查状态
//        // 返回一个给此链接的ID
//        byte error;
//        int connetionID = NetworkTransport.Connect(intHostID, "127.0.0.1", 41234, 0, out error);
//        // 此命令发送断开链接的请求
//        // NetworkTransport.Disconnect(intHostID, connetionID, out error);

//        //// 发送信息，将消息存储在缓存区消息长度为bufferLength
//        // NetworkTransport.Send(intHostID, connetionID, reliableByteId, buffer, bufferLength, out error);

//        socketId = intHostID;
//        connectionId = connetionID;
//        myReliableChannelId = reliableByteId;
//    }

//    public void SendSocketMessage()
//    {
//        SocketSend("hello world");
//        return;

//        byte error;
//        byte[] buffer = new byte[1024];
//        Stream stream = new MemoryStream(buffer);
//        BinaryFormatter formatter = new BinaryFormatter();
//        formatter.Serialize(stream, "HelloServer");

//        int bufferSize = 1024;

//        NetworkTransport.Send(socketId, connectionId, myReliableChannelId, buffer, bufferSize, out error);
//    }

//    private void OnDestroy()
//    {
//        // SocketQuit();

//        if (_netClient != null)
//            _netClient.Stop();
//    }

//    void Update()
//    {
//        //if (!SteamManager.Initialized)
//        //{
//        //    return;
//        //}

//        if (_netClient != null)
//        {
//            _netClient.PollEvents();
//        }

//        return;

//        int recHostId;
//        int recConnectionId;
//        int recChannelId;
//        byte[] recBuffer = new byte[1024];
//        int bufferSize = 1024;
//        int dataSize;
//        byte error;
//        NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, recBuffer, bufferSize, out dataSize, out error);
//        switch (recNetworkEvent)
//        {
//            case NetworkEventType.Nothing:
//                break;
//            case NetworkEventType.ConnectEvent:
//                Debug.Log("incoming connection event received");
//                break;
//            case NetworkEventType.DataEvent:
//                Stream stream = new MemoryStream(recBuffer);
//                BinaryFormatter formatter = new BinaryFormatter();
//                string message = formatter.Deserialize(stream) as string;
//                Debug.Log("incoming message event received: " + message);
//                break;
//            case NetworkEventType.DisconnectEvent:
//                Debug.Log("remote client event disconnected");
//                break;
//        }

//        return;

//        if (!IsConnectedToUNETServer())
//        {
//            return;
//        }

//        uint packetSize;
//        int channels = GetChannelCount();

//        // Read Steam packets
//        for (int chan = 0; chan < channels; chan++)
//        {
//            while (SteamNetworking.IsP2PPacketAvailable(out packetSize, chan))
//            {
//                byte[] data = new byte[packetSize];

//                CSteamID senderId;

//                if (SteamNetworking.ReadP2PPacket(data, packetSize, out packetSize, out senderId, chan))
//                {
//                    NetworkConnection conn;

//                    if (UNETServerController.IsHostingServer())
//                    {
//                        // We are the server, one of our clients will handle this packet
//                        conn = UNETServerController.GetClient(senderId);

//                        if (conn == null)
//                        {
//                            // In some cases the p2p connection can persist, resulting in UNETServerController.OnP2PSessionRequested not being called. This happens usually when testing in editor.
//                            // If the peers have already established a connection, reset it.
//                            P2PSessionState_t sessionState;
//                            if (SteamNetworking.GetP2PSessionState(senderId, out sessionState) && Convert.ToBoolean(sessionState.m_bConnectionActive))
//                            {
//                                Debug.Log("P2P connection is still established. Resetting.");
//                                SteamNetworking.CloseP2PSessionWithUser(senderId);
//                                UNETServerController.CreateP2PConnectionWithPeer(senderId);
//                                conn = UNETServerController.GetClient(senderId);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        // We are a client, we only have one connection (the server).
//                        conn = myClient.connection;
//                    }

//                    if (conn != null)
//                    {
//                        // Handle Steam packet through UNET
//                        conn.TransportReceive(data, Convert.ToInt32(packetSize), chan);
//                    }

//                }
//            }
//        }

//        //if (Input.GetKeyDown(KeyCode.Escape))
//        //{
//        //    Disconnect();
//        //}
//    }

//    public void RegisterNetworkPrefabs()
//    {
//        for (int i = 0; i < networkPrefabs.Count; i++)
//        {
//            ClientScene.RegisterPrefab(networkPrefabs[i]);
//        }
//    }

//    //public bool IsMemberInSteamLobby(CSteamID steamUser)
//    //{
//    //    if (SteamManager.Initialized)
//    //    {
//    //        int numMembers = SteamMatchmaking.GetNumLobbyMembers(steamLobbyId);

//    //        for (int i = 0; i < numMembers; i++)
//    //        {
//    //            var member = SteamMatchmaking.GetLobbyMemberByIndex(steamLobbyId, i);

//    //            if (member.m_SteamID == steamUser.m_SteamID)
//    //            {
//    //                return true;
//    //            }
//    //        }
//    //    }

//    //    return false;
//    //}

//    //public CSteamID GetSteamIDForConnection(NetworkConnection conn)
//    //{
//    //    if (UNETServerController.IsHostingServer())
//    //    {
//    //        return UNETServerController.GetSteamIDForConnection(conn);
//    //    }
//    //    else
//    //    {
//    //        // clients only have the client-to-server connection
//    //        var steamConn = myClient as SteamNetworkClient;
//    //        if (steamConn != null)
//    //        {
//    //            return steamConn.steamConnection.steamId;
//    //        }
//    //    }

//    //    Debug.LogError("Could not find Steam ID");
//    //    return CSteamID.Nil;
//    //}

//    public bool IsConnectedToUNETServer()
//    {
//        return myClient != null && myClient.connection != null && myClient.connection.isConnected;
//    }

//    public void Disconnect()
//    {
//        lobbyConnectionState = SessionConnectionState.DISCONNECTED;

//        ClientScene.DestroyAllClientObjects();

//        //if (SteamManager.Initialized)
//        //{
//        //    SteamMatchmaking.LeaveLobby(steamLobbyId);
//        //}

//        if (myClient != null)
//        {
//            myClient.Disconnect();
//            myClient = null;
//        }

//        UNETServerController.Disconnect();
//        NetworkClient.ShutdownAll();

//        //steamLobbyId.Clear();
//    }


//    //void OnLobbyChatUpdate(LobbyChatUpdate_t pCallback)
//    //{
//    //    if (pCallback.m_rgfChatMemberStateChange == (uint)EChatMemberStateChange.k_EChatMemberStateChangeLeft && pCallback.m_ulSteamIDLobby == steamLobbyId.m_SteamID)
//    //    {
//    //        Debug.Log("A client has disconnected from the UNET server");

//    //        // user left lobby
//    //        var userId = new CSteamID(pCallback.m_ulSteamIDUserChanged);
//    //        if (UNETServerController.IsHostingServer())
//    //        {
//    //            UNETServerController.RemoveConnection(userId);
//    //        }

//    //        SteamNetworking.CloseP2PSessionWithUser(userId);
//    //    }
//    //}


//    //void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t pCallback)
//    //{
//    //    // Invite accepted, game is already running
//    //    JoinLobby(pCallback.m_steamIDLobby);
//    //}

//    //public void JoinLobby(CSteamID lobbyId)
//    //{
//    //    if (!SteamManager.Initialized)
//    //    {
//    //        lobbyConnectionState = SessionConnectionState.FAILED;
//    //        return;
//    //    }

//    //    lobbyConnectionState = SessionConnectionState.CONNECTING;
//    //    SteamMatchmaking.JoinLobby(lobbyId);
//    //    // ...continued in OnLobbyEntered callback
//    //}

//    //public void InviteFriendsToLobby()
//    //{
//    //    if (lobbyConnectionState == SessionConnectionState.CONNECTING)
//    //    {
//    //        // Already trying to connect...
//    //        return;
//    //    }

//    //    if (lobbyConnectionState != SessionConnectionState.CONNECTED)
//    //    {
//    //        // No lobby yet
//    //        CreateLobbyAndInviteFriend();
//    //    }
//    //    else
//    //    {
//    //        // Already in lobby. Invite friends to current lobby
//    //        UNETServerController.InviteFriendsToLobby();
//    //    }
//    //}

//    //public void CreateLobbyAndInviteFriend()
//    //{
//    //    if (!SteamManager.Initialized)
//    //    {
//    //        lobbyConnectionState = SessionConnectionState.FAILED;
//    //        return;
//    //    }

//    //    UNETServerController.inviteFriendOnStart = true;
//    //    lobbyConnectionState = SessionConnectionState.CONNECTING;
//    //    SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePrivate, MAX_USERS);
//    //    // ...continued in OnLobbyEntered callback
//    //}

//    //public void FindMatch()
//    //{
//    //    if (!SteamManager.Initialized)
//    //    {
//    //        lobbyConnectionState = SessionConnectionState.FAILED;
//    //        return;
//    //    }

//    //    lobbyConnectionState = SessionConnectionState.CONNECTING;

//    //    //Note: call SteamMatchmaking.AddRequestLobbyList* before RequestLobbyList to filter results by some criteria
//    //    SteamMatchmaking.AddRequestLobbyListStringFilter("game", GAME_ID, ELobbyComparison.k_ELobbyComparisonEqual);
//    //    var call = SteamMatchmaking.RequestLobbyList();
//    //    m_LobbyMatchList.Set(call, OnLobbyMatchList);
//    //}

//    //void OnLobbyMatchList(LobbyMatchList_t pCallback, bool bIOFailure)
//    //{
//    //    uint numLobbies = pCallback.m_nLobbiesMatching;

//    //    if (numLobbies <= 0)
//    //    {
//    //        // no lobbies found. create one
//    //        Debug.Log("Creating lobby");

//    //        UNETServerController.inviteFriendOnStart = false;
//    //        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, MAX_USERS);
//    //        // ...continued in OnLobbyEntered callback
//    //    }
//    //    else
//    //    {
//    //        // If multiple lobbies are returned we can iterate over them with SteamMatchmaking.GetLobbyByIndex and choose the "best" one
//    //        // In this case we are just joining the first one
//    //        Debug.Log("Joining lobby");
//    //        var lobby = SteamMatchmaking.GetLobbyByIndex(0);
//    //        JoinLobby(lobby);
//    //    }


//    //}

//    //void OnLobbyEntered(LobbyEnter_t pCallback)
//    //{
//    //    if (!SteamManager.Initialized)
//    //    {
//    //        lobbyConnectionState = SessionConnectionState.FAILED;
//    //        return;
//    //    }

//    //    steamLobbyId = new CSteamID(pCallback.m_ulSteamIDLobby);

//    //    Debug.Log("Connected to Steam lobby");
//    //    lobbyConnectionState = SessionConnectionState.CONNECTED;

//    //    var hostUserId = SteamMatchmaking.GetLobbyOwner(steamLobbyId);
//    //    var me = SteamUser.GetSteamID();
//    //    if (hostUserId.m_SteamID == me.m_SteamID)
//    //    {
//    //        SteamMatchmaking.SetLobbyData(steamLobbyId, "game", GAME_ID);
//    //        UNETServerController.StartUNETServer();
//    //    }
//    //    else
//    //    {
//    //        // joined friend's lobby.
//    //        StartCoroutine(RequestP2PConnectionWithHost());
//    //    }


//    //}

//    //IEnumerator RequestP2PConnectionWithHost()
//    //{
//    //    var hostUserId = SteamMatchmaking.GetLobbyOwner(steamLobbyId);

//    //    //send packet to request connection to host via Steam's NAT punch or relay servers
//    //    Debug.Log("Sending packet to request P2P connection");
//    //    SteamNetworking.SendP2PPacket(hostUserId, null, 0, EP2PSend.k_EP2PSendReliable);

//    //    Debug.Log("Waiting for P2P acceptance message");
//    //    uint packetSize;
//    //    while (!SteamNetworking.IsP2PPacketAvailable(out packetSize))
//    //    {
//    //        yield return null;
//    //    }

//    //    byte[] data = new byte[packetSize];

//    //    CSteamID senderId;

//    //    if (SteamNetworking.ReadP2PPacket(data, packetSize, out packetSize, out senderId))
//    //    {
//    //        if (senderId.m_SteamID == hostUserId.m_SteamID)
//    //        {
//    //            Debug.Log("P2P connection established");

//    //            // packet was from host, assume it's notifying client that AcceptP2PSessionWithUser was called
//    //            P2PSessionState_t sessionState;
//    //            if (SteamNetworking.GetP2PSessionState(hostUserId, out sessionState))
//    //            {
//    //                // connect to the unet server
//    //                ConnectToUnetServerForSteam(hostUserId);

//    //                yield break;
//    //            }

//    //        }
//    //    }

//    //    Debug.LogError("Connection failed");
//    //}


//    //void ConnectToUnetServerForSteam(CSteamID hostSteamId)
//    //{
//    //    Debug.Log("Connecting to UNET server");

//    //    // Create connection to host player's steam ID
//    //    var conn = new SteamNetworkConnection(hostSteamId);
//    //    var mySteamClient = new SteamNetworkClient(conn);
//    //    this.myClient = mySteamClient;

//    //    // Setup and connect
//    //    mySteamClient.RegisterHandler(MsgType.Connect, OnConnect);
//    //    mySteamClient.SetNetworkConnectionClass<SteamNetworkConnection>();
//    //    mySteamClient.Configure(SteamNetworkManager.hostTopology);
//    //    mySteamClient.Connect();

//    //}

//    //void OnConnect(NetworkMessage msg)
//    //{
//    //    // Set to ready and spawn player
//    //    Debug.Log("Connected to UNET server.");
//    //    myClient.UnregisterHandler(MsgType.Connect);

//    //    RegisterNetworkPrefabs();

//    //    var conn = myClient.connection;
//    //    if (conn != null)
//    //    {
//    //        ClientScene.Ready(conn);
//    //        Debug.Log("Requesting spawn");
//    //        myClient.Send(NetworkMessages.SpawnRequestMsg, new StringMessage(SteamUser.GetSteamID().m_SteamID.ToString()));
//    //    }

//    //}

//    public void OnPeerConnected(NetPeer peer)
//    {
//        Debug.Log("[CLIENT] We connected to " + peer.EndPoint);
//    }

//    public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
//    {
//        Debug.Log("[CLIENT] We received error " + socketErrorCode);
//    }

//    public void OnNetworkReceive(NetPeer peer, NetDataReader reader, DeliveryMethod deliveryMethod)
//    {
//        //_newBallPosX = reader.GetFloat();

//        //var pos = _clientBall.transform.position;

//        //_oldBallPosX = pos.x;
//        //pos.x = _newBallPosX;

//        //_clientBall.transform.position = pos;

//        //_lerpTime = 0f;
//    }

//    public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader,
//        UnconnectedMessageType messageType)
//    {
//        if (messageType == UnconnectedMessageType.DiscoveryResponse && _netClient.PeersCount == 0)
//        {
//            Debug.Log("[CLIENT] Received discovery response. Connecting to: " + remoteEndPoint);
//            _netClient.Connect(remoteEndPoint, "sample_app");
//        }
//    }

//    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
//    {

//    }

//    public void OnConnectionRequest(ConnectionRequest request)
//    {

//    }

//    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
//    {
//        Debug.Log("[CLIENT] We disconnected because " + disconnectInfo.Reason);
//    }
//}

using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using Lidgren.Network;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Networking.NetworkSystem;

public class DATA_TYPE
{
    public const int DAT_CONNECT = 0;
    public const int DAT_UNET = 1;
    public const int DAT_ACCEPT = 2;
}

public class MyNetworkManager : MonoBehaviour
{

    public NetClient _netClient;

    public static MyNetworkManager instance;

    // Client-to-server connection
    public NetworkClient myClient;

    //private MyUNETServerController UNETServerController;
    public List<GameObject> networkPrefabs;

    public GameObject playerPrefab;
    // private List<NetworkConnection> connectedClients = new List<NetworkConnection>();

    public NetworkConnection connClient = null;
    public NetworkConnection connRemote = null;

    private bool isHost = false;

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

    private void Start()
    {
        instance = this;

        //UNETServerController.Init();

        LoomUtil kk = LoomUtil.Current;
    }

    private void OnDestroy()
    {
        if (_netClient != null)
        {
           _netClient.Disconnect("bye");
        }
    }

    //public void OnClickConnect()
    //{
    //    _netClient.Start();
    //    NetOutgoingMessage hail = _netClient.CreateMessage("this is the hail message");
    //    _netClient.Connect("127.0.0.1", 14242, hail);
    //}

    //public void OnClickSend()
    //{
    //    NetOutgoingMessage om = _netClient.CreateMessage("hello world");
    //    _netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
    //    //_netClient.FlushSendQueue();
    //}

    private void Update()
    {
        if (_netClient != null)
        {
            OnGotMessage(null);
        }
    }

    public bool Send(byte[] bytes, int numBytes, int channelId)
    {
        NetOutgoingMessage om = _netClient.CreateMessage();
        om.Write(DATA_TYPE.DAT_UNET);
        om.Write(numBytes);
        om.Write(bytes);
		if (channelId == 0)
			_netClient.SendMessage(om, NetDeliveryMethod.ReliableSequenced, channelId);
		else if (channelId == 1)
			_netClient.SendMessage(om, NetDeliveryMethod.Unreliable, 0);
		else
			Debug.Assert(false, "CHECK");

        return true;
    }

    private void OnGotMessage(object peer)
    {
        NetIncomingMessage im;
        while ((im = MyNetworkManager.instance._netClient.ReadMessage()) != null)
        {
            // handle incoming message
            switch (im.MessageType)
            {
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.ErrorMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.VerboseDebugMessage:
                    string text = im.ReadString();
                    Debug.Log("output: " + text);
                    break;
                case NetIncomingMessageType.StatusChanged:
                    NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

                    //if (status == NetConnectionStatus.Connected)
                    //    s_form.EnableInput();
                    //else
                    //    s_form.DisableInput();

                    //if (status == NetConnectionStatus.Disconnected)
                    //    s_form.button2.Text = "Connect";

                    string reason = im.ReadString();
                    Debug.Log(status.ToString() + ": " + reason);

                    break;
                case NetIncomingMessageType.Data:
                    //string chat = im.ReadString();
                    //Debug.Log(chat);

                    {
                        int dataType = im.ReadInt32();
                        if (dataType == DATA_TYPE.DAT_CONNECT)
                        {
                            string client_id = im.ReadString();

                            //LoomUtil.RunAsync(() => {
                                var newConn = new MyNetworkConnection();
                                newConn.ForceInitialize();

                                NetworkServer.AddExternalConnection(newConn);
                                connRemote = newConn;
                            //});

                            // send accept
                            NetOutgoingMessage om = _netClient.CreateMessage();
                            om.Write(DATA_TYPE.DAT_ACCEPT);
                            om.Write(client_id);
                            _netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 0);
                        }
                        else if (dataType == DATA_TYPE.DAT_UNET)
                        {
                            int byteLength = im.ReadInt32();
                            byte[] byteArray = im.ReadBytes(byteLength);

                            NetworkConnection conn = isHost ? connRemote : myClient.connection;

                            if (conn != null)
                            {
								Debug.Log("xx-- TransportReceive > " + im.SequenceChannel.ToString());
                                conn.TransportReceive(byteArray, byteLength, im.SequenceChannel);
                            }
                        }
                        else if (dataType == DATA_TYPE.DAT_ACCEPT)
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
                    }

                    break;
                case NetIncomingMessageType.ConnectionLatencyUpdated:
                    // heart beat 
                    break;
                default:
                    Debug.Log("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
                    break;
            }
            MyNetworkManager.instance._netClient.Recycle(im);
        }
    }

    void OnConnect(NetworkMessage msg)
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
            myClient.Send(NetworkMessages.SpawnRequestMsg, new StringMessage(""));
        }

    }

    public void RegisterNetworkPrefabs()
    {
        for (int i = 0; i < networkPrefabs.Count; i++)
        {
            ClientScene.RegisterPrefab(networkPrefabs[i]);
        }
    }

    private void StartUNETServer()
    {
        Debug.Log("Starting UNET server");

        // Listen for player spawn request messages 
        NetworkServer.RegisterHandler(NetworkMessages.SpawnRequestMsg, OnSpawnRequested);

        // Start UNET server
        NetworkServer.Configure(MyNetworkManager.hostTopology);
        NetworkServer.dontListen = true;
        NetworkServer.Listen(0);

        // Create a local client-to-server connection to the "server"
        // Connect to localhost to trick UNET's ConnectState state to "Connected", which allows data to pass through TransportSend
        myClient = ClientScene.ConnectLocalServer();
        myClient.Configure(MyNetworkManager.hostTopology);
        myClient.Connect("localhost", 0);
        myClient.connection.ForceInitialize();

        // Add local client to server's list of connections
        // Here we get the connection from the NetworkServer because it represents the server-to-client connection
        var serverToClientConn = NetworkServer.connections[0];
        // AddConnection(serverToClientConn);
        connClient = serverToClientConn;

        // register networked prefabs
        RegisterNetworkPrefabs();

        // Spawn self
        ClientScene.Ready(serverToClientConn);
        SpawnPlayer(serverToClientConn);
    }

    //public void AddConnection(NetworkConnection conn)
    //{
    //    connectedClients.Add(conn);
    //}

    //public void RemoveConnection(CSteamID steamId)
    //{
    //    var conn = GetClient(steamId);
    //    var steamConn = conn as SteamNetworkConnection;

    //    if (conn != null)
    //    {
    //        conn.InvokeHandlerNoData(MsgType.Disconnect);

    //        if (steamConn != null)
    //        {
    //            steamConn.CloseP2PSession();
    //        }

    //        DestroyPlayer(conn);
    //        connectedClients.Remove(conn);

    //        conn.hostId = -1;
    //        conn.Disconnect();
    //        conn.Dispose();
    //        conn = null;
    //    }

    //}

    bool SpawnPlayer(NetworkConnection conn)
    {
        NetworkServer.SetClientReady(conn);
        var player = GameObject.Instantiate(playerPrefab);

        return NetworkServer.SpawnWithClientAuthority(player, conn);
    }

    void DestroyPlayer(NetworkConnection conn)
    {
        if (conn == null)
        {
            return;
        }

        // quick and dirty hack to destroy a player. GameObject.FindObjectsOfType probably shouldn't be used here.
        var objs = GameObject.FindObjectsOfType<NetworkIdentity>();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].clientAuthorityOwner != null && objs[i].clientAuthorityOwner.connectionId == conn.connectionId)
            {
                NetworkServer.Destroy(objs[i].gameObject);
            }
        }
    }

    void OnSpawnRequested(NetworkMessage msg)
    {
        Debug.Log("Spawn request received");

        // Read the contents of this message. It should contain the steam ID of the sender
        //var strMsg = msg.ReadMessage<StringMessage>();
        //if (strMsg != null)
        {
            ulong steamId;
            //if (ulong.TryParse(strMsg.value, out steamId))
            {
                var conn = connRemote;

                if (conn != null)
                {
                    // spawn peer
                    if (SpawnPlayer(conn))
                    {
                        Debug.Log("Spawned player");
                        return;
                    }
                }
            }
        }

        Debug.LogError("Failed to spawn player");
    }

    public void CreateHost()
    {
        isHost = true;

        // UNETServerController.StartUNETServer();
        StartUNETServer();

        // connect to game server
        NetPeerConfiguration config = new NetPeerConfiguration("chat");
        config.EnableMessageType(NetIncomingMessageType.ConnectionLatencyUpdated);
        // config.AutoFlushSendQueue = false;
        _netClient = new NetClient(config);

       // _netClient.RegisterReceivedCallback(new SendOrPostCallback(OnGotMessage), new SynchronizationContext());

        _netClient.Start();
        NetOutgoingMessage hail = _netClient.CreateMessage("0wI4g5Q8");     // 0wI4g5Q8 对战房编号
        _netClient.Connect("127.0.0.1", 14242, hail);
    }

    public void ConnectHost()
    {
        // connect to game server
        NetPeerConfiguration config = new NetPeerConfiguration("chat");
        config.EnableMessageType(NetIncomingMessageType.ConnectionLatencyUpdated);
        // config.AutoFlushSendQueue = false;
        _netClient = new NetClient(config);

        // _netClient.RegisterReceivedCallback(new SendOrPostCallback(OnGotMessage), new SynchronizationContext());

        _netClient.Start();
        NetOutgoingMessage hail = _netClient.CreateMessage("0wI4g5Q8");     // 0wI4g5Q8 对战房编号
        _netClient.Connect("127.0.0.1", 14242, hail);
    }

    public bool IsHost()
    {
        return isHost;
    }
    
}