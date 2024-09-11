using gricel;
using UnityEngine;
namespace throwables
{

	namespace Items
	{
		public class StickyGrenade : ThrowableItem
		{

			[SerializeField] private ProtectionValues damage = new(60, 60, 60, 0);
			[SerializeField] private Countdown explosionOnLanding = new(3);
			[SerializeField] private Explosions.Explosion explosion;
			[SerializeField] private float explosionRadius;
			[SerializeField] private float explosionDuration;
			private bool hasLanded = false;

			private void OnValidate()
			{
				while (!throwable_rigidbody)
				{
					throwable_rigidbody = gameObject.GetComponent<Rigidbody>();
					if (!throwable_rigidbody) throwable_rigidbody = gameObject.AddComponent<Rigidbody>();
					break;
				}

				Collider col = gameObject.GetComponent<Collider>();
				if (!col) col = gameObject.AddComponent<BoxCollider>();
			}

			private void Update()
			{
				if (hasLanded)
					if (explosionOnLanding.CountdownReturn())
					{
						explosion.Explode(transform.position, damage * powerIncrease, explosionRadius, explosionDuration);
						Destroy(gameObject);
					}
			}

			private void OnTriggerEnter(Collider other)
			{
				if (other.GetComponent<CharacterController>() || hasLanded) return;
				if (other.TryGetComponent<Hitbox>(out var entity))
				{
					if (entity.health == thrower || entity.health == null) return;
					entity.Damage(damage);
				}
				transform.parent = other.transform;
				throwable_rigidbody.isKinematic = true;
				hasLanded = true;
			}
		}
	}
}