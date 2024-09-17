using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollRig : MonoBehaviour
{
	[SerializeField] private Countdown destruction = new(12f);
	private void Start()
	{
		var rbody = GetComponentsInChildren<Renderer>();
		foreach (var piece in rbody)
		{
			if (!piece.gameObject.GetComponent<Rigidbody>())
				piece.gameObject.AddComponent<Rigidbody>();
			if (!piece.gameObject.GetComponent<BoxCollider>())
				piece.gameObject.AddComponent<BoxCollider>();
		}

	}

	public RagdollRig Spawn(Transform _transform)
	{
		var ragdoll = Instantiate(this);
		ragdoll.transform.position = _transform.position;
		ragdoll.transform.rotation = _transform.rotation;
		ragdoll.transform.transform.localScale = _transform.localScale;

		return ragdoll.GetComponent<RagdollRig>();
	}

	private void Update()
	{
		if(destruction.CountdownReturn())
			Destroy(gameObject);
	}
}
