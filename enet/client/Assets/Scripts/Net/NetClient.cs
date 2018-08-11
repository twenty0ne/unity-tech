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
	ConnectingTo
}

public class NetClient : XNetPeer
{
	public string hostPlayerId;

	// ClientState 与 PeerState 区别是：
	// ClientState 是在  PeerState 之上的进一步扩展，更多的 Lobby，Room 等具体游戏相关
	public ClientState clientState = ClientState.None;

	public void NewSceneLoaded()
	{

	}
}
