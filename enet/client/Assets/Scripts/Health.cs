using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour {

    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangedHealth")] public int currentHealth = maxHealth;
    public RectTransform healthBar;
    public bool destroyOnDeath;

    private NetworkStartPosition[] spawnPoints;

    void Start()
    {
        if(isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }

        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;
                RpcRespawn();
            }
        }
    }

    void OnChangedHealth(int health)
    {
        healthBar.sizeDelta = new Vector2(health * 2, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if(isLocalPlayer)
        {
            Vector3 spawnPosition = Vector3.zero;

            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            transform.position = spawnPosition;
        }
    }
}
