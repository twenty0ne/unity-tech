using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetPlayer2 : NetworkBehaviour {
    public GameObject ingamePlayerPrefab;

    private void Start()
    {
        if (isLocalPlayer)
        {
            // rpc new player
            CmdCreatePlayer();
        }
    }

    [Command]
    public void CmdCreatePlayer()
    {
        // if (hasAuthority)
        {
            if (ingamePlayerPrefab != null)
            {
                var obj = Instantiate(ingamePlayerPrefab);
                NetworkServer.SpawnWithClientAuthority(obj, connectionToClient);
            }
        }
    }
}
