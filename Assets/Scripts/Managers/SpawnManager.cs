using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject SpawnEnemy(GameObject enemyType, Enemy.EnemyTier tier)
    {
        Debug.Log("Spawning enemy: " + enemyType.GetComponent<Enemy>().enemyName);
        GameObject spawnedEnemy = Instantiate(enemyType, new Vector3(0, 0, 0), Quaternion.identity);
        
        Debug.Log("Enemy spawned: " + spawnedEnemy.name);

        return spawnedEnemy;
    }
}
