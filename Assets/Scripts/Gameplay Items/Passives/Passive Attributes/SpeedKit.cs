using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class SpeedKit : PassiveAttribute
{
    public SpeedKit(PassiveData passiveData) : base(passiveData)
    {

    }

    
    public override void ApplyEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {

        //Increases the player speed to 150% but it makes the energy shield bar 75% of its usual capacity
        movement.SpeedValue *= 1.5f;
        //player.playerModifiers.speedMultiplier = 1.5f;
        // health ENERGY SHIELD BAR 75% capacity
    }

    public override void RemoveEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        movement.SpeedValue *= 1f;
    }
}
