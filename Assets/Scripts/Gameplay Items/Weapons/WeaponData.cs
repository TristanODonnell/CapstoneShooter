using System;
using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon Data")]
[System.Serializable]
public class WeaponData : ScriptableObject, IInteractable
{
    public enum DamageType { Shrapnel, Energy, Heavy, Stun }
    public enum WeaponType { Projectile, Hitscan, Melee }

    private bool isReloading = false;
    public int magazineSize;
    public int currentMagazineAmmo;
    public int totalAmmo;
    public int maxAmmo;

    public float reloadTime;
    public WeaponType weaponType;
    public DamageType damageType;
   

    public float verticalRecoil;
    public float horizontalRecoil;
    public float recoilResetSpeed;

    public string weaponName;
    public int itemCost;
    public GameObject weaponModel;
    public GameObject worldWeapon;
    public int damage;
    public float bulletSpeed; 
    public float range;
    public float fireRate;

    public GameObject projectilePrefab;
    public int poolSize;
    public string WeaponName => weaponName;
    public int ItemCost => itemCost;
    public int PoolSize => poolSize;
    public GameObject GetWeaponModel() => weaponModel;
    public GameObject GetProjectilePrefab() => projectilePrefab;
    public GameObject GetWorldWeapon() => worldWeapon;
    public int MagazineSize => magazineSize;
    public int Damage => damage;
    public float BulletSpeed => bulletSpeed;
    public float Range => range;
    public float FireRate => fireRate;
    public float ReloadTime => reloadTime;
    //VISUALS, change out later for specific variable we need 
    [SerializeField] protected GameObject reticle;
    [SerializeField] protected GameObject weaponVisual;
    public void OnHoverEnter()
    {
        Debug.Log("Weapon pickup available");
    }

    public void OnHoverExit()
    {

    }

    public static ProtectionValues GetProtectionValues(DamageType damageType)
    {
        {
            switch (damageType)
            {
                case DamageType.Shrapnel:
                    return new ProtectionValues(1f, 0f, 0f, 0f); // Shrapnel damage values
                case DamageType.Energy:
                    return new ProtectionValues(0f, 1f, 0f, 0f); // Energy damage values
                case DamageType.Heavy:
                    return new ProtectionValues(0f, 0f, 1f, 0f); // Heavy damage values
                case DamageType.Stun:
                    return new ProtectionValues(0f, 0f, 0f, 1f); // Stun damage values
                default:
                    throw new ArgumentException("Invalid ammo type", nameof(damageType));
            }
        }
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
 