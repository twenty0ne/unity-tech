using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class XNetConnection : NetworkConnection 
{
    private string clientId;
    public string ClientId { get { return clientId;  } }

	public XNetConnection(string clientId)
	{
        this.clientId = clientId;
	}

	public override bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
	{
        // TODO
        //if (clientId == local client id)
        //{
            
        //}

        // Send packet to peer
        bool ret = XNetManager.instance.Send(bytes, numBytes, channelId);
        if (ret)
        {
            error = 0;
            return true;
        }
        else
        {
            error = 1;
            return false;
        }
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