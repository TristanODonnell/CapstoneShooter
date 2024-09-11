using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : MonoBehaviour
{
    //speed 
    [System.Serializable]
    public struct Speed
    {
        public float baseSpeed;
        public float multiplier;
        public Speed(float baseSpeed)
        {
            this.baseSpeed = baseSpeed;
            this.multiplier = 1f;
        }

        public static Speed operator *(Speed speed, float multiplier)
        {
            return new Speed(speed.baseSpeed * multiplier);
        }

        public static Speed operator /(Speed speed, float divisor)
        {
            return new Speed(speed.baseSpeed / divisor);
        }

        public static implicit operator float(Speed speed)
        {
            return speed.baseSpeed;
        }
    }

    //ammo, magazine, weapon, weapon damage, etc


    //health ---- protection values



    //cooldown for ability use

    //jump
    //speed
    //xp gain
}
