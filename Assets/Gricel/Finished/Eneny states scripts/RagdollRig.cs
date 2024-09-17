using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollRig : MonoBehaviour
{
	[SerializeField] private Rigidbody[] body;
	[SerializeField] private Countdown destruction = new(12f);
	[SerializeField] private Countdown_AutoReset destruction_Update = new (0.1f);
	private float multiplier = 1 / 12f;
	private void OnValidate()
	{
		var rbody = GetComponentsInChildren<Renderer>();
		foreach (var piece in rbody)
		{
			if (!piece.gameObject.GetComponent<Rigidbody>())
				piece.gameObject.AddComponent<Rigidbody>();
			if (!piece.gameObject.GetComponent<BoxCollider>())
				piece.gameObject.AddComponent<BoxCollider>();
		}
		body = GetComponentsInChildren<Rigidbody>();
	}

	public RagdollRig Spawn(Transform _transform, float violence = 10f)
	{
		var ragdoll = Instantiate(this);
		ragdoll.transform.position = _transform.position;
		ragdoll.transform.rotation = _transform.rotation;
		ragdoll.transform.transform.localScale = _transform.localScale;

		float RandomFloat() => Random.Range(-1f, 1f);
		Vector3 RandomVector() => new Vector3(RandomFloat(), RandomFloat(), RandomFloat());

		foreach (var piece in body)
			piece.AddForce(RandomVector() * Random.value * violence);

		multiplier = 1f- (1f / destruction.maximumCount) * destruction_Update.maximumCount;
		return ragdoll;
	}

	private void Update()
	{
		if (destruction_Update.CountdownReturn())
			foreach (var piece in body)
				piece.transform.localScale *= multiplier;

		if(destruction.CountdownReturn())
			Destroy(gameObject);
	}
}
