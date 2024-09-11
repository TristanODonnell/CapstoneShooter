using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class BigPocketKit : PassiveAttribute
{
    public BigPocketKit(PassiveData passiveData) : base(passiveData)
    {
    }

    public override void ApplyEffects(PassiveBehavior passiveBehavior)
    {
        //Increases the max ammount of magazines +4 and the magazine size by 200% but increases reload time by 250%
    }

    public override void RemoveEffects(PassiveBehavior passiveBehavior)
    {
        throw new System.NotImplementedException();
    }
}
