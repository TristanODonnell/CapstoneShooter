using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Singleton
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


    public List<WeaponData> weapons = new List<WeaponData>();
    public List<EquipmentData> equipment = new List<EquipmentData>();
    public List<PassiveData> passive = new List<PassiveData>();
}
