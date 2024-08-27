using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassSelectManager : MonoBehaviour
{
    public static ClassSelectManager Singleton
    {
        get; private set;
    }
    public PlayerData playerData;


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

    public void DataReset()
    {
        playerData.playerWeaponData.Clear();
        playerData.playerPassiveData = null;
        playerData.playerEquipmentData.Clear();
    }

    public void ChooseWeapon(WeaponData weaponData)
    {
        if (playerData.playerWeaponData.Count < 3 )
        {
            playerData.playerWeaponData.Add(weaponData);
        }
        else
        {
            Debug.LogError("Maximum number of weapons reached!");
        }

    }


    public void ChoosePassive(PassiveData passiveData)
    {
        playerData.playerPassiveData = passiveData;
    }

    public void ChooseEquipment(EquipmentData equipmentData)
    {
        if (playerData.playerEquipmentData.Count < 2)
        {
            playerData.playerEquipmentData.Add(equipmentData);
        }
        else
        {
            Debug.LogError("Maximum number of equipment reached!");
        }


    }

    
}
