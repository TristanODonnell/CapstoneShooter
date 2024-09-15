using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
	[SerializeField] private float minutesToDestroy = 3.5f;
	protected bool playerIsInside { get; private set; }
	private void Start()
	{
		minutesToDestroy *= 60f;
	}
	private void Update()
	{
		if (minutesToDestroy < 0f)
			Destroy(gameObject);
		minutesToDestroy -= Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<PlayerController>())
			playerIsInside = true;
		try
		{
			if (TryPickUp(other))
				Destroy(gameObject);
		}
		catch { }
	}
	private void OnTriggerExit(Collider other)
	{

		if (other.GetComponent<PlayerController>())
			playerIsInside = false;
	}
	protected abstract bool TryPickUp(Collider other);
}
