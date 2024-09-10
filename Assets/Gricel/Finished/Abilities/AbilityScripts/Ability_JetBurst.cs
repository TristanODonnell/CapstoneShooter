using UnityEngine;


namespace Abilities
{
	public class Ability_JetBurst : AbilityBase
	{
		[SerializeField] private int jet_burstsMax = 3;
		private int jet_bursts = 3;
		[SerializeField] private float jet_jumpPower = 1f;
		

		protected override bool CanBeUsed()
		{
			var bursts = jet_bursts > 0;
			return bursts;
		}

		protected override void UsePress()
		{
			jet_bursts--;
			gravitation.Jump(jet_jumpPower);
		}
		private void Start() => jet_bursts = jet_burstsMax;
		private void Update()
		{
			if(controller.isGrounded)
				jet_bursts = jet_burstsMax;
		}
	}
}