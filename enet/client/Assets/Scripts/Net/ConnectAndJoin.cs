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
			NetManager.ConnectToMaster("127.0.0.1", 17210);
		}
	}

	public void OnConnectedToMaster()
	{
		Debug.Log("xx-- ConnectAndJoin.OnConnectedToMaster");
	}
}
