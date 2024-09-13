using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.Rendering.DebugUI;

public class RamboKit : PassiveAttribute
{//the energy shield is now 100 % of the health bar, but reduced to 75 %,
 //it also increases the damage of all weapons to 150 % and
 //magazine size to 150 %, and increases reload speed by 150 %
    public RamboKit(PassiveData passiveData) : base(passiveData)
    {
    }

    public override void ApplyEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.player.SetHealthPassiveModifier(0.00f, 1.0f, 0.00f, 10f, 1.00f, 1.00f, 0.75f);
        passiveBehavior.shoot.SetWeaponPassive( 1.5f, 1.5f, .50f, 1.00f);
    }

    public override void RemoveEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.player.SetHealthPassiveModifier(0.15f, 1.0f, 0.35f, 1.0f, 0.50f, 1.0f, 1.00f);
        passiveBehavior.shoot.SetWeaponPassive(1.0f, 1.0f, 1.0f, 1.00f);
    }
}
