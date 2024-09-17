using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, ILootSource
{
    public string enemyName;
    public int creditCost;
    public float spawnChance = 1.0f;
    public int xpToAdd;
    public int currencyToAdd;
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

    public void AddXP()
    {
        XPSystem.Singleton.AddXP(xpToAdd);
    }

    public void AddCurrency()
    {
        CurrencyManager.singleton.AddCurrency(currencyToAdd);
    }

    public void SpawnLoot()
    {
        float R() => Random.Range(-1f, 1f);
        Vector3 RandomVector() => R() * Vector3.right + R() * Vector3.forward;

        try
        {
            for(pickUpsToSpawn = (byte)Random.Range(0, pickUpsToSpawn); pickUpsToSpawn > 0; --pickUpsToSpawn)
            {
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
