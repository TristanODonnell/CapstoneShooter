using gricel;
using UnityEngine;
namespace throwables
{

	namespace Items
	{
		public class Grenade : ThrowableItem
		{
			[SerializeField] private Countdown iddleTime = new(2f);
			[SerializeField] private Explosions.Explosion explosion;
			[SerializeField] private ProtectionValues explosion_damage = new(100, 100, 100, 2);
			[SerializeField] private float explosion_radius;
			[SerializeField] private float explosion_duration;

			private void Update()
			{
				if (iddleTime.CountdownReturn())
				{
					explosion.Explode(transform.position, explosion_damage * powerIncrease, explosion_radius, explosion_duration);
					Destroy(gameObject);
				}
			}
		}
	}
}