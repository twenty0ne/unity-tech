using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    //public float speed;

	//void Update () {
        //transform.Translate(transform.forward * Time.deltaTime * speed);
	//}

    private void OnCollisionEnter(Collision other)
    {
        GameObject hit = other.gameObject;
        var player = hit.GetComponent<NetworkPlayer>();

        if (player != null)
        {
            player.TakeDamage(1);
        }

        Destroy(gameObject);
    }
}
