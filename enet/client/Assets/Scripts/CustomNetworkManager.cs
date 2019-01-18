using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Networking
{
#if PHOTON_THUNDER
    public class CustomNetworkManager : PhotonNetworkManager
    {
    }
#else
	public class CustomNetworkManager : NetworkManager
	{
		private void Start()
		{
			string msg = "Thunder installation not detected, please run the installer on Window -> Thunder -> Install";
#if UNITY_EDITOR
			EditorUtility.DisplayDialog("Thunder Install", msg, "OK");
			UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE
            Debug.LogError(msg);
#endif
		}
	}
#endif
}