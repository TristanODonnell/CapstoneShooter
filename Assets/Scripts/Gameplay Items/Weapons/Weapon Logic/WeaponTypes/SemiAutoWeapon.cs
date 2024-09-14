using System.Collections;
using System.Collections.Generic;
using System.Linq;
using gricel;
using UnityEngine;


public class SemiAutoWeapon : WeaponLogic
{
    public SemiAutoWeapon(ShootBehavior shootBehavior, WeaponData weaponData, ObjectPool objectPool, GameObject shooter) : base(shootBehavior, weaponData, objectPool, shooter)
    {
    currentWeaponData = weaponData;
        this.objectPool = objectPool;
        this.shooter = shooter;
    }
    public override void ReloadLogic()
    {
        base.ReloadLogic();
    }
    public override void StartShooting(Transform transform, bool useAmmo = true)
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
                ProjectileWeaponFire(transform.position, transform.rotation, currentWeaponData.range, currentWeaponData.bulletSpeed, this.shooter);
                if (useAmmo)
                {
                    ApplyRecoil();
                    currentWeaponData.currentMagazineAmmo -= 1;
                }
                
            }
            else if (currentWeaponData.weaponType == WeaponData.WeaponType.Hitscan)
            {
                HitscanWeaponFire(transform.position, transform.rotation, currentWeaponData.range);
                if (useAmmo)
                {
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
        //nothing for holding down semi auto
    }
    public override void StopShooting(Transform transform, bool useAmmo = true)
    {
        if (currentWeaponData.currentMagazineAmmo <= 0 && useAmmo)
        {
            ReloadLogic();
        }
    }
    public override void ProjectileWeaponFire(Vector3 position, Quaternion rotation, float range,  float bulletSpeed, GameObject shooter)
    {
        base.ProjectileWeaponFire(position, rotation, range,  bulletSpeed, shooter);
    }
    public override void HitscanWeaponFire(Vector3 position, Quaternion rotation, float range)
    {
        //Debug.Log($"HitscanWeaponFire called with Position: {position}, Rotation: {rotation}, Range: {range}");
        base.HitscanWeaponFire(position, rotation, range);
    }

    public override void ApplyRecoil()
    {
        base.ApplyRecoil();
    }

    
}
