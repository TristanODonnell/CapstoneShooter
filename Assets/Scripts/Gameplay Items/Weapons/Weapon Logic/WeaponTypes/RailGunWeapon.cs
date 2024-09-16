using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGunWeapon : WeaponLogic
{
    private bool isCharging = false;
    public float railgunChargeLevel;
    public RailGunWeapon(ShootBehavior shootBehavior, WeaponData weaponData, ObjectPool objectPool, GameObject shooter) : base(shootBehavior, weaponData, objectPool, shooter)
    {
        currentWeaponData = weaponData;
        this.objectPool = objectPool;
        this.shooter = shooter;
    }
    
    
    public override void StartShooting(Transform transform, bool useAmmo = true)
    {
        if (Time.time - lastFireTime < currentWeaponData.fireRate)
        {
            return; // Exit early if we're still within the fire rate cooldown
        }
        Debug.Log("Current weapon type: " + currentWeaponData.weaponType);
        if (currentWeaponData.currentMagazineAmmo > 0 && !isReloading)
        {
            lastFireTime = Time.time; // Update last fire time to enforce fire rate

            if (currentWeaponData.weaponType == WeaponData.WeaponType.Projectile)
            {
                if (objectPool == null)
                {
                    Debug.LogError("Object pool is not initialized for weapon: ");
                    return; // Exit if the object pool is not initialized
                }
                if (currentWeaponData.chargeTime > 0)
                {
                    // Initialize the rail gun's charge level
                    railgunChargeLevel = 0f;
                    isCharging = true;
                }
                else
                {
                    // Fire a low-damage shot immediately
                    ProjectileWeaponFire(transform.position, transform.rotation, currentWeaponData.range, currentWeaponData.bulletSpeed, shooter);
                    ApplyRecoil();
                    currentWeaponData.currentMagazineAmmo -= 1;
                }
            }
        }
        else if (currentWeaponData.currentMagazineAmmo <= 0)
        {
            StopShooting(transform);
        }

    }
    public override void Shooting(Transform transform, bool useAmmo = true)
    {
        if (currentWeaponData.weaponType == WeaponData.WeaponType.Projectile && currentWeaponData.chargeTime > 0 && isCharging)
        {
            // Fire the rail gun with the current charge level when the trigger is released
            float damage = Mathf.Lerp(currentWeaponData.minDamage, currentWeaponData.maxDamage, railgunChargeLevel / currentWeaponData.chargeTime);
            ProjectileWeaponFire(transform.position, transform.rotation, currentWeaponData.range, currentWeaponData.bulletSpeed, shooter);
            ApplyRecoil();
            currentWeaponData.currentMagazineAmmo -= 1;
            railgunChargeLevel = 0f;
            isCharging = false;
        }
        if (currentWeaponData.currentMagazineAmmo <= 0)
        {
            ReloadLogic();
        }
    }
    public override void StopShooting(Transform transform, bool useAmmo = true)
    {
        if (currentWeaponData.currentMagazineAmmo <= 0 && useAmmo)
        {
            ReloadLogic();
        }
    }
    public override void ProjectileWeaponFire(Vector3 position, Quaternion rotation, float range, float bulletSpeed, GameObject shooter)
    {
        base.ProjectileWeaponFire(position, rotation, range, bulletSpeed, shooter);
    }
    
    
}
