using Explosions;
using gricel;
using System;
using UnityEngine;
namespace throwables
{
	[RequireComponent(typeof(Rigidbody))]
	public abstract class ThrowableItem : MonoBehaviour
	{
		[SerializeField] protected Rigidbody throwable_rigidbody;
		[HideInInspector] public float powerIncrease;
		public Sprite icon;
		protected gricel.HealthSystem thrower;
		public throwables.ThrowableItem Throw(gricel.HealthSystem thrower, Vector3 position, Vector3 direction, float force, float powerIncrease = 1f)
		{
			var g = Instantiate(this);
			g.transform.position = position;
			g.transform.forward = direction;
			g.throwable_rigidbody.AddForce(force * direction);
			g.thrower = thrower;
			g.OnStart();
			g.powerIncrease = powerIncrease;
			return g;
		}

		public virtual void OnStart() { }
	}

}