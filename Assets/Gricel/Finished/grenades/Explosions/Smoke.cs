using gricel;
using System.Collections.Generic;
using UnityEngine;
namespace Explosions
{
	public class Smoke : MonoBehaviour
	{
		[SerializeField] private float duration;
		private float normalDuration;
		private float normalDurationMultiplier;
		[SerializeField] private float radius;
		private ProtectionValues stun = new(0, 0, 0, 1f);
		private List<HealthSystem> entities = new List<HealthSystem>();
		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent<HealthSystem>(out var e))
				entities.Add(e);
		}

		private void OnTriggerExit(Collider other)
		{
			var e = other.GetComponent<HealthSystem>();
			if(entities.Contains(e))
				entities.Remove(e);
		}

		private void Start()
		{
			normalDuration = 0;
			normalDurationMultiplier = 1f/duration;
		}

		private void Update()
		{
			foreach(var e in entities)
				if (e != null)
					e.HS_Damage(stun);

			normalDuration += Time.deltaTime * normalDurationMultiplier;
			var dur = normalDuration * 1.5f;
			dur *= dur;
			var sizeMod = Mathf.Lerp(0f, Mathf.Lerp(radius, 0f, (normalDuration - 1f) * 2f), dur);

			transform.localScale = Vector3.one * dur;

			if (normalDuration > 1f)
			{
				entities.Clear();
				Destroy(gameObject);
			}
		}
	}
}