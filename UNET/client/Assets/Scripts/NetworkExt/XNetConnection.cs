using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class XNetConnection : NetworkConnection 
{
	private static int nextConnectionId = -1;

	public XNetConnection(string uid)
	{
	}

	public override bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
	{
		
	}

	/// Because we fake the UNET connection, connection initialization is not handled by UNET internally. 
	/// Connections must be manually initialized with this function.
	/// 
	public void ForceInitialize(HostTopology hostTopology)
	{
		int id = ++nextConnectionId;
		this.Initialize ("localhost", id, id, hostTopology);
	}
}
