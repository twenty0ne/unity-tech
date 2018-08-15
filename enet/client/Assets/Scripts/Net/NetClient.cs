//using System.Collections;
//using System.Collections.Generic;
//using System;
//using System.Threading;
//using ENet;
//using UnityEngine;

//public class NetClient
//{
//	private Host host = null;
//	private Peer peer;
//	private bool bInit = false;

//	public void Connect(string ip, int port)
//	{
//		bInit = true;

//		host = new Host();
//		host.Create(null, 1);

//		var address = new Address();
//		address.SetHost(ip);
//		address.Port = (ushort)port;

//		peer = host.Connect(address, 200, 1234);
//	}

//	public void SendPacket(Packet pkt, byte chan)
//	{
//		if (!bInit)
//		{
//			UnityEngine.Debug.LogWarning("frist call Connect to init");
//			return;
//		}

//		peer.Send(chan, pkt);
//	}

//	public void ReceivePackets(System.Action<byte, ENet.Packet> funcParsePacket)
//	{
//		if (host.Service(1) >= 0)
//		{
//			ENet.Event evt;
//			while (host.CheckEvents(out evt) > 0)
//			{
//				switch (evt.Type)
//				{
//					case ENet.EventType.Receive:
//						{
//							// var data = evt.Packet.GetBytes();
//							// var value = BitConverter.ToUInt16(data, 0);
//							// evt.Packet.Dispose();
//							// var data = evt.Packet.GetBytes();
//							// peer.Send(evt.ChannelID, BitConverter.GetBytes(value));
//							if (funcParsePacket != null)
//							{
//								funcParsePacket(evt.ChannelID, evt.Packet);
//							}
//							evt.Packet.Dispose();
//						}
//						break;
//				}
//			}
//		}
//	}
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 与游戏相关的状态
public enum ClientState
{
	None,
	ConnectingToMasterServer,
	ConnectedToMasterServer,
	ConnectingToGameServer,
	ConnectedToGameServer,
}

public enum ServerType
{
	None,
	MasterServer,
	GameServer,	// relay server
}

public enum NetMessageMethod
{
	OnConnectedToMaster,
}

public enum OpCode : byte
{
	JoinRoom = 0,

}

public class NetClient : XNetPeer
{
	public string hostPlayerId;

	// ClientState 与 PeerState 区别是：
	// ClientState 是在  PeerState 之上的进一步扩展，更多的 Lobby，Room 等具体游戏相关
	public ClientState clientState = ClientState.None;

	public ServerType cachedServerType = ServerType.None;
	public string cachedServerAddress = "";
	public int cachedServerPort = 0;

	public static void SendNetMessage(NetMessageMethod method, params object[] parameters)
	{
		Object[] objs = GameObject.FindObjectsOfType(typeof(MonoBehaviour));

		foreach (GameObject obj in objs)
		{
			if (obj != null)
				obj.SendMessage(method.ToString(), parameters, SendMessageOptions.DontRequireReceiver);
		}
	}

	public override void OnStatusChanged(StatusCode statusCode)
	{
		Debug.Log(string.Format("OnStatusChanged: {0} current State: {1}", statusCode.ToString(), clientState.ToString()));

		switch (statusCode)
		{
			case StatusCode.Connect:
				{
					if (clientState == ClientState.ConnectingToMasterServer)
					{
						clientState = ClientState.ConnectedToMasterServer;
						SendNetMessage(NetMessageMethod.OnConnectedToMaster);
					}
					else if (clientState == ClientState.ConnectingToGameServer)
					{
						clientState = ClientState.ConnectedToGameServer;
					}
				}
				break;
		}
	}

	public bool Connect(string address, int port, ServerType stype)
	{
		base.Connect(address, port);

		cachedServerType = stype;
		cachedServerAddress = address;
		cachedServerPort = port;

		if (stype == ServerType.MasterServer)
			clientState = ClientState.ConnectingToMasterServer;
		else if (stype == ServerType.GameServer)
			clientState = ClientState.ConnectingToGameServer ;
		else
			Debug.Assert(false, "CHECK");

		return true;
	}

	public void NewSceneLoaded()
	{
		
	}

	//public void SendOutgoingCommands()
	//{
	//}

	//public void ReceiveIncomingCommands()
	//{
	//}
	
	public void JoinRoom()
}
