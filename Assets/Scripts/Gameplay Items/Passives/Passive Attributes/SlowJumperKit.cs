using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class SlowJumperKit : PassiveAttribute
{
    public SlowJumperKit(PassiveData passiveData) : base(passiveData)
    {
    }
    //Increases the jumping strength by 150% Reduces the speed to 75%
    public override void ApplyEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.movement.SetPlayerSpeedModifier(0.75f);
        passiveBehavior.player.SetJumpPassiveModifier(1.5f);
    }

    public override void RemoveEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.movement.SetPlayerSpeedModifier(1.0f);
        passiveBehavior.player.SetJumpPassiveModifier(1.0f);
    }
}
