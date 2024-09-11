using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	[SerializeField]
	private BoxCollider collider;
	[SerializeField]
	public gricel.HealthSystem health;
	[SerializeField]
	public float damagePercentage = 1;
	private void OnValidate()
	{
		if (!health)
			health = GetComponentInParent<gricel.HealthSystem>();
		if (!collider)
		{
			collider = gameObject.GetComponent<BoxCollider>();
			if (collider)
			{
				collider.isTrigger = true;
				return;
			}
			var renderer = GetComponentInChildren<Renderer>(true);

			collider = gameObject.AddComponent<BoxCollider>();
			collider.size = renderer.transform.rotation * renderer.bounds.size;
			collider.isTrigger = true;
		}
	}

	public void Damage(gricel.ProtectionValues damage)
	{
		health.HS_Damage(damage * damagePercentage);
	}
	public void Damage(float damage)
	{
		health.HS_Damage_Indiscriminately(damage * damagePercentage);
	}
}
