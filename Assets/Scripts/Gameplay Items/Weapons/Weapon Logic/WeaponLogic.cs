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
    protected GameObject shooter;

    protected WeaponLogic(ShootBehavior shootBehavior, WeaponData weaponData, ObjectPool objectPool, GameObject shooter)
    {
        this.shootBehavior = shootBehavior; lookBehavior = shootBehavior.lookBehavior; this.shooter = shooter;
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

    public virtual void ProjectileWeaponFire(Vector3 position, Quaternion rotation, float range, float bulletSpeed, GameObject shooter)
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
            bullet.SetShooter(shooter);
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

    public virtual void HitscanWeaponFire(Vector3 position, Quaternion rotation, float range )
    {

        RaycastHit hit;
        Vector3 direction = rotation * Vector3.forward;
        int layerMask = LayerMask.GetMask("Enemy");
        if (Physics.Raycast(position, direction, out hit, range, layerMask))
        {
            Debug.Log($"Raycast hit at position: {hit.point}");
            Hitbox hitbox = hit.transform.GetComponent<Hitbox>();
            if (hitbox != null)
            {
                hitbox.Damage(currentWeaponData.ProtectionValues);
                HealthSystem healthSystem = hit.transform.GetComponentInParent<HealthSystem>();
                if (healthSystem != null)
                {
                    if (healthSystem.healthBars[0].H_IsDepleted())
                    {
                        // Enemy is dead, do something (e.g., destroy the enemy, play a death animation, etc.)
                        Debug.Log("Enemy is dead!");
                    }
                    else
                    {
                        // Enemy is damaged, do something (e.g., play a hurt animation, etc.)
                        Debug.Log("Enemy is damaged!");
                    }
                }
                else
                {
                    Debug.LogError("No HealthSystem component found on the hit object.");
                }
            }
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
                
            }
        }
    }
    public virtual void ApplyRecoil()
    {
        float randomVertical = UnityEngine.Random.Range(0, currentWeaponData.verticalRecoil);
        float randomHorizontal = UnityEngine.Random.Range(-currentWeaponData.horizontalRecoil, currentWeaponData.horizontalRecoil);
        //Debug.Log($"Generated Recoil - Vertical: {randomVertical}, Horizontal: {randomHorizontal}");
        lookBehavior.ApplyCameraRecoil(randomVertical, randomHorizontal);
    }

}
