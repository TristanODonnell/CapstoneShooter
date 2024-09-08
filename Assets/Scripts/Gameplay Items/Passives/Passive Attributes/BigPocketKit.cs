using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class BigPocketKit : PassiveAttribute
{
    public BigPocketKit(PassiveData passiveData) : base(passiveData)
    {
    }

   
    public override void ApplyEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        //Increases the max ammount of magazines +4
        //and the magazine size by 200% but
        //increases reload time by 250%

      //  player.playerModifiers.magazineSizeMultiplier = 2; // 200% increase
       // player.playerModifiers.maxAmmoAddition = 4;
      //  player.playerModifiers.reloadTimeMultiplier = 2.5f; // 250% increase
    }

    public override void RemoveEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        throw new System.NotImplementedException();
    }
}
