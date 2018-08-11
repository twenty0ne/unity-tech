using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PeerState
{
	Connecting,
	Connected,
	Disconnecting,
	Disconnected,
}

public class XNetPeer
{
	public XNetTransport transport;

	public PeerState peerState = PeerState.Disconnected;

	// public

	public void Connect(string address, int port)
	{
		transport = new ENetTransport();
		transport.peer = this;

		transport.Connect(address, port);
	}

	public virtual void OnConnect()
	{
		Debug.Log("xx-- XNetPeer.OnConnect");
	}

	public virtual void OnDisconnect()
	{
		Debug.Log("xx-- XNetPeer.OnDisconnect");
	}

	public virtual void ReceiveData(byte[] data, int length)
	{

	}
}
