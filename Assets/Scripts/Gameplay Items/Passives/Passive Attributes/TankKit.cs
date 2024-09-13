using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;

public class TankKit : PassiveAttribute
{
    //makes the armour health bar now be worth 30% of total health while
    //flesh health is now worth 10%,
    //and decreases speed to 90%
    public TankKit(PassiveData passiveData) : base(passiveData) { }

    public override void ApplyEffects(PassiveBehavior passiveBehavior)
    {

        passiveBehavior.movement.SetPassiveModifier(0.90f);
        passiveBehavior.player.SetHealthPassiveModifier(0.30f, 1.0f, 0.10f, 1.0f, 0.60f, 1.00f, 1.00f); 
        
    }

    public override void RemoveEffects(PassiveBehavior passiveBehavior)
    {
        passiveBehavior.movement.SetPassiveModifier(1.0f);
        passiveBehavior.player.SetHealthPassiveModifier(0.15f, 1.0f, 0.35f, 1.0f, 0.50f, 1.0f, 1.00f);
    }
}
