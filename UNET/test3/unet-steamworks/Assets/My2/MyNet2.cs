using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNet2 : NetworkManager 
{
    //public GameObject objPlayerPrefab;

    //public override void OnStartClient(NetworkClient client)
    //{
    //    base.OnStartClient(client);

    //    if (objPlayerPrefab != null)
    //    {
    //        NetworkServer.SetClientReady(client.connection);
    //        var player = GameObject.Instantiate(objPlayerPrefab);
    //        NetworkServer.SpawnWithClientAuthority(player, client.connection);
    //    }
    //}
}
