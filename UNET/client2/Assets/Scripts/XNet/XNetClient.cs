using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class XNetClient :  NetworkClient
{
	public XNetClient(NetworkConnection conn) : base(conn)
	{
	}

	public override void Disconnect ()
	{
		base.Disconnect ();

		// TODO
	}

    public void Connect()
    {
        // Connect to localhost and trick UNET by setting ConnectState state to "Connected"
        // which triggers some initialization and allows data to pass through TransportSend
        Connect("localhost", 0);
        m_AsyncConnect = ConnectState.Connected;

        // manually init connection
        connection.ForceInitialize(hostTopology);

        // send Connected message
        connection.InvokeHandlerNoData(MsgType.Connect);
    }
}
