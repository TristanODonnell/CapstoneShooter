using UnityEngine;
namespace throwables
{

	namespace Items
	{
		public class SmokeGrenade : ThrowableItem
		{
			[SerializeField] private Countdown iddleTime = new(2f);
			[SerializeField] private Explosions.Smoke smoke;
			private void Update()
			{
				if (iddleTime.CountdownReturn())
				{
					var cloud = Instantiate(smoke);
					cloud.transform.position = transform.position;

					Destroy(gameObject);
				}
			}
		}
	}
}