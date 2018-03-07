using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Player2 : NetworkBehaviour {

    public GameObject bulletPrefab;
//    public TextMesh label;
    public float moveSpeed;

    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangedHealth")]
    public int curHealth = 100;
    //[SyncVar]
    //

    public RectTransform healthBar;

    public Transform bulletSpawn;

    public static int totalShotBulletNum = 0;

    public float healthBarOriginWidth = 0;

    private string logTag = "xx--";

    [SyncVar]
    public int testSyncVar = 77;

    private int lastTestSyncVar = 0;
    float tick = 0f;

    public override void OnStartServer()
    {
        base.OnStartServer();

        logTag = "xx--" + gameObject.GetInstanceID().ToString() + "-- ";
        Debug.Log(logTag + "OnStartServer");

        //StartCoroutine(SetNameWhenReady());

        //this.healthBarOriginWidth = this.healthBar.sizeDelta.x;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        logTag = "xx--" + gameObject.GetInstanceID().ToString() + "-- ";
        Debug.Log(logTag + "OnStartClient");
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log(logTag + "OnStartLocalPlayer");
    }

    private void Start()
    {
        //curHealth = maxHealth;
        //this.healthBarOriginWidth = this.healthBar.sizeDelta.x;
        Debug.Log(logTag + "isServer - " + isServer.ToString());
        Debug.Log(logTag + "isClient - " + isClient.ToString());
        Debug.Log(logTag + "isLocalPlayer - " + isLocalPlayer.ToString());
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
        //if (!isLocalPlayer)
        //    return;

        if (hasAuthority)
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

        if (isServer)
        {
            tick += Time.deltaTime;
            if (tick >= 5f)
            {
                tick = 0f;
                testSyncVar += 1;
                Debug.Log("------ testSyncVar " + testSyncVar.ToString());
            }
        }

        if (hasAuthority)
        {
            if (lastTestSyncVar != testSyncVar)
            {
                lastTestSyncVar = testSyncVar;
                Debug.Log(">>>>> testSyncVar " + testSyncVar.ToString());
            }
        }

    }
        

    [Command]
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

            NetworkServer.Spawn(bullet);

            // Destroy Bullet after 2 seg
            Destroy(bullet, 10);

            RpcAddNum();
        }
    }

    [Command]
    public void CmdRotate()
    {
        // if (isServer)
        {
            transform.Rotate(0, 90, 0);
        }
    }

    [ClientRpc]
    public void RpcAddNum()
    {
        if (isLocalPlayer)
        {
            totalShotBulletNum += 1;

            Debug.LogWarning("total shot bullet: " + totalShotBulletNum.ToString());
        }
    }

    public void TakeDamage(int amount)
    {
        //  server to handle
        //if (!NetworkServer.active)
        //    return;
        if (!isServer)
            return;

        curHealth -= amount;
        
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
        //
        Debug.Log(logTag + "take damage >" + amount.ToString());
    }

    void OnChangedHealth(int health)
    {
        curHealth = health;

        // Debug.Log(logTag + "on changed health > " + health.ToString() + " - " + this.curHealth.ToString());
        healthBar.sizeDelta = new Vector2(healthBarOriginWidth * 1.0f * health / maxHealth, healthBar.sizeDelta.y);
    }

//    public override void OnStartLocalPlayer()
//    {
//        GetComponent<Renderer>().material.color = Color.black;
//    }
}
