using UnityEngine;


namespace Abilities
{
	public class Ability_Dash : AbilityBase
	{
		[SerializeField] private byte dash_chargesMax;
		private byte dash_charges;
		[SerializeField] private Countdown_AutoReset dash_cooldown = new(0.6f);
		[SerializeField] private float dash_speed = 12f;
		private Vector3 dash_Movement;
		private float dash_NormalMovement;

		bool isCurrentlyDashing => dash_NormalMovement > 0f;
		bool hasCharges => dash_charges > 0;

		protected override bool CanBeUsed() => !isCurrentlyDashing && hasCharges;

		protected override void UseHold()
		{
			var dir = Vector3.zero ;
			dir.x = Input.GetAxisRaw("Horizontal");
			dir.z = Input.GetAxisRaw("Vertical");

			dash_Movement = Vector3.zero;



			if (Mathf.Abs(dir.z) > Mathf.Abs(dir.x))
				dir.x = 0;
			else
				dir.z = 0;

			if (dir.magnitude == 0f)
				dir.z = 1f;

			dash_Movement = dir.normalized;

			dash_NormalMovement = 1f;
		}

		private void Update()
		{
			if (!isCurrentlyDashing && controller.isGrounded && dash_charges < dash_chargesMax)
				if (dash_cooldown.CountdownReturn())
					dash_charges++;
			if (isCurrentlyDashing)
				Dash();
		}

		private void Dash()
		{
			var velocity = dash_Movement;
			for (int i = 0; i < 2; ++i) 
				velocity *= dash_NormalMovement;
			velocity *= dash_speed;
			velocity *= Time.deltaTime;

			controller.Move(velocity);

			dash_NormalMovement -= Time.deltaTime;

			gravitation.Jump(0f);
		}
	}
}