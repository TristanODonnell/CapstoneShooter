using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;
using static WeaponData;

public abstract class WeaponLogic
{
    //FIRE RATES, keep in the 0.0x for fast full autos, up to whole numbers for slow fire 
    protected ObjectPool objectPool;
    public WeaponData currentWeaponData;
    protected ShootBehavior shootBehavior;
    protected LookBehavior lookBehavior;
    protected bool isReloading = false;
    protected float lastFireTime = 0f;
    protected float totalVerticalRecoil = 0f;
    protected float totalHorizontalRecoil = 0f;
    public WeaponLogic(ShootBehavior shootBehavior, DamageType damageType) 
    {
        this.shootBehavior = shootBehavior;
        lookBehavior = shootBehavior.lookBehavior;
    }

    protected WeaponLogic(ShootBehavior shootBehavior)
    {
        this.shootBehavior = shootBehavior;
    }

    private void OnReloadComplete()
    {
        isReloading = false;
    }
    private IEnumerator ReloadCoroutine()
    {
        yield return shootBehavior.Reload(OnReloadComplete);
    }
    public virtual void ReloadLogic()
    {
        if (isReloading) return;
        isReloading = true;
        shootBehavior.StartCoroutine(ReloadCoroutine());
    }
    public virtual void StartShooting(Transform transform)
    {
         
    }
    public virtual void Shooting(Transform transform )
    {

    }
    public virtual void StopShooting(Transform transform )
    {

    }

    public virtual void ProjectileWeaponFire(Vector3 position, Quaternion rotation, float range, float bulletSpeed)
    {
        PooledObject pooledObject = objectPool.RetrievePoolObject();
    if (pooledObject == null)
    {
        Debug.LogWarning("No available bullets in the pool.");
        return; // Exit if no bullet is available
    }
    Bullet bullet = pooledObject.GetComponent<Bullet>();
    if (bullet != null)
    {
            bullet.objectPool = objectPool;
            bullet.weaponLogic = this;
            bullet.currentWeaponData = currentWeaponData;
            bullet.ResetBullet();
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            bullet.gameObject.SetActive(true);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
            rb.velocity = Vector3.zero;
            rb.AddForce(rotation * Vector3.forward * bulletSpeed, ForceMode.Impulse);
            }
            else
            {
            Debug.LogWarning("No Rigidbody component found on the bullet!");
            }
    }
    else
    {
        Debug.LogWarning("No Bullet component found on the pooled object!");
    }
    }

    public virtual void HitscanWeaponFire(Vector3 position, Quaternion rotation, float range)
    {

        RaycastHit hit;
        Vector3 direction = rotation * Vector3.forward;

        if (Physics.Raycast(position, direction, out hit, range))
        {
            Debug.Log("Hitscan hit something...");
            DealDamage(GetProtectionValues(currentWeaponData.damageType), hit.transform);
        }
        else
        {
            Debug.Log("Hitscan did not hit anything.");
        }

        
    }


    //NEED TO FINISH UP MELEE HERE
    public void MeleeWeaponFire(Vector3 position, Quaternion rotation, float range)
    {
        Debug.Log("Shooting melee...");
        Collider[] hits = Physics.OverlapSphere(position, range);
        foreach (Collider hit in hits)
        {
            {
                Debug.Log("Melee hit something...");
                DealDamage(GetProtectionValues(currentWeaponData.damageType), hit.transform);
            }
        }
    }
    public virtual void ApplyRecoil()
    {
        float randomVertical = UnityEngine.Random.Range(0, currentWeaponData.verticalRecoil);
        float randomHorizontal = UnityEngine.Random.Range(-currentWeaponData.horizontalRecoil, currentWeaponData.horizontalRecoil);
        Debug.Log($"Generated Recoil - Vertical: {randomVertical}, Horizontal: {randomHorizontal}");
        lookBehavior.ApplyCameraRecoil(randomVertical, randomHorizontal);
    }

    public virtual void DealDamage(Transform transform)
    {
        ProtectionValues damageType = GetProtectionValues(currentWeaponData.damageType);
        DealDamage(damageType, transform);
    }

    public virtual void DealDamage(ProtectionValues protectionValues, Transform transform)
    {
        Hitbox hitbox = transform.GetComponent<Hitbox>();
        hitbox.Damage(protectionValues);
    }
}
