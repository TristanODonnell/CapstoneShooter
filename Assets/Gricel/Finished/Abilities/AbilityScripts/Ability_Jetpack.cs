using UnityEngine;


namespace Abilities
{
	public class Ability_Jetpack : AbilityBase
	{
		[SerializeField] private float flyTimeMax = 8f;
		private float flyTimeMaxOriginal = 8f;
		private float flyTime;
		[SerializeField] private GravityZone gravity;
		private float buttonPressed = 0.2f;

		public override void Ability_CooldownOverride(float multiplier)
		{
			if(multiplier != 0)
				flyTimeMax = flyTimeMaxOriginal * multiplier;
		}

		protected override bool CanBeUsed() => flyTime > 0f;
		private void Start()
		{
			flyTimeMaxOriginal = flyTimeMax;
			gravity.enabled = false;
		}
		protected override void UseHold()
		{
			buttonPressed = Mathf.Clamp(buttonPressed + Time.deltaTime, 0f, 1f);
			flyTime -= Time.deltaTime;
		}
		private void Update()
		{
			gravity.enabled = buttonPressed > 0.1f;
			buttonPressed -= Time.deltaTime * 0.5f;
			if (!gravity.enabled && flyTime < flyTimeMax && controller.isGrounded)
				flyTime += Time.deltaTime;
		}

	}
}