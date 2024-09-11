using gricel;
using System;
using UnityEngine;
namespace throwables
{

	namespace Items
	{
		public class ThrowingKnife : ThrowableItem
		{
			[SerializeField] private ProtectionValues damage = new(60, 60, 60, 0);
			[SerializeField][Range(1, 12)] private float destructionTime = 6f;
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
				if(!col) col = gameObject.AddComponent<BoxCollider>();
			}

			public override void OnStart()
			{
				Destroy(gameObject, destructionTime);
			}

			private void OnTriggerEnter(Collider other)
			{
				if (other.GetComponent<CharacterController>() || hasLanded) return;
				if(other.TryGetComponent<Hitbox>(out var entity))
				{
					if (entity.health == thrower || entity.health == null) return;
					entity.Damage(damage * powerIncrease);
				}
				transform.parent = other.transform;
				throwable_rigidbody.isKinematic = true;
				hasLanded = true;
			}
		}
	}
}