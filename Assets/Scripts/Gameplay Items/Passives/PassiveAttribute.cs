using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAttribute
{
    protected PassiveData passiveData;

    public PassiveAttribute(PassiveData passiveData)
    {
        this.passiveData = passiveData;
    }
    public abstract void ApplyEffects(PlayerController player, Hitbox hitBox, gricel.HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational);


    public abstract void RemoveEffects(PlayerController player, Hitbox hitBox, gricel.HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational);
}
