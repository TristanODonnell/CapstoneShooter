using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	[SerializeField]
	private BoxCollider hb_collider;
	[SerializeField]
	public gricel.HealthSystem health;
	[SerializeField]
	public float damagePercentage = 1;
	private void OnValidate()
	{
		if (!health)
			health = GetComponentInParent<gricel.HealthSystem>();
		if (!hb_collider)
		{
			hb_collider = gameObject.GetComponent<BoxCollider>();
			if (hb_collider)
			{
				hb_collider.isTrigger = true;
				return;
			}
			var renderer = GetComponentInChildren<Renderer>(true);

			hb_collider = gameObject.AddComponent<BoxCollider>();
			hb_collider.size = renderer.transform.rotation * renderer.bounds.size;
			hb_collider.isTrigger = true;
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
