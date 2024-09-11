using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAttribute
{
    protected PassiveData passiveData;

    public PassiveAttribute(PassiveData passiveData)
    {
        this.passiveData = passiveData;
    }

    public abstract void ApplyEffects(PassiveBehavior passiveBehavior);
    public abstract void RemoveEffects(PassiveBehavior passiveBehavior);
}
