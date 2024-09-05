using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_TurretNothing : Enemy_State
{
	[SerializeField] private Transform turret_Head;
	[SerializeField] private float turret_rotationPerSecond;
	[SerializeField] private Countdown_AutoReset turret_RotationDelay = new(1f/12f);
	[SerializeField] private Countdown turret_RotationChange = new(8f);
	private Quaternion turret_targetRotation;

	private float turret_velocity => turret_rotationPerSecond * turret_RotationDelay.maximumCount;
	public override void ActionStart()
	{
		Turret_RandomizeHead();
	}

	public override void ActionUpdate()
	{
		if (turret_RotationDelay.CountdownReturn())
			turret_Head.rotation = Quaternion.RotateTowards(turret_Head.rotation, turret_targetRotation, turret_velocity);

		if (turret_RotationChange.CountdownReturn())
		{
			Turret_RandomizeHead();
		}

	}

	private void Turret_RandomizeHead()
	{
		turret_targetRotation = Quaternion.Euler(0f, Random.value * 360f, 0f);

		turret_RotationChange.CountdownResetRandomized(0.4f);
	}
}
