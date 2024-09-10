using UnityEngine;


namespace Abilities
{
	public class Ability_ExtraBatery : AbilityBase
	{
		[SerializeField] private Countdown cooldown;
		protected override bool CanBeUsed() => cooldown.mReviseCountdownIsOver;

		protected override void UsePress()
		{
			controller.GetComponentInParent<gricel.HealthSystem>(true).HS_Regenerate();
			cooldown.Countdown_Restart();
		}
		private void Update() => cooldown.CountdownReturn();

	}
}