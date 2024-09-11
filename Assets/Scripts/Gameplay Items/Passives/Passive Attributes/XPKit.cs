using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;
using static gricel.HealthSystem;

public class XPKit : PassiveAttribute
{

    public XPKit(PassiveData passiveData) : base(passiveData) { }

    public override void ApplyEffects(PassiveBehavior passiveBehavior)
    {
        // Makes the player gain a boost of 250 % xp gain 
        passiveBehavior.xpSystem.ApplyXPModifiers(2.5f);
           // but reduces all health bars to a 80 %
           // and max ammunition by 80 %
    }

    public override void RemoveEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.xpSystem.ApplyXPModifiers(1f);
    }
}
