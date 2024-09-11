using System.Collections;
using System.Collections.Generic;
using gricel;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static WeaponData;

public class Bullet : MonoBehaviour
{
    private GameObject shooter;

    public ObjectPool objectPool;
    private bool isReturning = false;
    public WeaponLogic weaponLogic;
    public WeaponData currentWeaponData;
    public float bulletLifetime = 0.3f;
    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;

        // Ignore collision between the bullet and the shooter
        Collider shooterCollider = shooter.GetComponent<Collider>();
        Collider bulletCollider = GetComponent<Collider>();

        if (shooterCollider != null && bulletCollider != null)
        {
            Physics.IgnoreCollision(bulletCollider, shooterCollider);
        }

        StartCoroutine(ReturnProjectileToPoolAfterLifetime());
    }
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet collided with: " + other.gameObject.name);
        ProcessBulletHit(other, currentWeaponData.penetration);
        StartCoroutine(ReturnProjectileToPool(0.1f));
    }
    private void ProcessBulletHit(Collider other, float penetration)
    {

        Debug.Log("Bullet collided with: " + other.gameObject.name);

        if(currentWeaponData.bulletType == BulletType.Explosive)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentWeaponData.explosionRadius);
            HashSet<GameObject> parents = new HashSet<GameObject>();
            foreach (Collider hitCollider in hitColliders)
            {
                GameObject hitParent = hitCollider.transform.root.gameObject;
                if (!hitEnemies.Contains(hitParent))
                {
                    hitEnemies.Add(hitParent);
                    Hitbox hitbox = hitCollider.GetComponent<Hitbox>();
                    if (hitbox != null)
                    {
                        hitbox.Damage(currentWeaponData.ProtectionValues);
                        HealthSystem healthSystem = hitParent.GetComponent<HealthSystem>();
                        if (healthSystem != null)
                        {
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
                        }
                    }
                }
            }
        }
        else
        {
            Hitbox hitbox = other.transform.GetComponent<Hitbox>();
            if (hitbox != null)
            {
                float remainingPenetration = currentWeaponData.penetration;
                Vector3 startPosition = other.transform.position;
                Vector3 direction = other.transform.forward;
                int iterationCount = 0;
                while (remainingPenetration > 0)
                {
                    Debug.Log($"Iteration {iterationCount}: remainingPenetration = {remainingPenetration}");
                    iterationCount++;
                        Debug.Log("Bullet hit an enemy!");
                        GameObject hitParent = other.transform.root.gameObject;
                        if (!hitEnemies.Contains(hitParent))
                        {
                            hitbox.Damage(currentWeaponData.ProtectionValues);
                            HealthSystem healthSystem = other.gameObject.GetComponentInParent<HealthSystem>();
                            if (healthSystem != null)
                            {
                                hitEnemies.Add(hitParent); // Add the parent (enemy) to the HashSet
                                Debug.Log($"Added to hitEnemies: {hitParent.name}");
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
                            // Reduce penetration distance based on the enemy's thickness
                            remainingPenetration -= currentWeaponData.penetrationPerHit;
                            remainingPenetration = Mathf.Max(0, remainingPenetration);
                        }
                        else
                        {
                            Debug.Log($"Hit already processed: {other.transform.root.gameObject.name}");
                            break;
                        }
                }
            }
        }      
    }

    IEnumerator ReturnProjectileToPool(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool();
    }

    private IEnumerator ReturnProjectileToPoolAfterLifetime()
    {
        yield return new WaitForSeconds(bulletLifetime); // Wait for the bullet's lifetime
        ReturnToPool();
    }
    private void ReturnToPool()
    {
        if (isReturning) return;

        isReturning = true;
        this.gameObject.SetActive(false); // Deactivate the bullet first

        ResetBullet();

        if (objectPool != null)
        {
            // Check if the object is already in the pool before adding it back
            if (!objectPool.availableObjects.Contains(this.GetComponent<PooledObject>()))
            {
                objectPool.SendBackToPool(this.GetComponent<PooledObject>());
            }
            else
            {
                Debug.LogWarning("Trying to return an already available object.");
            }
        }
        else
        {
            Debug.LogWarning("Object pool reference is missing!");
        }
    }
    public void ResetBullet() // Reset the bullet's state
    {
        isReturning = false;
        // Reset any bullet-specific logic or state here (e.g., velocity, position, etc.)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        // Add other properties to reset, if needed
    }
}
/*
        switch (currentWeaponData.bulletType)
        {
            case BulletType.Standard:
                Hitbox hitbox = collision.transform.GetComponent<Hitbox>();
                if (hitbox != null)
                {
                    hitbox.Damage(currentWeaponData.ProtectionValues);
                    HealthSystem healthSystem = collision.gameObject.GetComponentInParent<HealthSystem>();
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
                break;
            case BulletType.Penetrating:
                // Implement logic here
                break;
            case BulletType.Explosive:
                // Implement logic here
                break;
        }
        */