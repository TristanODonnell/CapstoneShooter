using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiotShieldWeapon : WeaponLogic
{
    public RiotShieldWeapon(ShootBehavior shootBehavior, WeaponData weaponData, ObjectPool objectPool, GameObject shooter) : base(shootBehavior, weaponData, objectPool, shooter)
    {
        currentWeaponData = weaponData;
        this.objectPool = objectPool;
        this.shooter = shooter;
    }

    public override void StartShooting(Transform transform)
    {
        if (Time.time - lastFireTime < currentWeaponData.fireRate)
        {
            return; // Exit early if we're still within the fire rate cooldown
        }
        if (currentWeaponData.currentMagazineAmmo > 0 && !isReloading)
        {
            lastFireTime = Time.time;
            MeleeWeaponFire(transform.position, transform.rotation, currentWeaponData.range);
            ApplyRecoil();
            currentWeaponData.currentMagazineAmmo -= 1;
        }
        else if (currentWeaponData.currentMagazineAmmo <= 0)
        {
            StopShooting(transform);
        }
    }
    public override void Shooting(Transform transform)
    {

    }
    public override void StopShooting(Transform transform)
    {
        if (currentWeaponData.currentMagazineAmmo <= 0)
        {
            ReloadLogic();
        }
    }
    public override void MeleeWeaponFire(Vector3 position, Quaternion rotation, float range)
    {
        base.MeleeWeaponFire(position, rotation, range);
    }
}
