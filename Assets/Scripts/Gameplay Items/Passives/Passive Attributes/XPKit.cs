using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class XPKit : PassiveAttribute
{
    public XPKit(PassiveData passiveData) : base(passiveData) { }
    

    public override void ApplyEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        //Makes the player gain a boost of 250 % xp gain
        //but reduces all health bars to a 80 % and max ammunition by 80 %

        //health 80%
        //health 80%
        //health 80%
        //250% xp gain, need to add XP system still 
    }

    public override void RemoveEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        throw new System.NotImplementedException();
    }
}
