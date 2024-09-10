using UnityEngine;


namespace Abilities
{
	public class Ability_Teleporter : AbilityBase
	{
		[SerializeField] private Countdown cooldown = new(30f);
		[SerializeField] private float radiusOfTeleportableArea = 2f;
		[SerializeField] private float maxDistance = 30f;
		protected override bool CanBeUsed()
		{
			return cooldown.mReviseCountdownIsOver;
		}
		private void Start() => cooldown.Countdown_ForceSeconds(-1f);
		private void Update() => cooldown.CountdownReturn();

		protected override void UsePress()
		{

			if (LookDirectionRaycast(out var point))
			{
				var distance = point - controller.transform.position;
				controller.Move(distance);
				cooldown.Countdown_Restart();
			}
		}

		private bool LookDirectionRaycast(out Vector3 point)
		{
			var camera = Camera.main.transform;

			var pos = camera.position;
			var dir = camera.forward;
			var ray = new Ray(pos, dir);
			var raycast = Physics.RaycastAll(ray, maxDistance);
			point = Vector3.zero;
			foreach (var r in raycast)
			{
				if (r.collider.GetComponentInParent<PlayerController>())
					continue;

				point = r.point;
				return true;
			}
			return false;
		}
	}
}