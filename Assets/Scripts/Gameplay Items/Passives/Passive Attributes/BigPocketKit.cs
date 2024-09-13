using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class BigPocketKit : PassiveAttribute
{
    public BigPocketKit(PassiveData passiveData) : base(passiveData)
    {
    }
    //Increases the max ammount of magazines +4 and the magazine size by 200% but increases reload time by 250%
    public override void ApplyEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.shoot.SetWeaponPassive(1.0f, 2.0f, 2.5f, 1.75f );
    }

    public override void RemoveEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.shoot.SetWeaponPassive(1.0f, 1.0f, 1.0f, 1.00f);
    }
}
