using System.Collections;
using System.Collections.Generic;
using System.Linq;
using gricel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Enemy;

public class RoundManager : MonoBehaviour
{
    [Header("Round Manager Systems")]
    public SpawnManager spawnManager;
    public EnemyTypesList EnemyTypesList;
    public List<GameObject> highTierEnemies;
    public List<GameObject> lowTierEnemies;
    public int currentRound = 0;
    public int currentRoundCredits;

    [Header("Parameter Values")]
    public int roundCreditMultiplier;
    public float highTierPercentage;
    public int spawnChanceMultiplier;
    public int roundFinishTimerValue;

    
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool isRoundEnded = false;
    
    public Dictionary<int, List<GameObject>> SpecificRounds { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SpecificRounds = new Dictionary<int, List<GameObject>>();
        highTierEnemies = EnemyTypesList.HighTierEnemies;
        lowTierEnemies = EnemyTypesList.LowTierEnemies;

        // Populate SpecificRounds dictionary
        // SpecificRounds.Add(1, new List<Enemy> { enemyTypesList.EnemyTypes[0] });
        // SpecificRounds.Add(2, new List<Enemy> { enemyTypesList.EnemyTypes[1] });
    }
    public IEnumerator RunRoundLoop()
    {
        while (true)
        {
            StartNextRound();
            yield return new WaitUntil(() => areAllEnemiesDestroyed());

            EndRound();
            yield return new WaitUntil(() => isRoundEnded);
            if (currentRound == 101)
            {
                // Game over, stop the coroutine
                yield break;
            }
        }
    }

    public void StartNextRound()
    {
        
        isRoundEnded = false;
        currentRound++;
        currentRoundCredits = CalculateRoundCredits(currentRound);

        if (SpecificRounds.ContainsKey(currentRound))
        {
            //SpawnSpecificEnemies(SpecificRounds[currentRound]);
        }
        else
        {
            isSpawningEnemies = true;
            StartCoroutine(SpawnEnemiesCoroutine());
        }
    }
    
    private void EndRound()
    {
        isSpawningEnemies = false; 
        StopCoroutine(SpawnEnemiesCoroutine());
        spawnedEnemies.Clear();
        StartCoroutine(WaitForRoundToEnd());
        Debug.Log("Cleared");
    }
    private IEnumerator WaitForRoundToEnd()
    {
        isRoundEnded = false;
        yield return RoundEndTimer(roundFinishTimerValue);
        isRoundEnded = true;
    }
    private IEnumerator RoundEndTimer(float time)
    {
        yield return new WaitForSeconds(time);
    }
    private int CalculateRoundCredits(int roundNumber)
    {
        return roundNumber * roundCreditMultiplier;
    }

    private bool isSpawningEnemies = false;
    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            if(spawnedEnemies.Count < 50 && currentRoundCredits >= GetMinCreditCost())
            {
                isSpawningEnemies = true;
                SpawnEnemiesUsingCredits();
            }
            else
            {
                isSpawningEnemies = false;
            }
            yield return new WaitForSeconds(0.1f);

        }

    }

    private int GetMinCreditCost()
    {
        int minCreditCost = int.MaxValue;
        foreach (GameObject enemyPrefab in highTierEnemies)
        {
            int creditCost = enemyPrefab.GetComponent<Enemy>().creditCost;
            if (creditCost < minCreditCost)
            {
                minCreditCost = creditCost;
            }
        }
        foreach (GameObject enemyPrefab in lowTierEnemies)
        {
            int creditCost = enemyPrefab.GetComponent<Enemy>().creditCost;
            if (creditCost < minCreditCost)
            {
                minCreditCost = creditCost;
            }
        }
        return minCreditCost;
    }
    private void SpawnEnemiesUsingCredits()
    {

        while (spawnedEnemies.Count < 50 && currentRoundCredits > 0)
        {
            int highTierCredits = (int)(currentRoundCredits * highTierPercentage);
            int lowTierCredits = currentRoundCredits - highTierCredits;

            SpawnHighTierEnemies(highTierCredits);
            SpawnLowTierEnemies(lowTierCredits);
        }
    } 
    private void SpawnHighTierEnemies(int tierCredit)
    {

        while (tierCredit > 0 && currentRoundCredits > 0 && spawnedEnemies.Count < 50)
        {
            int randomIndex = Random.Range(0, highTierEnemies.Count);
            GameObject highTierEnemyPrefab = highTierEnemies[randomIndex];
            if (highTierEnemyPrefab != null)
            {
                int creditCost = highTierEnemyPrefab.GetComponent<Enemy>().creditCost;
                if (creditCost <= tierCredit)
                {
                    //spawning a high tier enemy 
                    GameObject highTierEnemy = spawnManager.SpawnEnemy(highTierEnemyPrefab, EnemyTier.High);
                    HealthSystem enemyHealth = highTierEnemy.GetComponent<HealthSystem>();
                    enemyHealth.onDeath.AddListener(() => RemoveEnemyFromList(highTierEnemy));
                    spawnedEnemies.Add(highTierEnemy);
                    tierCredit -= creditCost;
                    currentRoundCredits -= creditCost;
                    IncreaseSpawnChance(highTierEnemy);
                    

                }
                else
                {
                    break;
                }
            }   
        }    
    }              
    private void SpawnLowTierEnemies(int tierCredit)
    {
        
        while (tierCredit > 0 && currentRoundCredits > 0 && spawnedEnemies.Count < 50)
        {

            int randomIndex = Random.Range(0, lowTierEnemies.Count);

            GameObject lowTierEnemyPrefab = lowTierEnemies[randomIndex];
            int creditCost = lowTierEnemyPrefab.GetComponent<Enemy>().creditCost;
            if (lowTierEnemyPrefab != null && creditCost <= currentRoundCredits)
            {
                GameObject lowTierEnemy = spawnManager.SpawnEnemy(lowTierEnemyPrefab, EnemyTier.Low);
                HealthSystem enemyHealth = lowTierEnemy.GetComponent<HealthSystem>();
                enemyHealth.onDeath.AddListener(() => RemoveEnemyFromList(lowTierEnemy));
                spawnedEnemies.Add(lowTierEnemy);
                currentRoundCredits -= creditCost;
                IncreaseSpawnChance(lowTierEnemy);
                
            }
            else
            {
                break;
            }
        }
    }
    private void IncreaseSpawnChance(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.spawnChance *= spawnChanceMultiplier;
    }

    
    public void RemoveEnemyFromList(GameObject enemy)
    {
        Debug.Log("Method called");
        spawnedEnemies.Remove(enemy);
        Destroy(enemy);
        
    }

    private bool areAllEnemiesDestroyed()
    {
        return spawnedEnemies.Count == 0;
    }
    private void SpawnSpecificEnemies(List<Enemy> enemyTypes)
    {
        //foreach (Enemy enemy in enemyTypes)
        // {
        //     SpawnEnemy(enemy);
        // }
    }






}
