using System.Collections;
using System.Collections.Generic;
using Abilities;
using throwables;
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
    public List<AbilityBase> abilities = new List<AbilityBase>();
    public List<PassiveData> passive = new List<PassiveData>();
    public List<ThrowableItem> grenades = new List<ThrowableItem>();
    
}
