using UnityEngine;


namespace Abilities
{
	public class Ability_Jetpack : AbilityBase
	{
		[SerializeField] private float flyTimeMax = 8f;
		private float flyTime;
		[SerializeField] private GravityZone gravity;
		private float buttonPressed = 0.2f;
		protected override bool CanBeUsed() => flyTime > 0f;

		protected override void UseHold()
		{
			buttonPressed = 0f;
			flyTime -= Time.deltaTime;
		}
		private void Update()
		{
			gravity.enabled = buttonPressed < 0.2f;
			if (!gravity.enabled && flyTime < flyTimeMax)
				flyTime += Time.deltaTime;
		}

	}
}