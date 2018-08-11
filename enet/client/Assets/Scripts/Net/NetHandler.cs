using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 与 Unity 交互的地方，所有 Unity 内部定义的函数
public class NetHandler : MonoBehaviour
{
	private void Start ()
	{
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadMode) => 
		{
			NetManager.nclient.NewSceneLoaded();
		};
	}
	
	private void Update ()
	{
		// NetManager.peer.
	}

	private void OnApplicationQuit()
	{
		
	}

	private void OnApplicationPause(bool pause)
	{
		
	}
}
