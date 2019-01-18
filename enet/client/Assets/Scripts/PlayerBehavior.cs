using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerBehavior : NetworkBehaviour {

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    [Command]
    void CmdFire()
    {
        // Create bullet from Prefab
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Add velocity to Bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6.0f;

        // Spawn the bullet
        NetworkServer.Spawn(bullet);

        // Destroy Bullet after 2 seg
        Destroy(bullet, 2);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }
}
