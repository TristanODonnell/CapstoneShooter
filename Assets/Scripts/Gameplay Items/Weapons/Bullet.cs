using System.Collections;
using System.Collections.Generic;
using gricel;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject shooter;

    public ObjectPool objectPool;
    private bool isReturning = false;
    public WeaponLogic weaponLogic;
    public WeaponData currentWeaponData;

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
    }
    private void OnCollisionEnter(Collision collision)
    {
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
        StartCoroutine(ReturnProjectileToPool(0.1f));
    }

    IEnumerator ReturnProjectileToPool(float delay)
    {
        yield return new WaitForSeconds(delay);
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
