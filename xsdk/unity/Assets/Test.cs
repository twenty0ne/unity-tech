using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSDK;

public class Test : MonoBehaviour
{
	public void OnClickAuth()
	{
		Auth.Init((ResultAPI result, AuthInitResult authInitResult) => {
			Debug.Log("click auth callback");
		});	
	}
}
