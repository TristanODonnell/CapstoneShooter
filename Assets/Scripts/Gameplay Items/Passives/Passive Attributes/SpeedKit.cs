using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class SpeedKit : PassiveAttribute
{
    public SpeedKit(PassiveData passiveData) : base(passiveData)
    {

    }

    public override void ApplyEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.movement.SetPassiveModifier(1.5f);
        passiveBehavior.player.SetHealthPassiveModifier(0.15f, 1.0f, 0.35f, 1.0f, 0.50f, 0.75f, 1.00f);

    }

    public override void RemoveEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.movement.SetPassiveModifier(1.0f);
        passiveBehavior.player.SetHealthPassiveModifier(0.15f, 1.0f, 0.35f, 1.0f, 0.50f, 1.0f, 1.00f);
    }
}
