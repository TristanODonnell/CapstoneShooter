using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class SlowJumperKit : PassiveAttribute
{
    public SlowJumperKit(PassiveData passiveData) : base(passiveData)
    {
    }


    public override void ApplyEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        //Increases the jumping strength by 150%
        //player.playerModifiers.jumpForceMultiplier = 1.5f;
        //Reduces the speed to 75%
       // player.playerModifiers.speedMultiplier = 0.75f;
    }
    public override void RemoveEffects(PlayerController player, Hitbox hitBox, HealthSystem health, MovementBehavior movement, ShootBehavior shoot, EquipmentBehavior equipment, PassiveBehavior passive, GravitationalBehaviour gravitational)
    {
        throw new System.NotImplementedException();
    }
}
