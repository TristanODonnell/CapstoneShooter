using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassSelectManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject player;
    public PlayerData selectedPlayerData;


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
        player = Instantiate(playerPrefab);
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.AssignPlayerData(playerData);
    }




    public void DataReset()
    {
        selectedPlayerData.playerWeaponData.Clear();
        selectedPlayerData.playerPassiveData = null;
        selectedPlayerData.playerEquipmentData.Clear();
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

    public void ChooseEquipment(int equipmentIndex)
    {
        EquipmentData equipmentData = DataManager.Singleton.equipment[equipmentIndex];
        if (selectedPlayerData.playerEquipmentData.Count < 2)
        {
            selectedPlayerData.playerEquipmentData.Add(equipmentData);
        }
        else
        {
            Debug.LogError("Maximum number of equipment reached!");
        }


    }

    
}
