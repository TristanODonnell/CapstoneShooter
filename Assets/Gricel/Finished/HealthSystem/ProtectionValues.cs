namespace gricel
{
	[System.Serializable]
    public struct ProtectionValues
    {
        public float unprotected;
        public float energyShield;
        public float armor;
        public float stun;


        public ProtectionValues(float shrapnel, float energy, float heavy, float stun)
        {
            this.unprotected = shrapnel;
            this.energyShield = energy;
            this.armor = heavy;
            this.stun = stun;
        }


        public ProtectionValues Absolute()
        {
            void Absoluted(ref float value)
            {
                if (value < 0f)
                    value = -value;
            }
            var a = this;

            Absoluted(ref a.unprotected);
            Absoluted(ref a.energyShield);
            Absoluted(ref a.armor);
            Absoluted(ref a.stun);
            return a;
        }
        public static ProtectionValues operator +(ProtectionValues a) => a;
        public static ProtectionValues operator -(ProtectionValues a) => (a - a) - a;

        public static ProtectionValues operator -(ProtectionValues l, ProtectionValues r)
        {
            l.unprotected -= r.unprotected;
            l.energyShield -= r.energyShield;
            l.armor -= r.armor;
            l.stun -= r.stun;
            return l;
        }
        public static ProtectionValues operator +(ProtectionValues l, ProtectionValues r)
        {
            return l - (-r);
        }


        public static ProtectionValues operator *(ProtectionValues a, float mult)
        {
            var b = a;
            b.unprotected *= mult;
            b.energyShield *= mult;
            b.armor *= mult;
            b.stun *= mult;
            return b;
        }

        public static ProtectionValues operator /(ProtectionValues a, float div)
        {
            var d = 1f / div;
            return a * d;
        }
    }

}