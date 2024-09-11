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
        passiveBehavior.movement.SpeedValue *= 1.5f;
        // makes the energy shield bar 75% of its usual capacity

    }

    public override void RemoveEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.movement.SpeedValue *= 1f;
    }
}
