using UnityEngine;


namespace Abilities
{
	public class Ability_Jetpack : AbilityBase
	{
		[SerializeField] private float flyTimeMax = 8f;
		private float flyTimeMaxOriginal = 8f;
		private float flyTime;
		[SerializeField] private GravityZone gravity;
		private float wasPressed = 0f;
		private bool hasLanded;

		public override void Ability_CooldownOverride(float multiplier)
		{
			if(multiplier != 0)
				flyTimeMax = flyTimeMaxOriginal * multiplier;
		}

		protected override bool CanBeUsed() => flyTime > 0f;
		private void Start()
		{
			flyTime = flyTimeMaxOriginal = flyTimeMax;
		}
		protected override void UseHold()
		{
			if (flyTime <= 0f) return;
			flyTime -= Time.deltaTime;
			wasPressed = Time.deltaTime * 2f;
			hasLanded = false;
		}
		private void Update()
		{
			gravity.transform.localPosition = AbiltyKeyPressed() && wasPressed > 0f ? Vector3.zero : Vector3.down * 1000f;

			if (controller.isGrounded) hasLanded = true;

			if (flyTime < flyTimeMax && hasLanded)
				flyTime += Time.deltaTime;

			if (wasPressed > 0f)
			{
				wasPressed -= Time.deltaTime;
				if (wasPressed <= 0f)
					gravitation.Jump(1f);
			}
		}

		public override float Ability_Normalized() => flyTime / flyTimeMaxOriginal;
	}
}