using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponLogic
{
    public MeleeWeapon(ShootBehavior shootBehavior, WeaponData weaponData, ObjectPool objectPool, GameObject shooter) : base(shootBehavior, weaponData, objectPool, shooter)
    {
    }

    public override void StartShooting(Transform transform)
    {

    }
    public override void Shooting(Transform transform)
    {

    }
    public override void StopShooting(Transform transform)
    {

    }
}
