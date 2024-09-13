using System.Collections;
using System.Collections.Generic;
using Abilities;
using throwables;
using UnityEngine;

public class ClassSelectManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject player;
    public PlayerData selectedPlayerData;
    public GrenadeManager grenadeManager;

    public static ClassSelectManager Singleton
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

    private void Start()
    {
        DataReset();

    }

    public void AssignPlayerToController(PlayerData playerData)
    {
        player = Instantiate(playerPrefab, SpawnManager.Singleton.playerSpawn.position, Quaternion.identity);
        PlayerController playerController = player.GetComponent<PlayerController>();
        PassiveBehavior passiveBehavior = playerController.GetComponent<PassiveBehavior>();
        passiveBehavior.Initialize();
        playerController.AssignPlayerData(playerData);
    }




    public void DataReset()
    {
        selectedPlayerData.playerWeaponData.Clear();
        selectedPlayerData.playerPassiveData = null;
        selectedPlayerData.playerAbilityReference = null;
        
    }

    public void ChooseWeapon(int weaponIndex)
    {
        WeaponData weaponData = DataManager.Singleton.weapons[weaponIndex];

        if (selectedPlayerData.playerWeaponData.Count < 3 )
        {
            selectedPlayerData.playerWeaponData.Add(weaponData);
        }
        else
        {
            Debug.LogError("Maximum number of weapons reached!");
        }

    }


    public void ChoosePassive(int passiveIndex)
    {
        PassiveData passiveData = DataManager.Singleton.passive[passiveIndex];
        selectedPlayerData.playerPassiveData = passiveData;
    }


    public void ChooseAbility(int abilityIndex)
    {
        AbilityBase chosenAbility = DataManager.Singleton.abilities[abilityIndex];

        selectedPlayerData.playerAbilityReference = chosenAbility;
    }
     
    
    public void ChooseGrenade(int grenadeIndex)  
    {
        ThrowableItem chosenGrenade = DataManager.Singleton.grenades[grenadeIndex];
        grenadeManager.currentIndex = grenadeIndex;
        grenadeManager.currentGrenade = chosenGrenade;
        grenadeManager.grenadeCounts[grenadeIndex] = 2;

        for (int i = 0; i < grenadeManager.grenadeCounts.Length; i++)
        {
            grenadeManager.grenadeCounts[i] = 0;
        }
        grenadeManager.grenadeCounts[grenadeIndex] = 2;

      //  Debug.Log("Current Grenade: " + grenadeManager.currentGrenade);
        Debug.Log("Grenade Count: " + grenadeManager.grenadeCounts[grenadeIndex]);
    }

    
} 
