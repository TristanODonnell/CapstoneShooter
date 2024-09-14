using UnityEngine;


namespace Abilities
{
	public class Ability_ExtraBatery : AbilityBase
	{
		[SerializeField] private Countdown cooldown;
		private float cooldownOg;

		private void Start() => cooldownOg = cooldown.maximumCount;
		public override void Ability_CooldownOverride(float multiplier = 1)
		{
			if (multiplier == 0) return;
			cooldown.maximumCount = cooldownOg / multiplier;
			cooldown.Countdown_ForceSeconds(0);
		}

		protected override bool CanBeUsed() => cooldown.mReviseCountdownIsOver;

		protected override void UsePress()
		{
			controller.GetComponentInParent<gricel.HealthSystem>(true).HS_Regenerate();
			cooldown.Countdown_Restart();
		}
		private void Update() => cooldown.CountdownReturn();

		public override float Ability_Normalized() => cooldown.normalized;
	}
}