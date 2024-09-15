using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, ILootSource
{
    public HealthSystem healthSystem;
    public string enemyName;
    public int creditCost;
    public float spawnChance = 1.0f;
    public GameObject enemyPrefab;
    public EnemyTier enemyTier;
    public enum EnemyTier { High, Low }


    public GameObject[] lootItems;
    public Enemy(string name, int creditCost, GameObject prefab)
    {
        enemyName = name;
        this.creditCost = creditCost;
        enemyPrefab = prefab;
    }



    public void SpawnLoot()
    {
        float RV() => Random.Range(-1f, 1f);
        Vector3 R() => Vector3.right * RV() + Vector3.up * RV() + Vector3.forward * RV();

        foreach (var item in lootItems)
        {
            var c = Instantiate(item);
            c.transform.position = transform.position + Vector3.up*3f + R();
        }
    }

    

    public Transform GetLootSpawnPoint()
    {
        return transform;
    }
}
