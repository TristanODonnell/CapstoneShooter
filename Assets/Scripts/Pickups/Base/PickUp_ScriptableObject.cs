using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickUp_ScriptableObject : ScriptableObject
{
	[SerializeField] private Pickup[] items; 
	public Pickup Pickup_Spawn(Vector3 position)
	{
		try
		{
			var item = Instantiate(items[Random.Range(0, items.Length)]);
			item.transform.position = position;
			item.transform.transform.rotation = Random.rotation;
			return item;
		}
		catch { }

		return null;
	}
}
