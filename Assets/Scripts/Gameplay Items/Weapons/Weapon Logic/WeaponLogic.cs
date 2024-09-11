using System.Collections;
using System.Collections.Generic;
using System.Linq;
using gricel;
using UnityEngine;



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
        GameObject projectileVisualInstance = Object.Instantiate(currentWeaponData.projectileVisualPrefab);
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
            float scale = currentWeaponData.bulletScale;
            bullet.transform.localScale = new Vector3(scale, scale, scale);
            projectileVisualInstance.transform.localScale = bullet.transform.localScale;
            projectileVisualInstance.transform.parent = bullet.transform;
            projectileVisualInstance.transform.localPosition = Vector3.zero;
            projectileVisualInstance.transform.localRotation = Quaternion.identity;
            
            if (rb != null)
            {
            rb.velocity = Vector3.zero;
            rb.AddForce(shootBehavior.GetShootingOrigin().forward * bulletSpeed, ForceMode.Impulse);
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
        HashSet<GameObject> hitObjects = new HashSet<GameObject>();
        float remainingPenetration = currentWeaponData.penetration;
        int iterationCount = 0;
        while (remainingPenetration > 0 && range > 0)
        {
            iterationCount++;
            
            if (iterationCount > 100)
            {
                Debug.LogError("Too many iterations, breaking loop.");
                break;
            }
            Debug.Log($"Iteration {iterationCount}: remainingPenetration = {remainingPenetration}, range = {range}");
            RaycastHit hit; 
            Vector3 direction = rotation * Vector3.forward;
            //int layerMask = LayerMask.GetMask("Enemy", "Player");
            if (Physics.Raycast(shootBehavior.GetShootingOrigin().position, shootBehavior.GetShootingOrigin().forward, out hit, range))
            {
                Debug.Log($"Raycast hit: {hit.transform.name} at position: {hit.point}");
                GameObject hitParent = hit.transform.root.gameObject;
                if (!hitObjects.Contains(hitParent))
                {
                    
                    Hitbox hitbox = hit.transform.GetComponent<Hitbox>();
                    if (hitbox != null)
                    {
                        hitbox.Damage(currentWeaponData.ProtectionValues);
                        HealthSystem healthSystem = hit.transform.GetComponentInParent<HealthSystem>();
                         
                        if (healthSystem != null)
                        {
                            hitObjects.Add(hitParent); // Add the parent (enemy) to the HashSet
                            Debug.Log($"Added to hitObjects: {hitParent.name}");

                            if (healthSystem.healthBars.Length > 0 && healthSystem.healthBars[0] != null)
                            {
                                if (healthSystem.healthBars[0].H_IsDepleted())
                                {
                                    Debug.Log("Enemy is dead!");
                                }
                                else
                                {
                                    Debug.Log("Enemy is damaged!");
                                }
                            }
                            else
                            {
                                Debug.LogError("Health bars are not properly set up.");
                            }
                        }
                        else
                        {
                            Debug.LogError("No HealthSystem component found on the hit object.");
                        }
                    }
                    float distanceToHit = Vector3.Distance(position, hit.point);
                    range -= distanceToHit;
                    position = hit.point;
                    remainingPenetration -= currentWeaponData.penetrationPerHit;
                    remainingPenetration = Mathf.Max(0, remainingPenetration);
                }
                else
                {
                    Debug.Log($"Hit already processed: {hitParent.name}");
                    break;
                }
            }
            else
            {
                Debug.Log("Hitscan did not hit anything.");
                break;
            }
        }
        Debug.Log($"Loop terminated after {iterationCount} iterations.");
    }
    public virtual void MeleeWeaponFire(Vector3 position, Quaternion rotation, float range)
    {
        Debug.Log("Shooting melee...");
        Collider[] hits = Physics.OverlapSphere(shootBehavior.GetShootingOrigin().position, range);
        foreach (Collider hit in hits)
        {
            Hitbox hitbox = hit.transform.GetComponent<Hitbox>();
            if (hitbox != null)
            {
                hitbox.Damage(currentWeaponData.ProtectionValues);
                // Stop iterating as soon as we find the first valid hit
                return;
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
