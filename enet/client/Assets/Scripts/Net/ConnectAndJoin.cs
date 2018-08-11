using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectAndJoin : MonoBehaviour
{
	private bool ConnectInUpdate = true;

	public void Update()
	{
		if (ConnectInUpdate && !NetManager.connected)
		{
			ConnectInUpdate = false;
			NetManager.Connect("127.0.0.1", 17210);
		}
	}
}
