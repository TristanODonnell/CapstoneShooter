using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class NoPassive : PassiveAttribute
{
    public NoPassive(PassiveData passiveData) : base(passiveData)
    {
    }

    

    public override void ApplyEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
      //nothing here
    }
    public override void RemoveEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        throw new System.NotImplementedException();
    }
}
