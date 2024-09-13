using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class NoPassive : PassiveAttribute
{
    public NoPassive(PassiveData passiveData) : base(passiveData)
    {
    }

    public override void ApplyEffects(PassiveBehavior passiveBehavior)
    {
        //double check past passive values erase
    }

    public override void RemoveEffects(PassiveBehavior passiveBehavior)
    {
        //double check next passive changes values
    }
}
