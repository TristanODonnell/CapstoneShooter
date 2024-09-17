using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_TurretShootInPlace : Enemy_State
{
	[SerializeField] private Transform turret_Head;
	[SerializeField] private float turret_RotationPerSecond;
	[SerializeField] private Countdown_AutoReset turret_rotationDelay = new(0.1f);
	private float rotationPerSecond => turret_RotationPerSecond * turret_rotationDelay.maximumCount;

	[SerializeField] private Countdown_AutoReset gunfire_hold = new(0.5f);
	[SerializeField] private Countdown gunfire_release = new(3f);
	[SerializeField] private Countdown_AutoReset gunfire_CancelDetection = new(6);
	public override void ActionStart()
	{
	}

	public override void ActionUpdate()
	{

		id.DetectPlayer_CancelDetection();
		if (gunfire_CancelDetection.CountdownReturn())
			SetAction_Next();

		if (turret_rotationDelay.CountdownReturn())
			Turret_LookAtPlayer();
		if(gunfire_release.CountdownReturn())
			Turret_Fire();
	}

	private void Turret_Fire()
	{

		if (gunfire_hold.CountdownReturn())
		{
			gunfire_release.CountdownResetRandomized(0.4f);
			id.gun.Shoot();
		}
	}

	private void Turret_LookAtPlayer()
	{
		var forward = transform.rotation;
		forward.eulerAngles = Quaternion.LookRotation(Player_Detection.position - turret_Head.position).eulerAngles;
		turret_Head.rotation = Quaternion.RotateTowards(turret_Head.rotation, forward, rotationPerSecond);
	}
}
