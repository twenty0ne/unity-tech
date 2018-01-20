using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class XNetConnection : NetworkConnection 
{
	public XNetConnection(string uid)
	{
	}

	public override bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
	{
		
	}
}
