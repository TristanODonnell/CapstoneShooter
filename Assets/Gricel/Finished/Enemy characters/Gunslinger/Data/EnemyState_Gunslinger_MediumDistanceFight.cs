using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Gunslinger_MediumDistanceFight : Enemy_State
{
	[SerializeField] private float fight_DangerZone;
	[SerializeField] private Countdown_AutoReset fight_checkPlayer = new (66f);

	[SerializeField] private float escape_distanceToNode = 1f;
	[SerializeField] Countdown escape_throwGrenadeTime = new(3f);
	[SerializeField][Range(0, 5)] private byte escape_GrenadesLeft = 5;
	[SerializeField] private throwables.ThrowableItem escape_grenade;
	[SerializeField] private Transform escape_grenadePivot;
	[SerializeField] private float escape_grenadeThrowForce;

	[SerializeField] private Countdown fight_holdTrigger = new(1f);
	[SerializeField] private Countdown fight_releaseTrigger = new(5f);

	private PathNode path_current;
	private Vector3 path_target;
	[SerializeField] private Countdown_AutoReset path_lookForEscape = new(3);
	private bool path_hasFoundEscape;
	private bool path_mustJump;

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, fight_DangerZone);
	}

	private bool Danger_PlayerIsTooClose()
	{
		return (playerPosition - transform.position).sqrMagnitude < fight_DangerZone * fight_DangerZone || path_hasFoundEscape;
	}
	public override void ActionStart()
	{
		path_current = PathFinderManager.GetClosestPathNode(transform.position);
		path_target = transform.position;
	}

	public override void ActionUpdate()
	{
		if (Danger_PlayerIsTooClose())
			Danger_EmergencyActions();
		else
			Danger_ShootInPlace();

		if (fight_checkPlayer.CountdownReturn())
			if (id.TryToDetectPlayer(true))
			{
				SetAction_Next();
				id.movement_specialAnimation = true;
			}
	}

	private void Danger_ShootInPlace()
	{
		Fight_KeepEyesLookingAtPlayer();
		if (!fight_releaseTrigger.CountdownReturn())
			return;
		if (!fight_holdTrigger.CountdownReturn())
		{
			id.ShootWeapon();
			return;
		}
		fight_holdTrigger.CountdownResetRandomized(0.4f);
		fight_releaseTrigger.CountdownResetRandomized(0.6f);


		var direction = playerPosition - transform.position;
		direction.y = 0;
		transform.forward = direction;
	}


	private void Fight_KeepEyesLookingAtPlayer()
	{
		var toPlayer = Player_Detection.position - transform.position;
		var multiplication = Quaternion.FromToRotation(transform.forward, toPlayer);

		var toVision = Vector3.zero;
		toVision.Set(multiplication.y * 2f, multiplication.x, 0f);
		id.movement_vision = toVision;
		id.movement_specialAnimation = false;
		id.movement_walk = 0f;
	}


	private void Danger_EmergencyActions()
	{
		id.movement_specialAnimation = true;

		if (path_lookForEscape.CountdownReturn() && !path_hasFoundEscape)
			Emergency_FindEscapeRoute();

		if (path_hasFoundEscape)
			Emergency_FollowEscapeRoute();
		else
		{
			id.movement_walk = 0;
			if (escape_GrenadesLeft > 0)
			{
				if (escape_throwGrenadeTime.CountdownReturn())
					Emergency_ThrowGrenades();
			}
			else
				Emergency_BegForMercy();
		}
	}


	private void Emergency_FindEscapeRoute()
	{
		if(!path_current)
			ActionStart();
		var l = path_current.AllSurroundingPaths();
		l.Add(path_current);

		float randomVector() => UnityEngine.Random.Range(-1f, 1f);
		var randomPosition = Vector3.zero;
		randomPosition.x = randomVector();
		randomPosition.z = randomVector();

		path_hasFoundEscape = false;
		while (l.Count > 0)
		{
			var i = l[UnityEngine.Random.Range(0, l.Count)];

			var position = i.position + randomPosition * i.radius;
			var distance = (playerPosition - i.position).sqrMagnitude;
			var distanceMax = fight_DangerZone * fight_DangerZone;

			if(distance > distanceMax)
			{
				path_hasFoundEscape = true;
				l.Clear();
				path_mustJump = path_current.jump.Contains(i);
				path_current = i;
				path_target = position;
			}
			else
				l.Remove(i);

		}
	}
	private void Emergency_FollowEscapeRoute()
	{
		id.MoveTowardsForward(path_target, 1f, 400);
		if((transform.position - path_target).sqrMagnitude < escape_distanceToNode * escape_distanceToNode)
			path_hasFoundEscape = false;
	}
	private void Emergency_ThrowGrenades()
	{
		if(escape_throwGrenadeTime.CountdownReturn())
		{
			var position = escape_grenadePivot.position;
			var direction = escape_grenadePivot.forward;
			escape_grenade.Throw(id.health, position, direction, escape_grenadeThrowForce);
			id.UseSpecialAnimation();
			escape_throwGrenadeTime.Countdown_Restart();
			escape_GrenadesLeft--;
		}

	}

	private void Emergency_BegForMercy()
	{
		id.movement_walk = -1;
	}
}