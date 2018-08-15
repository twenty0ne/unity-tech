using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// connect state
public enum PeerState
{
	Connecting,
	Connected,
	Disconnecting,
	Disconnected,
}

// 
public enum StatusCode
{
	Connect,
}

// TODO
// 消息格式

// short - peerId
// byte - crcEnabled
// byte - udpCommandCount
// int - timeInt
// int - challenge
// - {
//		NetCommand {
//         CommndType

// 以上内容 ENet/Lidgren 已经封装，需要关注的是 Payload

//         Payload {
//				MsgType
//             OperationCode - 内部/外部
//             ReturnCode
//             DebugMessage
//             Parameters - ParameterCode
//			}
//		}
// - }

public class XNetPeer
{
	public XNetTransport transport;

	public PeerState peerState = PeerState.Disconnected;

	private byte[] dataBuffer;

	private Object incomingCommandsLock = new Object();
	private Object outgoingCommandsLock = new Object();
	public Queue<XNetCommand> incomingCommands = new Queue<XNetCommand>();
	public Queue<XNetCommand> outgoingCommands = new Queue<XNetCommand>();

	protected virtual bool Connect(string address, int port)
	{
		Debug.Assert(peerState == PeerState.Disconnected, "CHECK");
		peerState = PeerState.Connecting;

		transport = new ENetTransport();
		transport.peer = this;

		transport.Connect(address, port);

		return true;
	}

	public virtual void OnConnect()
	{
		// NOTE: thread
		Debug.Log("xx-- XNetPeer.OnConnect");
		// OnConnect 需要通知其他 Client
		// 如何区分连接不同的 server - 由 NetClient 处理连接不同 server 判断

		if (peerState == PeerState.Connecting)
			peerState = PeerState.Connected;

		PushCommand(new XNetCommand());
	}

	public virtual void OnDisconnect()
	{
		// NOTE: thread
		Debug.Log("xx-- XNetPeer.OnDisconnect");
	}

	public virtual void OnReceiveData(byte[] data, int length)
	{
		// NOTE: thread
		Debug.Log("xx-- XNetPeer.OnReceiveData");
	}

	public virtual void OnStatusChanged(StatusCode statusCode)
	{

	}

	protected void PushCommand(XNetCommand cmd)
	{
		lock(incomingCommandsLock)
		{
			incomingCommands.Enqueue(cmd);
		}
	}

	public virtual void SendOutgoingCommands()
	{
		
	}

	public virtual void HandleIncomingCommands()
	{
		lock(incomingCommandsLock)
		{
			while (incomingCommands.Count > 0)
			{
				XNetCommand cmd = incomingCommands.Dequeue();
				
			}
		}
	}
}
