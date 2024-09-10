using System;
using UnityEngine;
namespace gricel
{
	[RequireComponent(typeof(Rigidbody))]
	public class Grenade : MonoBehaviour
	{
		[SerializeField] private Rigidbody grenade_rigidbody;
		[SerializeField] private Countdown grenade_explosionTime = new(4);
		[SerializeField] private Grenade_Explosion grenade_Explosion;
		public gricel.Grenade Throw(Vector3 position, Vector3 direction, float force)
		{
			var g = Instantiate(this);
			g.transform.position = position;
			g.transform.forward = direction;
			g.grenade_rigidbody.AddForce(force * direction);

			return g;
		}


		public void Update()
		{
			if (grenade_explosionTime.CountdownReturn())
			{
				Instantiate(grenade_Explosion);
				Destroy(gameObject);
			}
		}

		internal void Throw(object position, object direction, float escape_grenadeThrowForce)
		{
			throw new NotImplementedException();
		}
	}
}