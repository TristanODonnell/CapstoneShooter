using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveBashBehavior : MonoBehaviour
{
    [SerializeField] private ShootBehavior shootBehavior;
    [SerializeField] private float meleeDamage;
    private bool isMelee;

    public void UseRecursiveBash()
    {
        if (isMelee)
        {
            return; // Exit early if we're still within the fire rate cooldown
        }
        float meleeRange = Vector3.Distance(shootBehavior.GetShootingOrigin().position, transform.position);
        Collider[] hits = Physics.OverlapSphere(shootBehavior.GetShootingOrigin().position, meleeRange);
        foreach (Collider hit in hits)
        {
            Hitbox hitbox = hit.transform.GetComponent<Hitbox>();
            if (hitbox != null)
            {
                hitbox.Damage(meleeDamage);
                // Stop iterating as soon as we find the first valid hit
                return;
            }
        }
    }
    public void SetRecursiveBashModifier()
    {
        int currentRecursiveBashModifier = ModifierManager.Singleton.currentRecursiveBashLevel;
        float recursiveModifier = ModifierManager.Singleton.recursiveBashModifiers[currentRecursiveBashModifier -1];

        float modifiedMeleeDamage = meleeDamage * recursiveModifier;
        meleeDamage  = modifiedMeleeDamage;
    }
}
