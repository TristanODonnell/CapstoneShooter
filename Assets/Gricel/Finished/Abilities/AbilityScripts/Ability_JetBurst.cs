using UnityEngine;


namespace Abilities
{
	public class Ability_JetBurst : AbilityBase
	{
		[SerializeField] private int jet_burstsMax = 3;
		private int jet_burstsMaxOg = 3;
		private int jet_bursts = 3;
		[SerializeField] private float jet_jumpPower = 1f;
		public override void Ability_CooldownOverride(float multiplier)
		{
			jet_burstsMax = Mathf.RoundToInt((float)jet_burstsMaxOg * multiplier);
		}

		protected override bool CanBeUsed()
		{
			var bursts = jet_bursts > 0;
			return bursts && !controller.isGrounded;
		}

		protected override void UsePress()
		{
			jet_bursts--;
			gravitation.Jump(jet_jumpPower);
		}
		private void Start() => jet_burstsMaxOg = jet_bursts = jet_burstsMax;
		private void Update()
		{
			if(controller.isGrounded)
				jet_bursts = jet_burstsMax;
		}
	}
}