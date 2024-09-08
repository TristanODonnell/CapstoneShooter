using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class TankKit : PassiveAttribute
{
    public TankKit(PassiveData passiveData) : base(passiveData) { }



   

    public override void ApplyEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        //makes the armour health bar now beworth 30% of total health while
        //flesh health is now worth 10%, and decreases speed to 90%
        movement.speed *= 0.9f;

        foreach (var healthBar in health.healthBars)
        {
            if (healthBar.protection == HealthSystem.Health.HealthT.armor)
            {
                healthBar.health_Max = healthBar.health_Max * 0.3f;
            }
            else if (healthBar.protection == HealthSystem.Health.HealthT.unprotected)
            {
                healthBar.health_Max = healthBar.health_Max * 0.1f;
            }
        }
    }

    public override void RemoveEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        throw new System.NotImplementedException();
    }
}
