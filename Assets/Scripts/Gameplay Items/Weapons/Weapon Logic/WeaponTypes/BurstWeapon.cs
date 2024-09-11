using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BurstWeapon : WeaponLogic
{
    private bool isFiringBurst = false;
    public BurstWeapon(ShootBehavior shootBehavior, WeaponData weaponData, ObjectPool objectPool, GameObject shooter) : base(shootBehavior, weaponData, objectPool, shooter)
    {
        currentWeaponData = weaponData;
        this.objectPool = objectPool;
        this.shooter = shooter;
    }
    
    public override void StartShooting(Transform transform)
    {
        Debug.Log("StartShooting called");
        if (Time.time - lastFireTime < currentWeaponData.fireRate)
        {
            Debug.Log("Still within fire rate cooldown");
            return; // Exit early if we're still within the fire rate cooldown
        }
        if (currentWeaponData.weaponType == WeaponData.WeaponType.Projectile)
        {
            if (objectPool == null)
            {
                Debug.LogError("Object pool is not initialized for weapon: ");
                return; // Exit if the object pool is not initialized
            }
            isFiringBurst = true;
            shootBehavior.StartCoroutine(FireBurstAndWait(transform));
        }
        else if (currentWeaponData.weaponType == WeaponData.WeaponType.Hitscan)
        {
            isFiringBurst = true;
            shootBehavior.StartCoroutine(FireBurstAndWait(transform));
        }
        else if (currentWeaponData.currentMagazineAmmo <= 0)
        {
            StopShooting(transform);
        }
    }

    public IEnumerator FireBurstAndWait(Transform transform)
    {
        for (int i = 0; i < currentWeaponData.burstSize; i++)
        {
            if (currentWeaponData.weaponType == WeaponData.WeaponType.Projectile)
            {
                ProjectileWeaponFire(transform.position, transform.rotation, currentWeaponData.range, currentWeaponData.bulletSpeed, this.shooter);
            }
            else if (currentWeaponData.weaponType == WeaponData.WeaponType.Hitscan)
            {
                HitscanWeaponFire(transform.position, transform.rotation, currentWeaponData.range);
            }
            ApplyRecoil();
            currentWeaponData.currentMagazineAmmo -= 1;

            yield return new WaitForSeconds(currentWeaponData.burstDelay);
        }

        OnBurstFinished();
    }

    private void OnBurstFinished()
    {
        // Code to execute after the burst has finished
        // You can reset the isFiringBurst flag here, for example
        isFiringBurst = false;
    }
    public override void ReloadLogic()
    {
        base.ReloadLogic();
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
    public override void ProjectileWeaponFire(Vector3 position, Quaternion rotation, float range, float bulletSpeed, GameObject shooter)
    {
        Debug.Log("ProjectileWeaponFire called with Position: " + position + ", Rotation: " + rotation + ", Range: " + range);
        base.ProjectileWeaponFire(position, rotation, range, bulletSpeed, shooter);
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
