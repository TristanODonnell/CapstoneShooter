using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public WeaponData myweaponData;
    public GameObject currentWorldWeapon;
    public int groundTotalAmmo;

    private void Start()
    {
        // Set a random amount of ammo for demonstration
        groundTotalAmmo = Random.Range(0, myweaponData.maxAmmo/2);
    }
} 
