using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class CooldownKit : PassiveAttribute
{
    public CooldownKit(PassiveData passiveData) : base(passiveData)
    {
    }

   
    public override void ApplyEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        //Reduces the cooldown of the ability in a 75%
        //reducing the maximum ammount of magazines the player can carry by -2 magazines



        // equipment lower cooldown for use
        //max magazine changes, maybe another variable? 
    }
    public override void RemoveEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        throw new System.NotImplementedException();
    }
}
