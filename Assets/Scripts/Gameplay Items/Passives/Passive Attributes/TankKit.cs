using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class TankKit : PassiveAttribute
{
    public TankKit(PassiveData passiveData) : base(passiveData) { }

    public override void ApplyEffects(PassiveBehavior passiveBehavior)
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveEffects(PassiveBehavior passiveBehavior)
    {
        throw new System.NotImplementedException();
    }
}
