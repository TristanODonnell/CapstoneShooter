using System;
using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Singleton
    {
        get; private set;
    }
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Transform[] spawnPointTransforms;
    public Transform playerSpawn;

    public Transform GetPlayerSpawn()
    {
        GameObject playerSpawnObject = GameObject.FindWithTag("Player Spawn");
        if (playerSpawnObject != null)
        {
            playerSpawn = playerSpawnObject.transform;
        }
        
        return playerSpawn;
    }
    public void GetSpawnPoints()
    {
        SpawnPoint[] spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();
        Debug.Log("Spawn points found: " + spawnPoints.Length);
        

        if (spawnPoints.Length == 0)
        {
            return;
        }
        spawnPointTransforms = new Transform[spawnPoints.Length];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPointTransforms[i] = spawnPoints[i].transform;
            Debug.Log("Spawn point " + i + ": " + spawnPoints[i].name + " at " + spawnPoints[i].transform.position);
        }
    }

    public GameObject SpawnEnemy(GameObject enemyType, Enemy.EnemyTier tier)
    {
        if (spawnPointTransforms != null)
        {
            int randomIndex = UnityEngine.Random.Range(0, spawnPointTransforms.Length);
            Transform spawnPoint = spawnPointTransforms[randomIndex];
            GameObject spawnedEnemy = Instantiate(enemyType, spawnPoint.position, Quaternion.identity);
            return spawnedEnemy;
		}
		Debug.LogError("spawnPointTransforms is null!");
		return null;

	}

    
}
