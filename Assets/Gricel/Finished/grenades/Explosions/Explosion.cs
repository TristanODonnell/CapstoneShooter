using gricel;
using System;
using UnityEngine;
namespace Explosions
{
	public class Explosion : MonoBehaviour
	{
		private ProtectionValues damage;
		private float radius, normalDurationMultiplier, normalDuration;
		private bool isDisapearing;
		public Explosion Explode(Vector3 pos, ProtectionValues damage, float radius, float duration)
		{
			var exp = Instantiate(gameObject).GetComponent<Explosion>();
			exp.transform.position = pos;
			exp.damage = damage;

			exp.radius = radius;
			if (duration != 0)
				exp.normalDurationMultiplier = 1f / duration;
			else
				exp.normalDurationMultiplier = 1f;
			normalDuration = 0f;
			isDisapearing = false;
            return exp;
		}

		private void Update()
		{
			transform.localScale = Vector3.one * normalDuration * radius * 0.5f;
			if (!isDisapearing)
				Blow();
			else
				Dissapear();
		}
		private void Blow()
		{
			normalDuration += Time.deltaTime * normalDurationMultiplier *4f;
			isDisapearing = normalDuration > 1;
		}

		private void Dissapear()
		{
			normalDuration -= Time.deltaTime * normalDurationMultiplier * 3f;
			if (normalDuration < 0)
				Destroy(gameObject);
		}


		private void OnTriggerEnter(Collider other)
		{
			if (normalDuration > 1f) return;
			if (other.TryGetComponent<HealthSystem>(out var dam))
				dam.HS_Damage(damage);
		}
	}
}