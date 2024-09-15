using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Gizmos.color = gravity < 0? Color.red : Color.cyan;
		var sizeY = gravity < 0 ? -1f : 1f;
		var length = Vector3.up * transform.localScale.y * sizeY * 0.5f;
		Gizmos.DrawLine(transform.position + length, transform.position - length);
		Gizmos.DrawSphere(transform.position + length, gravity * 0.25f);
	}
	[SerializeField]
	private float gravity;
	private List<GravitationalBehaviour> inside = new();

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<GravitationalBehaviour>(out var gravitationalItem))
		{
			if(!inside.Contains(gravitationalItem))
				inside.Add(gravitationalItem);
			gravitationalItem.currentGravity = gravity;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent<GravitationalBehaviour>(out var gravitationalItem))
		{

			if (inside.Contains(gravitationalItem))
				inside.Remove(gravitationalItem);
			gravitationalItem.currentGravity = SceneGravity.GlobalGravity;
		}
	}
	private void OnDestroy()
	{
		foreach (var item in inside)
		{
			item.currentGravity = SceneGravity.GlobalGravity;
		}
	}
}
