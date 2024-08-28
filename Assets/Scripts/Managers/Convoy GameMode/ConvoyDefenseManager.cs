using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class ConvoyDefenseManager : MonoBehaviour
{
    public EnemyTypesList enemyTypesList;

    private ConvoySceneSpecificData convoySceneData;
        private Encounter[] encounters;
            private Collider[][] encounterStartColliders;
            private Collider[][]encounterEndColliders;
            private Transform[][] spawnPoints;




    private Dictionary<int, Encounter> encountersByRound;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        enemyTypesList = GetComponent<EnemyTypesList>();

       

       
        // Populate SpecificRounds dictionary
        // SpecificRounds.Add(1, new List<Enemy> { enemyTypesList.EnemyTypes[0] });
        // SpecificRounds.Add(2, new List<Enemy> { enemyTypesList.EnemyTypes[1] });
    }

    private void Start()
    {
        
    }

    public void InitializeEncounters(ConvoySceneSpecificData convoySceneData)
    {
        encounters = convoySceneData.encounters;
        encounterStartColliders = new Collider[encounters.Length][];
        encounterEndColliders = new Collider[encounters.Length][];
        spawnPoints = new Transform[encounters.Length][];

        for (int i = 0; i < encounters.Length; i++)
        {
            encounterStartColliders[i] = encounters[i].encounterStartColliders;
            encounterEndColliders[i] = encounters[i].encounterEndColliders;
            spawnPoints[i] = encounters[i].spawnPoints;
        }

        Debug.Log("Round Start Colliders: " + encounterStartColliders.Length);
        Debug.Log("Round End Colliders: " + encounterEndColliders.Length);
        Debug.Log("Encounters: " + encounters.Length);
    }
    private void OnTriggerEnter(Collider collider)
    {
        // Check if the collider is part of the roundStartColliders array
        for (int i = 0; i < encounterStartColliders.Length; i++)
        {
            if (encounterStartColliders[i].Contains(collider))
            {

                Debug.Log("Entered collider");
                Encounter encounter = encounters[i + 1];
                // Start the encounter for the current stop
                encounter.StartEncounter();
                encounter.UpdateEncounter();//IF NEEDED
                break;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        for (int i = 0; i < encounterEndColliders.Length; i++)
        {
            if (encounterEndColliders[i].Contains(collider))
            {
                Debug.Log("Exited collider");
                Encounter encounter = encounters[i + 1];
                encounter.EndEncounter();
                break;
            }
        }
    }        
}
