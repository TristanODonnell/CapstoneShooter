using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public void SpawnWeapon(WeaponData weaponData)
    {
        Instantiate(weaponData.GetWorldWeapon(), new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void SpawnAbility()
    {
        //Instantiate(, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void SpawnPassive(PassiveData passiveData)
    {
        Instantiate(passiveData.GetWorldPassive(), new Vector3(0, 0, 0), Quaternion.identity);
    }

}
