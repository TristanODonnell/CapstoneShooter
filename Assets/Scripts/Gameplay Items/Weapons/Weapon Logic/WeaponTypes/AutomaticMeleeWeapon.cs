using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticMeleeWeapon : WeaponLogic
{
    private bool isFiring = false;
    public AutomaticMeleeWeapon(ShootBehavior shootBehavior, WeaponData weaponData, ObjectPool objectPool, GameObject shooter) : base(shootBehavior, weaponData, objectPool, shooter)
    {
        currentWeaponData = weaponData;
        this.objectPool = objectPool;
        this.shooter = shooter;
    }

    public override void StartShooting(Transform transform, bool useAmmo = true)
    {
        isFiring = true;
        lastFireTime = Time.time;
        if (currentWeaponData.currentMagazineAmmo > 0 && !isReloading)
        {
            MeleeWeaponFire(transform.position, transform.rotation, currentWeaponData.range);
            if (useAmmo)
            {
                ApplyRecoil();
                currentWeaponData.currentMagazineAmmo -= 1;
            }
        }
        else if (currentWeaponData.currentMagazineAmmo <= 0 && useAmmo)
        {
            StopShooting(transform);
        }
    }
    public override void Shooting(Transform transform, bool useAmmo = true)
    {
        if (isFiring)
        {
            if (Time.time - lastFireTime >= currentWeaponData.fireRate)
            {
                lastFireTime = Time.time;
                if (currentWeaponData.currentMagazineAmmo > 0 && !isReloading)
                {
                    MeleeWeaponFire(transform.position, transform.rotation, currentWeaponData.range);
                    if (useAmmo)
                    {
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
    public override void StopShooting(Transform transform, bool useAmmo = true)
    {
        if (currentWeaponData.currentMagazineAmmo <= 0 && useAmmo)
            ReloadLogic();
    }
    public override void MeleeWeaponFire(Vector3 position, Quaternion rotation, float range)
    {
        base.MeleeWeaponFire(position, rotation, range);
    }
}


