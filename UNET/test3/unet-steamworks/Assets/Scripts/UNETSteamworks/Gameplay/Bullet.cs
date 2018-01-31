using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{

    //public float speed;

    //void Update () {
    //transform.Translate(transform.forward * Time.deltaTime * speed);
    //}

    // private bool isDestroy = false;

    private void Awake()
    {
        Debug.LogWarning("create bullet");
    }

    private void OnCollisionEnter(Collision other)
    {
        //if (isDestroy)
        //    return;

        GameObject hit = other.gameObject;
        var player = hit.GetComponent<NetworkPlayer>();

        if (player != null)
        {
            player.TakeDamage(1);
        }

        //isDestroy = true;
        Destroy(gameObject);
    }
}
