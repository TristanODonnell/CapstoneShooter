using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickup : Pickup
{
	[SerializeField] private throwables.ThrowableItem prefab;

	protected override bool TryPickUp(Collider other)
	{
		if (other.GetComponent<PlayerController>() != null)
		{
			try
			{
				FindObjectOfType<GrenadeManager>().AddGrenade(prefab);
				return true;
			}
			catch { }
		}
		return false;
	}
}
