using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.Rendering.DebugUI;

public class RamboKit : PassiveAttribute
{
    public RamboKit(PassiveData passiveData) : base(passiveData)
    {
    }

    public override void ApplyEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        //The flesh and armour healthbars are now decreased to 0 %,
        //the energy shield is now 100 % of the health bar, but reduced to 75 %,
        //it also increases the damage of all weapons to 150 % and
        //magazine size to 150 %, and
        //increases reload speed by 150 %

    }

    public override void RemoveEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        throw new System.NotImplementedException();
    }
}
