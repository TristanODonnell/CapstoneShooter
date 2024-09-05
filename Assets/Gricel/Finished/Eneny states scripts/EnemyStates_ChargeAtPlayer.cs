using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random =UnityEngine.Random;

public class EnemyStates_ChargeAtPlayer : Enemy_State
{
	private PathNode path_current;
	private List<PathNode> path_next = new();
	private bool path_mustJump;
	[SerializeField] private float path_degreesPerSecond = 360;
	private Vector3 path_player => Player_Detection.position;

	[SerializeField] private Countdown_AutoReset path_recalculate = new(4);
	[SerializeField] private Countdown_AutoReset attack_hitDelay = new(2);
	[SerializeField] private float attack_Distance = 7f;


	public override void ActionStart()
	{
		Charge_RecalculatePath();
	}
	


	public override void ActionUpdate()
	{
		if (path_recalculate.CountdownReturn())
			Charge_RecalculatePath();
		if (Attack_IsInRange())
			Attack_HitPlayer();
		else
			Charge_PathfindToPlayer();

	}

	private void Charge_PathfindToPlayer()
	{
		if (path_next.Count == 0)
			Charge_RunToPlayer();
		else
		{
			var pos = path_current.position;
			var extra = (path_player - pos).normalized * path_current.radius * 0.85f;
			if(!path_current.IsInRadius(transform.position, 0.75f))
				id.MoveTowardsForward(pos + extra, 1f, path_degreesPerSecond);
			if (path_current.IsInRadiusOfJump(transform, 0.2f))
				id.Jump();
			if (id.controller.isGrounded && path_current.IsInRadius(transform.position, 1f))
				Charge_PathFindNextNode();
		}
	}

	private void Charge_PathFindNextNode()
	{
		var prevPath = path_current;
		path_current = path_next[0];
		path_next.RemoveAt(0);
		path_mustJump = prevPath.jump.Contains(path_current);
	}

	private void Charge_RunToPlayer()
	{
		id.MoveTowardsForward(path_player, 1f, 480f);
	}

	private void Attack_HitPlayer()
	{
		if (!attack_hitDelay.CountdownReturn())
			return;
		id.UseSpecialAnimation();
		id.ShootWeapon();
		id.RotateToWardsPosition(path_player, 40f);
	}

	private bool Attack_IsInRange()
	{
		var distanceToPlayer = path_player - transform.position;
		var minDistance = attack_Distance * attack_Distance;
		id.movement_walk = 0;
		if(distanceToPlayer.sqrMagnitude < minDistance)
		{
			path_recalculate.Countdown_Restart();
			return true;
		}
		return false;
	}

	private void Charge_RecalculatePath()
	{
		id.DetectPlayer_SeeInFull360DegreesOnce();
		if (!id.TryToDetectPlayer(false))
			SetAction_Next();

		if (Random.value < 0.5 || !path_current)
			path_current = PathFinderManager.GetClosestPathNode(transform.position);
		path_next = PathNode.Pathfind_List(path_current, path_player);
	}
}
