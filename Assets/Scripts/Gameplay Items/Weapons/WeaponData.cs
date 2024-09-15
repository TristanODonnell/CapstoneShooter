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
    public enum WeaponCategory { Shrapnel, Energy, Heavy, Melee }
    public enum WeaponType { Projectile, Hitscan, Melee }
    public enum BulletType { Standard, Explosive }
    public WeaponCategory weaponCategory;
    public WeaponType weaponType;
    [SerializeField] public ProtectionValues ProtectionValues;
    [SerializeField] public ProtectionValues originalProtectionValues;
    public ProtectionValues GetProtectionValues() { return ProtectionValues; }

    [Header("Gameplay Variable Ammo Fields")]
    public int currentMagazineAmmo;
    public int totalAmmo;
    public int magazineSize;
    public int maxAmmo;
    public float reloadTime;

    [Header("Original Ammo Values")]
    public float originalReloadTime;
    public int originalMagazineSize;
    public int originalMaxAmmo;

    [Header("Game Object Assignment")]
    public GameObject weaponModel;
    public GameObject worldWeapon;

    [HideInInspector]
    public ObjectPool ObjectPool;

    [Header("Projectile Specific Parameters")]
    public GameObject projectileVisualPrefab;
    public GameObject objectPoolGameObject;
    
    public BulletType bulletType;
    public float bulletScale;

    [Header("Weapon Settings")]
    public float verticalRecoil;
    public float horizontalRecoil;
    public float recoilResetSpeed;
    public float bulletSpeed; 
    public float range;
    public float fireRate; 
    public float adsZoomLevel;
    public float penetration;
    public float penetrationPerHit = 1f;

    [Header("Burst Weapon Settings")]
    public int burstSize;
    public float burstDelay;

    [Header("RailGun Weapon Settings")]

    [Header("Riot Shield Weapon Settings")]
    public GameObject riotShieldCollider;

    [Header("Explosive Bullet Settings")]
    public float explosionRadius;

    [Header("Shop Settings")] 
    public int itemCost;
    public int ammoRefillCost;

    [Header("Visual Settings")]
    public Sprite v_crosshair;
    public Sprite v_Icon;


    public int AmmoRefillCost => ammoRefillCost;
    public string WeaponName => weaponName;
    public int ItemCost => itemCost;
    
    public GameObject GetWeaponModel() => weaponModel;
    
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

    public void Interact(PlayerController player, ShopLocation shopLocation)
    {
        throw new NotImplementedException();
    }
}
 