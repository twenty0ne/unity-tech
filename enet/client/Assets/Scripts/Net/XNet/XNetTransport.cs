using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TransportType
{
	UDP,
	TCP,
}

enum TransportUDPType
{
	Socket,
	ENet,
	Lidgren,
}

public class XNetTransport
{
	public XNetPeer peer;

	public virtual void Connect(string address, int port)
	{
		
	}

	public virtual void Send(byte[] data, int length)
	{

	}
}
