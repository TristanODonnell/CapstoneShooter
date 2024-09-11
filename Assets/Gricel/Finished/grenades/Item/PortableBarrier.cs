using UnityEngine;
namespace throwables
{

	namespace Items
	{
		public class PortableBarrier : ThrowableItem
		{
			[SerializeField] private float openSeq_duration = 0.5f;
			private float openSeq_Normalduration;
			private float openSeq_NormaldurationMult;
			[SerializeField] private Countdown destroyTime = new(30f);

			private void Start()
			{
				var rot = transform.rotation;
				rot.x = 0;
				transform.rotation = rot;
				transform.localScale = Vector3.one * 0.001f;
				openSeq_NormaldurationMult = 1f / openSeq_duration;
			}


			private void Update()
			{
				if(openSeq_Normalduration < 1f)
				{
					openSeq_Normalduration += Time.deltaTime * openSeq_NormaldurationMult;
					transform.localScale = Vector3.one * openSeq_duration;
				}
				else
				{
					if(destroyTime.CountdownReturn())
						Destroy(gameObject);
				}
			}
		}
	}
}