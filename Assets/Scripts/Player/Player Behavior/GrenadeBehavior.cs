using System.Collections;
using System.Collections.Generic;
using throwables;
using throwables.Items;
using UnityEngine;


public class GrenadeBehavior : MonoBehaviour
{

    [SerializeField] private float force;
    [SerializeField] private float upwardForce;

    public GrenadeManager grenadeManager;
    [SerializeField] private Camera myCamera;
    public void ThrowGrenade(ThrowableItem grenade)
    {
        if (grenadeManager.currentGrenade != null)
        {
            Debug.Log("ThrowGrenade called with grenade: " + grenade);
            gricel.HealthSystem thrower = gameObject.GetComponent<gricel.HealthSystem>();
            Vector3 direction = myCamera.transform.forward * 1f;
            Vector3 pivotPoint = myCamera.transform.position + direction * 0.5f;
            float powerIncrease = 1f;
            Vector3 forceVector = new Vector3(direction.x, upwardForce, direction.z);

            float grenadeModifier = GetGrenadeDamageModifier();
            float modifiedPowerIncrease = powerIncrease * grenadeModifier;
            var myGrenade = grenade.Throw(thrower, pivotPoint, direction, force, modifiedPowerIncrease);
            float damageModifier = GetGrenadeDamageModifier();
            
        }
    }

    public float GetGrenadeDamageModifier()
    {
        int currentGrenadeLevel = ModifierManager.Singleton.currentGrenadeDamageLevel;
        float grenadeModifier = ModifierManager.Singleton.grenadeDamageModifiers[currentGrenadeLevel - 1];

        return grenadeModifier;
    }
}
