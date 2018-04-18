using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Steamworks;
using UnityEngine.Networking.NetworkSystem;

[NetworkSettings(channel =1, sendInterval =5)]
public class NetworkPlayer : NetworkBehaviour {

    public GameObject bulletPrefab;
    public TextMesh label;
    public float moveSpeed;

    //[SyncVar]
    //public ulong steamId;

    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangedHealth")]
	public int curHealth = maxHealth;

    public RectTransform healthBar;

    public Transform bulletSpawn;

    public static int totalShotBulletNum = 0;

    public float healthBarOriginWidth = 0;

    [SyncVar]
    public int testSyncVar = 77;

    private int lastTestSyncVar = 0;
    float tick = 0f;

	public int testRpcSend = 1;

    public override void OnStartServer()
    {
        base.OnStartServer();

        //StartCoroutine(SetNameWhenReady());

        this.healthBarOriginWidth = this.healthBar.sizeDelta.x;
    }

    private void Start()
    {
        curHealth = maxHealth;
        this.healthBarOriginWidth = this.healthBar.sizeDelta.x;
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

			if (Input.GetKeyDown(KeyCode.F))
			{
				CmdTargetRpcTest();
				//TargetTestSendInterval(MyNetworkManager.instance.connRemote);
			}
        }

		// Disable physics for peer objects
		//GetComponent<Rigidbody>().isKinematic = !hasAuthority;

		// Update player name
		// label.text = SteamFriends.GetFriendPersonaName(new CSteamID(steamId));

		//if (isServer && hasAuthority)
		//{
		//	tick += Time.deltaTime;
		//	if (tick >= 5f)
		//	{
		//		tick = 0f;
		//		testSyncVar += 1;
		//		// Debug.Log("------ testSyncVar " + testSyncVar.ToString());
		//	}
		//}

		//if (lastTestSyncVar != testSyncVar)
		//{
		//    lastTestSyncVar = testSyncVar;
		//    Debug.Log(">>>>> testSyncVar " + testSyncVar.ToString());
		//}

	}
        

    [Command]
    public void CmdFire()
    {
        Debug.Log("CmdFire 1");

        if (NetworkServer.active)
        {
            Debug.Log("CmdFire 2");

            var bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            
            // add velocity to bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 1f;

            NetworkServer.Spawn(bullet);

            // Destroy Bullet after 2 seg
            Destroy(bullet, 10);

            RpcAddNum();
        }
    }

    [ClientRpc]
    public void RpcAddNum()
    {
        totalShotBulletNum += 1;

        Debug.LogWarning("total shot bullet: " + totalShotBulletNum.ToString());
    }

    public void TakeDamage(int amount)
    {
        //  server to handle
        if (!NetworkServer.active)
            return;
        //if (!isServer)
        //    return;

        this.curHealth -= amount;
        //
        if (this.curHealth <= 0)
        {
            Destroy(gameObject);
        }
        //
        Debug.Log("take damage >" + amount.ToString());
    }

    void OnChangedHealth(int health)
    {
        curHealth = health;

        Debug.LogWarning("on changed health > " + health.ToString() + " - " + this.curHealth.ToString());
        healthBar.sizeDelta = new Vector2(healthBarOriginWidth * 1.0f * health / maxHealth, healthBar.sizeDelta.y);
    }

//    public override void OnStartLocalPlayer()
//    {
//        GetComponent<Renderer>().material.color = Color.black;
//    }

	[Command]
	void CmdTargetRpcTest()
	{
		Debug.Log("xx-- CmdTargetRpcTest");
		TargetTestRpc(MyNetworkManager.instance.connRemote);
	}

	[TargetRpc]
	public void TargetTestRpc(NetworkConnection conn)
	{
		Debug.Log("xx-- target test rpc");
	}

	[TargetRpc]
	public void TargetTestSendInterval(NetworkConnection conn)
	{
		Debug.Log("xx-- target test send interval");

	}
}
