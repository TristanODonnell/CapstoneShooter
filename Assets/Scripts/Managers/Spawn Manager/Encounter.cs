using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    public List<GameObject> encounterEnemies;
    

    public Transform[] spawnPoints;

    public Collider[] encounterStartColliders;
    public Collider[] encounterEndColliders;

    public void StartEncounter()
    {
        Debug.Log("Encounter Started");
        //initialize spawns for encounter 
        //set up spawns for enemies 
        
    }

    public void UpdateEncounter()
    {
        //AI Updating? 
    }

    public void EndEncounter()
    {
        //Destroy remaining enemies in List 
    }
}
