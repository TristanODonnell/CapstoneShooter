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

    public UnityEvent onEnemyDeath = new UnityEvent();

    private void Update()
    {
        Die();
    }
    public Enemy(string name, int creditCost, GameObject prefab)
    {
        enemyName = name;
        this.creditCost = creditCost;
        enemyPrefab = prefab;
    }










    
    

    public void SpawnLoot()
    {
        
    }

    public void Die() 
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            
            Destroy(gameObject);
            healthSystem.onDeath.Invoke();
            
        }
        
        
    }

    

    public Transform GetLootSpawnPoint()
    {
        throw new System.NotImplementedException();
    }
}
