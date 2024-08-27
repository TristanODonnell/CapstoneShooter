using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public RoundManager roundManager;
    public ConvoyDefenseManager convoyDefenseManager;
    public string[] scenesList;

    public GameObject playerPrefab;
    public GameObject player;

    
    public static GameManager Singleton
    {
        get; private set;
    }



    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (scenesList.Contains(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            
        }
        else
        {
            Debug.LogError("Scene not found: " + sceneName);
        }
    }



    public void AssignPlayerToController(PlayerData playerData)
    {
        player = Instantiate(playerPrefab);
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.AssignPlayerData(playerData);
    }


    public void LoadArenaMode() 
    {
        string sceneName = "Asteroid TEST Arena"; 
        LoadScene(sceneName);
        
        SceneManager.sceneLoaded += (scene, mode) => InitializeArenaMode();
    }

    public void LoadConvoyDefenseMode()
    {
        string sceneName = "Test Convoy";
        LoadScene(sceneName);

        SceneManager.sceneLoaded += (scene, mode) => InitializeConvoyDefenseMode();
    }

    public void InitializeArenaMode()
    {
        //WIN CONDITION
        //reach wave 100

        StartCoroutine(roundManager.RunRoundLoop());


    }

    public void InitializeConvoyDefenseMode()
    {
        
        ConvoySceneSpecificData convoySceneData = GameObject.FindObjectOfType<ConvoySceneSpecificData>();
        convoyDefenseManager.InitializeEncounters(convoySceneData);

        //truck roadmapUI
    }

    public void InitializeNuclearOverrideMode()
    {
        //Win Condition
        //Last 30 minutes holding the drone countdown 

        //30 minute timer UI
        //nuclear override UI 
    }

}
