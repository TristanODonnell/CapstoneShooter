using gricel;
using System;
using UnityEngine;
namespace Explosions
{
	public class Explosion : MonoBehaviour
	{
		private ProtectionValues damage;
		private float radius, normalDurationMultiplier, normalDuration, dissapearence;
		public Explosion Explode(Vector3 pos, ProtectionValues damage, float radius, float duration)
		{
			var exp = Instantiate(this);
			exp.transform.position = pos;
			exp.damage = damage;

			exp.radius = radius;
			if (duration != 0)
				exp.normalDurationMultiplier = 1f / duration;
			else
				exp.normalDurationMultiplier = 1f;
			normalDuration = 0f;
			dissapearence = 1f;
            return exp;
		}

		private void Update()
		{
			if (normalDuration < 1f)
				Blow();
			else
				Dissapear();
		}
		private void Blow()
		{
			normalDuration += Time.deltaTime * normalDurationMultiplier;
			transform.localScale = Vector3.one * normalDuration * 0.5f;
		}

		private void Dissapear()
		{
			dissapearence -= Time.deltaTime * normalDurationMultiplier;
			transform.localScale = Vector3.one * dissapearence * radius*0.5f;
		}


		private void OnTriggerEnter(Collider other)
		{
			if (normalDuration > 1f) return;
			if (other.TryGetComponent<HealthSystem>(out var dam))
				dam.HS_Damage(damage);
		}
	}
}