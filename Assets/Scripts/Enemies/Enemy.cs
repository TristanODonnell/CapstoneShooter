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
    public GameObject enemyPrefab;
    public EnemyTier enemyTier;
    public enum EnemyTier { High, Low }

    public Enemy(string name, int creditCost, GameObject prefab)
    {
        enemyName = name;
        this.creditCost = creditCost;
        enemyPrefab = prefab;
    }

    public void SpawnLoot()
    {
        
    }

    
    public Transform GetLootSpawnPoint()
    {
        throw new System.NotImplementedException();
    }
}
