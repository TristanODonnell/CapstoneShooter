using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;
using static gricel.HealthSystem;

public class XPKit : PassiveAttribute
{

    public XPKit(PassiveData passiveData) : base(passiveData) { }
    //Makes the player gain a boost of 250% xp gain but reduces all health bars to a 80% and max ammunition by 80%
    public override void ApplyEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.xpSystem.SetPassiveXPMultiplier(2.5f);
        passiveBehavior.player.SetHealthPassiveModifier(0.15f, 0.80f, 0.35f, 0.80f, 0.50f, 0.80f, 1.00f);
        passiveBehavior.shoot.SetWeaponPassive(1.0f, 1.0f, 1.0f, 0.80f);
    }

    public override void RemoveEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.xpSystem.SetPassiveXPMultiplier(1.0f);
        passiveBehavior.shoot.SetWeaponPassive(1.0f, 1.0f, 1.0f, 1.00f);
        passiveBehavior.player.SetHealthPassiveModifier(0.15f, 1.0f, 0.35f, 1.0f, 0.50f, 1.0f, 1.00f);
    }
}
