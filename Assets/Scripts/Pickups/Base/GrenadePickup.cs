using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickup : MonoBehaviour
{
	[SerializeField] private throwables.ThrowableItem prefab;
	[SerializeField] private float minutesToDestroy = 3.5f;

	private void Start()
	{
		minutesToDestroy *= 60f;
	}
	private void Update()
	{
		if (minutesToDestroy < 0f)
			Destroy(gameObject);
		minutesToDestroy-= Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent<PlayerController>(out var controller))
			try
			{
				if(controller.grenadeManager.AddGrenade(prefab))
					Destroy(gameObject);
			}
			catch { }
	}
}
