using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickup : Pickup
{
	[SerializeField] private throwables.ThrowableItem prefab;

	protected override bool TryPickUp(Collider other)
	{
		return other.GetComponent<GrenadeManager>().AddGrenade(prefab);
	}
}
