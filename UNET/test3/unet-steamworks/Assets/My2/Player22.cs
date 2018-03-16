using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Player22 : NetworkBehaviour
{
    [SyncVar]
    public int kk = 32;

    private int lastTestSyncVar = 0;
    float tick = 0f;

    void Update()
    {
        if (isServer && hasAuthority)
        {
            tick += Time.deltaTime;
            if (tick >= 5f)
            {
                tick = 0f;
                kk -= 1;
                // Debug.Log("------ testSyncVar " + testSyncVar.ToString());
            }
        }

        if (lastTestSyncVar != kk)
        {
            lastTestSyncVar = kk;
            Debug.Log(">>>>> kk " + kk.ToString());

            int rr = NetworkServer.numChannels;
            bool t1 = NetworkServer.active;
            bool t2 = NetworkServer.localClientActive;
        }

        if (!isServer)
        {
            Debug.Log("------------------------------ not server");
        }
    }
}
