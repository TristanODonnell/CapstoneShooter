using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class LootSystem : MonoBehaviour
{
    public Enemy enemy; 
    private ILootSource lootSource;

    // Start is called before the first frame update
    void Start()
    {
       // enemy.onEnemyDeath.AddListener(SpawnLoot);
        lootSource = enemy.gameObject.GetComponent<ILootSource>();
    }

    private void SpawnLoot()
    {
        lootSource.SpawnLoot();
    }
}
