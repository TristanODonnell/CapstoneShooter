using System.Collections;
using System.Collections.Generic;
using gricel;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int damage;
    public ObjectPool objectPool;
    private bool isReturning = false;
    public WeaponLogic weaponLogic;
    public WeaponData currentWeaponData;
    private void OnCollisionEnter(Collision collision)
    {
        ProtectionValues protectionValues = WeaponData.GetProtectionValues(currentWeaponData.damageType);
        weaponLogic.DealDamage(protectionValues, collision.transform);
        Invoke(nameof(ReturnToPool), 2f);
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
