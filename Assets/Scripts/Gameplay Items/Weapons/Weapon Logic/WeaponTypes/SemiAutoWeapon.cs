using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WeaponData;

public class SemiAutoWeapon : WeaponLogic
{
    public SemiAutoWeapon(ShootBehavior shootBehavior, WeaponData weaponData, ObjectPool objectPool, DamageType damageType) 
        : base(shootBehavior)
    {
        currentWeaponData = weaponData;
        this.objectPool = objectPool;
        
    }
    public override void ReloadLogic()
    {
        base.ReloadLogic();
    }
    public override void StartShooting(Transform transform)
    {
        if (Time.time - lastFireTime < currentWeaponData.fireRate)
        {
            return; // Exit early if we're still within the fire rate cooldown
        }

        Debug.Log("Current weapon type: " + currentWeaponData.weaponType);


        if ( currentWeaponData.currentMagazineAmmo > 0 && !isReloading)
        {
            lastFireTime = Time.time; // Update last fire time to enforce fire rate
            
            if (currentWeaponData.weaponType == WeaponData.WeaponType.Projectile)
            {
                if (objectPool == null)
                {
                    Debug.LogError("Object pool is not initialized for weapon: ");
                    return; // Exit if the object pool is not initialized
                }
                ProjectileWeaponFire(transform.position, transform.rotation, currentWeaponData.range, currentWeaponData.bulletSpeed);
                ApplyRecoil();
                currentWeaponData.currentMagazineAmmo -= 1;
                
            }
            else if (currentWeaponData.weaponType == WeaponData.WeaponType.Hitscan)
            {
                HitscanWeaponFire(transform.position, transform.rotation, currentWeaponData.range);
                ApplyRecoil();
                currentWeaponData.currentMagazineAmmo -= 1;
                
            }
            
        }
        else if (currentWeaponData.currentMagazineAmmo <= 0)
            {
                StopShooting(transform);
            }

    }
    public override void Shooting(Transform transform)
    {
        //nothing for holding down semi auto
    }
    public override void StopShooting(Transform transform)
    {
        if (currentWeaponData.currentMagazineAmmo <= 0)
        {
            ReloadLogic();
        }
    }
    public override void ProjectileWeaponFire(Vector3 position, Quaternion rotation, float range,  float bulletSpeed)
    {
        base.ProjectileWeaponFire(position, rotation, range,  bulletSpeed);
    }
    public override void HitscanWeaponFire(Vector3 position, Quaternion rotation, float range)
    {
        Debug.Log($"HitscanWeaponFire called with Position: {position}, Rotation: {rotation}, Range: {range}");
        base.HitscanWeaponFire(position, rotation, range);
    }

    public override void ApplyRecoil()
    {
        base.ApplyRecoil();
    }
}
