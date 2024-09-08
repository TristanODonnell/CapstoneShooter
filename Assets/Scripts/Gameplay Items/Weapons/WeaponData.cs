using System;
using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon Data")]
[System.Serializable]
public class WeaponData : ScriptableObject, IInteractable
{
    public string weaponName;
    public enum WeaponType { Projectile, Hitscan, Melee }
    public WeaponType weaponType;
    [SerializeField] public ProtectionValues ProtectionValues;
    public ProtectionValues GetProtectionValues() { return ProtectionValues; }

    [Header("Gameplay Variable Ammo Fields")]
    public int currentMagazineAmmo;
    public int totalAmmo;

    public int magazineSize;
    public int maxAmmo;
    public float reloadTime;

    [Header("Editor Set Ammo Fields")]
    public int originalMagazineSize;
    public int originalMaxAmmo;
    public float originalReloadTime;


    [Header("Game Object Assignment")]
    public GameObject weaponModel;
    public GameObject worldWeapon;
    
    [SerializeField] protected GameObject reticle;
    [SerializeField] protected GameObject weaponVisual;

    [Header("Projectile Specific Parameters")]
    public GameObject projectilePrefab;
    public int poolSize;

    [Header("Weapon Settings")]
    public float verticalRecoil;
    public float horizontalRecoil;
    public float recoilResetSpeed;
    public float bulletSpeed; 
    public float range;
    public float fireRate; 
    public float adsZoomLevel; 

    [Header("Shop Settings")]
    public int itemCost;
    public int ammoRefillCost;
    public int AmmoRefillCost => ammoRefillCost;
    public string WeaponName => weaponName;
    public int ItemCost => itemCost;
    public int PoolSize => poolSize;
    public GameObject GetWeaponModel() => weaponModel;
    public GameObject GetProjectilePrefab() => projectilePrefab;
    public GameObject GetWorldWeapon() => worldWeapon;
    public int MagazineSize => magazineSize;
    public float BulletSpeed => bulletSpeed;
    public float Range => range;
    public float FireRate => fireRate;
    public float ReloadTime => reloadTime;
    public void OnHoverEnter()
    {
        Debug.Log("Weapon pickup available");
    }
    public void OnHoverExit()
    {

    }
    public void Interact(PlayerController player, WeaponHolder weaponHolder)
    {

        if (weaponHolder != null && weaponHolder.myweaponData != null)
        {
            // Check if the player already has the weapon
            foreach (WeaponData weapon in player.shoot.weapons)
            {
                if (weaponHolder.myweaponData == weapon)
                {
                    Debug.Log("You already have this weapon.");
                    return; // Exit the method since the weapon is the same
                }
            }
             
            // If the player has a current weapon, drop it
            if (player.shoot.currentWeapon != null)
            {
                player.shoot.DropWeapon(player.shoot.currentWeaponIndex);
            }

            // Now pick up the new weapon
            player.shoot.PickUpWeapon(weaponHolder);
        }
        else
        {
            Debug.LogWarning("WeaponHolder or myweaponData is null. Cannot interact.");
        }
    }
    public void Interact(PlayerController player, EquipmentHolder equipmentHolder)
    {
        throw new System.NotImplementedException();
    }
    public void Interact(PlayerController player, EquipmentData equipmentData)
    {
        throw new System.NotImplementedException();
    }
}
 