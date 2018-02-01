using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Player3 : MonoBehaviour {

    public GameObject bulletPrefab;
//    public TextMesh label;
    public float moveSpeed;

    public const int maxHealth = 100;
    public int curHealth = 100;
    //[SyncVar]
    //

    public RectTransform healthBar;

    public Transform bulletSpawn;

    public static int totalShotBulletNum = 0;

    public float healthBarOriginWidth = 0;

    private string logTag = "xx--";

    public bool isEnable = true;

    private void Start()
    {
        //curHealth = maxHealth;
        //this.healthBarOriginWidth = this.healthBar.sizeDelta.x;
    }

    //IEnumerator SetNameWhenReady()
    //{
    //    // Wait for client to get authority, then retrieve the player's Steam ID
    //    var id = GetComponent<NetworkIdentity>();
    //    while (id.clientAuthorityOwner == null)
    //    {
    //        yield return null;
    //    }

    //    steamId = SteamNetworkManager.Instance.GetSteamIDForConnection(id.clientAuthorityOwner).m_SteamID;

    //}

    void Update()
    {
        if (!this.isEnable)
            return;

        // if (hasAuthority)
        {
            // Only allow input for client with authority 
            var input = new Vector3( Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical") );
            transform.Translate(input * Time.deltaTime*moveSpeed, Space.World);

            //if (Input.GetButtonDown("Fire1"))
            if (Input.GetKeyDown(KeyCode.Space))
            {
               CmdFire();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                CmdRotate();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log(logTag + "health > " + curHealth.ToString());
            }
        }
      
        // Disable physics for peer objects
        //GetComponent<Rigidbody>().isKinematic = !hasAuthority;

        // Update player name
        // label.text = SteamFriends.GetFriendPersonaName(new CSteamID(steamId));

    }
        

    public void CmdFire()
    {
        // Debug.Log("CmdFire 1");

        // if (NetworkServer.active)
        // if (isServer)
        {
            // Debug.Log("CmdFire 2");

            var bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            
            // add velocity to bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 1f;

            // Destroy Bullet after 2 seg
            Destroy(bullet, 10);
        }
    }

    public void CmdRotate()
    {
        // if (isServer)
        {
            transform.Rotate(0, 90, 0);
        }
    }

    public void TakeDamage(int amount)
    {
        //  server to handle
        //if (!NetworkServer.active)
        //    return;

        curHealth -= amount;
        
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
        //
        Debug.Log(logTag + "take damage >" + amount.ToString());
    }

//    public override void OnStartLocalPlayer()
//    {
//        GetComponent<Renderer>().material.color = Color.black;
//    }
}
