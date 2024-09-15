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


    [SerializeField] private PickUp_ScriptableObject pickupTable;
    [SerializeField] private byte pickUpsToSpawn = 2;
    public Enemy(string name, int creditCost, GameObject prefab)
    {
        enemyName = name;
        this.creditCost = creditCost;
        enemyPrefab = prefab;
    }



    public void SpawnLoot()
    {
        float R() => Random.Range(-1f, 1f);
        Vector3 RandomVector() => R() * Vector3.right + R() * Vector3.forward;

        try
        {
            while(pickUpsToSpawn > 0)
            {
                pickUpsToSpawn--;
                if (Random.value > 0.5f)
                    pickUpsToSpawn--;
                pickupTable.Pickup_Spawn(transform.position + Vector3.up * 3f + RandomVector() * 2f);
            }
        }
        catch { }
    }

    

    public Transform GetLootSpawnPoint()
    {
        return transform;
    }
}
