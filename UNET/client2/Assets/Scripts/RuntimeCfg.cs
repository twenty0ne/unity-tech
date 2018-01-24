using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class RuntimeCfg : MonoBehaviour
{

#if PHOTON_THUNDER

    private PhotonThunderConfig config;

    void Awake()
    {
        config = PhotonUtils.getThunderConfig();
    }

    void OnGUI()
    {
        int ypos = 40;
        string msg = null;

        msg = string.Format("Default Connection Type: {0}", config.connectionType);
        GUI.Label(new Rect(Screen.width - 600, ypos, 280, 20), msg);

        if (!NetworkServer.active)
        {
            msg = string.Format("Current Connection Type: {0}", PhotonNetworkManager.currentConnectionType);
            GUI.Label(new Rect(Screen.width - 300, ypos, 280, 20), msg);
            ypos += 20;
        }

        if (config.useLocalServer)
        {
            GUI.Label(new Rect(Screen.width - 300, ypos, 280, 20), "Self Hosted Server Configuration");
            ypos += 20;

            msg = string.Format("Address: {0}", config.localServerIp);
            GUI.Label(new Rect(Screen.width - 300, ypos, 280, 20), msg);
            ypos += 20;

            msg = string.Format("Port: {0}", config.localServerPort);
            GUI.Label(new Rect(Screen.width - 300, ypos, 280, 20), msg);
            ypos += 20;
        }
        else 
        {
            msg = string.Format("Current Region: {0}", config.currentRegion);
            GUI.Label(new Rect(Screen.width - 300, ypos, 280, 20), msg );
        }
    }

#endif

}
