using System.Collections.Generic;
using UnityEngine;
namespace gricel
{
	[RequireComponent(typeof(SphereCollider))]
	public class Grenade_Explosion : MonoBehaviour
	{
		[SerializeField] private Countdown exp_Duration;
		private List<HealthSystem> exp_entityList = new();
		[SerializeField] private Rigidbody exp_rigidbody;

		[SerializeField] private float exp_SizeAtStart = 1f;
		[SerializeField] private float exp_SizeAtEnd = 2f;

		[SerializeField] private ProtectionValues exp_damageOnce;
		[SerializeField] private ProtectionValues exp_damagePerSecond;

		private void OnValidate()
		{
			if (!exp_rigidbody)
			{
				exp_rigidbody = GetComponent<Rigidbody>();
				if(!exp_rigidbody)
					exp_rigidbody = gameObject.AddComponent<Rigidbody>();
			}
			exp_rigidbody.freezeRotation = false;
			exp_rigidbody.useGravity = false;


			var sphere = GetComponent<SphereCollider>();
			sphere.isTrigger = true;


			if (exp_SizeAtStart < 0f)
				exp_SizeAtStart = 0f;
			transform.localScale = Vector3.one * exp_SizeAtStart;
			if (exp_SizeAtEnd < 0f)
				exp_SizeAtEnd = 0f;

		}


		private void FixedUpdate()
		{
			foreach (var i in exp_entityList)
				i.HS_Damage(exp_damagePerSecond);


			var normal = exp_Duration.normalized;
			var lrp = Mathf.Lerp(exp_SizeAtStart, exp_SizeAtEnd, normal);

			transform.localScale = lrp * Vector3.one;

			if (exp_Duration.CountdownReturn())
			{
				foreach (var i in exp_entityList)
					i.HS_Damage(exp_damageOnce);
				Destroy(gameObject);
			}
		}


		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<HealthSystem>(out var hs))
				if(!exp_entityList.Contains(hs))
					exp_entityList.Add(hs);
		}
		private void OnTriggerExit(Collider other)
		{
			foreach (var i in exp_entityList)
				if (i.gameObject == other.gameObject)
					exp_entityList.Remove(i);
        }
	}
}