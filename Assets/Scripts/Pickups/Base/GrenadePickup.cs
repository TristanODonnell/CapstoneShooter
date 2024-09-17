using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickup : Pickup
{
	[SerializeField] private throwables.ThrowableItem prefab;

	protected override bool TryPickUp(Collider other)
	{
		if (other.GetComponent<PlayerController>() != null)
			try
			{
				return FindObjectOfType<Player_GrenadeThrow>().TryAddGrenade(prefab);
			}
			catch { }
		return false;
	}
}
