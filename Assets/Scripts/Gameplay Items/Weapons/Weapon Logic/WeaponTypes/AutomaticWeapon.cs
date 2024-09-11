using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponData;

public class AutomaticWeapon : WeaponLogic
{
    private bool isFiring = false;

    public AutomaticWeapon(ShootBehavior shootBehavior, WeaponData weaponData, ObjectPool objectPool, GameObject shooter) : base(shootBehavior, weaponData, objectPool, shooter)
    {
        currentWeaponData = weaponData;
        this.objectPool = objectPool;
        this.shooter = shooter;
    }

    public override void ReloadLogic()
    {
        base.ReloadLogic();
    }
    public override void StartShooting(Transform transform)
    {
        isFiring = true;
        lastFireTime = Time.time;

        Debug.Log("Current weapon type: " + currentWeaponData.weaponType);
        if (currentWeaponData.currentMagazineAmmo > 0 && !isReloading)
        {
            
            if (currentWeaponData.weaponType == WeaponData.WeaponType.Projectile)
            {
                if (objectPool == null)
                {
                    return; // Exit if the object pool is not initialized
                }
                ProjectileWeaponFire(transform.position, transform.rotation, currentWeaponData.range, currentWeaponData.bulletSpeed, shooter);
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
    public override void Shooting(Transform transform )
    {
        if (isFiring)
        {
            if (Time.time - lastFireTime >= currentWeaponData.fireRate)
            {
                lastFireTime = Time.time;
                if (currentWeaponData.currentMagazineAmmo > 0 && !isReloading)
                {
                    
                    if (currentWeaponData.weaponType == WeaponData.WeaponType.Projectile)
                    {
                        if (objectPool == null)
                        {
                            return; // Exit if the object pool is not initialized
                        }
                        ProjectileWeaponFire(transform.position, transform.rotation, currentWeaponData.range, currentWeaponData.bulletSpeed, shooter);
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
            }

        }
        else if (currentWeaponData.currentMagazineAmmo <= 0)
        {
            StopShooting(transform);
        }
    }
    public override void StopShooting(Transform transform)
    {
        isFiring = false;
        if (currentWeaponData.currentMagazineAmmo <= 0)
        {
            ReloadLogic();
        }
    }
    public override void ProjectileWeaponFire(Vector3 position, Quaternion rotation, float range, float bulletSpeed, GameObject shooter) => base.ProjectileWeaponFire(position, rotation, range, bulletSpeed, shooter);
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
