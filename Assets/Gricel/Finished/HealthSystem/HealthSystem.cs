using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gricel
{
    [SelectionBase]
	public class HealthSystem : MonoBehaviour
    {
        [System.Serializable]
        public class Health
        {
            public enum HealthT
            {
                unprotected,
                energyShield,
                armor
            }
            public HealthT protection;

            public float health_Max = 40f;
            public float health { get; private set; }

            public float healthRegenPerS = 11f;

            [SerializeField]
            private UnityEngine.Events.UnityEvent OnDepleted;

            public bool H_IsDamaged() => health < health_Max;
            public bool H_IsDepleted() => health <= 0f;
            public void H_Start()
            {
                health = health_Max;
            }
            public void H_Regenerate(float deltaTime)
            {
                if (!H_IsDamaged())
                    return;
                health = Mathf.Clamp(health + healthRegenPerS * deltaTime, 0f, health_Max);
            }

            public void H_ModifyHealth(ref float addSub, HealthT _protection)
            {
                if (_protection != protection || addSub == 0f || (addSub < 0f && H_IsDepleted()))
                    return;
                health = Mathf.Clamp(health + addSub, 0f, health_Max);
                if (H_IsDepleted())
                    OnDepleted.Invoke();
                addSub = 0f;
            }
        }

        [SerializeField]
        public Health[] healthBars = new Health[1];

        public float regenDelay_Max = 6f;
        private float regenDelay;
        private byte regen_Index = 0;

        [SerializeField]
        private float stunMax = 0.8f;
        [SerializeField]
        [Range(0, 1f)]
        private float stunWeakness = 1f;


        [SerializeField]
        private UnityEngine.Events.UnityEvent OnRegeneration;
		[SerializeField]
		public UnityEngine.Events.UnityEvent onDeath;
		[SerializeField]
		public UnityEngine.Events.UnityEvent onDamaged;

		public float stun { get; private set; }
        public void HS_Damage(ProtectionValues damage)
        {
            bool DoExit()
            {
                var unp = damage.unprotected == 0f;
                var enS = damage.energyShield == 0f;
                var amr = damage.armor == 0f;
                return unp && enS && amr;
            }

            if (DoExit())
                return;

            regenDelay = regenDelay_Max;
            damage = -damage.Absolute();

            stun = Mathf.Clamp(stun + damage.stun * stunWeakness, 0f, stunMax);

			for (var i = healthBars.Length - 1; i >= 0; i--)
            {
				Health h = healthBars[i];
				if (h.H_IsDepleted())
                    continue;

                h.H_ModifyHealth(ref damage.unprotected, Health.HealthT.unprotected);
                h.H_ModifyHealth(ref damage.energyShield, Health.HealthT.energyShield);
                h.H_ModifyHealth(ref damage.armor, Health.HealthT.armor);
                if (DoExit())
                    break;
            }
            if (healthBars[0].H_IsDepleted())
                onDeath.Invoke();
        }
        public void HS_Damage_Indiscriminately(float damage)
        {
            if (damage > 0)
                damage = -damage;
            if(damage < 0)
                regenDelay = regenDelay_Max;

            for (var i = healthBars.Length - 1; i >= 0; i--)
			{
                if (damage >= 0)
                    return;
				Health h = healthBars[i];
                var prDamage = damage;
                var prHealth = h.health;
                h.H_ModifyHealth(ref damage, Health.HealthT.unprotected);
                h.H_ModifyHealth(ref damage, Health.HealthT.energyShield);
                h.H_ModifyHealth(ref damage, Health.HealthT.armor);
                var crHealth = h.health;

                damage = prDamage + (prHealth - crHealth);
            }
        }


        public void HS_Heal(ProtectionValues regenerate)
        {

            bool DoExit()
            {
                var unp = regenerate.unprotected == 0f;
                var enS = regenerate.energyShield == 0f;
                var amr = regenerate.armor == 0f;
                return unp && enS && amr;
            }

            if (DoExit())
                return;
            stun = 0;
            regenDelay = 0f;
            regenerate = regenerate.Absolute();

            foreach (var h in healthBars)
            {
                if (!h.H_IsDamaged())
                    continue;
                h.H_ModifyHealth(ref regenerate.unprotected, Health.HealthT.unprotected);
                h.H_ModifyHealth(ref regenerate.energyShield, Health.HealthT.energyShield);
                h.H_ModifyHealth(ref regenerate.armor, Health.HealthT.armor);
                if (DoExit())
                    return;
            }
        }
        public void HS_Heal_Indiscriminately(float heal)
        {
            if (heal < 0)
                heal = -heal;
            stun = 0;
            for (var i = healthBars.Length - 1; i >= 0; i--)
            {
                if (heal <= 0)
                    return;
                Health h = healthBars[i];
                var prHeal = heal;
                var prHealth = h.health;
                h.H_ModifyHealth(ref heal, Health.HealthT.unprotected);
                h.H_ModifyHealth(ref heal, Health.HealthT.energyShield);
                h.H_ModifyHealth(ref heal, Health.HealthT.armor);
                var crHealth = h.health;

                heal = prHeal - (prHealth - crHealth);
            }
        }

        public void HS_Regenerate()
        {
            if (regen_Index >= healthBars.Length)
                regen_Index = 0;
            var hb = healthBars[regen_Index];
            ++regen_Index;
            if (!hb.H_IsDamaged())
                return;

            hb.H_Regenerate(Time.deltaTime * (float)healthBars.Length);
            if(regenDelay != 0f)
                OnRegeneration.Invoke();

            regenDelay = 0f;
        }
        public bool isStunned => stun > 0f;

        void Start()
        {
            foreach (var h in healthBars)
                h.H_Start();
        }

        void Update()
        {
            if (stun > 0)
                stun -= Time.deltaTime;


            if (regenDelay > 0f)
                regenDelay -= Time.deltaTime;
            else
                HS_Regenerate();
        }
    }

}