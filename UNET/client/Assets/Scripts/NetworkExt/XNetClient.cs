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
}
