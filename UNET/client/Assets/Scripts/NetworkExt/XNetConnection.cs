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

public static class NetworkConnectionExt
{
	private static int nextConnectionId = -1;

	/// Because we fake the UNET connection, connection initialization is not handled by UNET internally. 
	/// Connections must be manually initialized with this function.
	/// 
	public static void ForceInitialize(this NetworkConnection conn, HostTopology hostTopology)
	{
		int id = ++nextConnectionId;
		conn.Initialize ("localhost", id, id, hostTopology);
	}
}