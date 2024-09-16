using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Transform shopPlatformTransform;

    public static ItemSpawner instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }
    public void SpawnWeapon(WeaponData weaponData)
    {

        Instantiate(weaponData.GetWorldWeapon(), shopPlatformTransform.position, Quaternion.identity);
    }

    public void SpawnPassive(PassiveData passiveData)
    {
        Instantiate(passiveData.GetWorldPassive(), shopPlatformTransform.position, Quaternion.identity);
    }

}
