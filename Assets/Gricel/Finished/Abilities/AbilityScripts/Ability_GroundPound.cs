using gricel;
using UnityEngine;


namespace Abilities
{
	public class Ability_GroundPound : AbilityBase
	{
		private Countdown timeOnAir = new(0.4f);
		private bool isGroundPounding;
		[SerializeField] private Countdown cooldown;
		float cooldown_OriginalTime;
		[SerializeField] private ProtectionValues damagePerFallTime;
		[SerializeField] private float radiusPerFallTime;
		float damageMultiplier;
		protected override bool CanBeUsed() => timeOnAir.mReviseCountdownIsOver && cooldown.mReviseCountdownIsOver;
		protected override void UsePress()
		{
			isGroundPounding = true;
			damageMultiplier = 0f;
			cooldown.Countdown_Restart();
		}
		private void Update()
		{
			if (!isGroundPounding)
			{
				cooldown.CountdownReturn();
				if (controller.isGrounded)
					timeOnAir.Countdown_Restart();
				else
					timeOnAir.CountdownReturn();
			}
			else
				GroundpointUpdate();
		}

		private void GroundpointUpdate()
		{
			if (controller.isGrounded)
				GP_Punch();
			else
				GP_Fall();
		}

		private void GP_Punch()
		{
			var enemies = FindObjectsOfType<gricel.Enemy>(false);
			gravitation.Jump(0.4f);
			foreach (var e in enemies)
			{
				var distance = transform.position - e.transform.position;
				if (distance.magnitude < radiusPerFallTime * Mathf.Clamp(damageMultiplier, 0f, 3f))
				{
					e.health.HS_Damage(damagePerFallTime * damageMultiplier);
					e.GetComponent<GravitationalBehaviour>().Jump(0.4f);
				}
			}
			isGroundPounding = false;

		}

		private void GP_Fall()
		{
			gravitation.Jump(0f);
			controller.Move(Vector3.down * Time.deltaTime * damageMultiplier * damageMultiplier);

			damageMultiplier += Time.deltaTime *gravitation.currentGravity *2f;
		}

		private void Start() => cooldown_OriginalTime = cooldown.maximumCount;
		public override void Ability_CooldownOverride(float multiplier)
		{
			if (multiplier == 0f)
				return;
			cooldown.maximumCount = cooldown_OriginalTime / multiplier;
			cooldown.Countdown_ForceSeconds(0);
		}

		public override float Ability_Normalized() => cooldown.normalized;
	}
}