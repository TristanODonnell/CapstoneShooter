using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyState_WanderQuick : Enemy_State
{
	private PathNode path_current;
	private Vector3 path_target;
	private bool path_mustJump = false;
	public override void ActionStart()
	{
		path_current = PathFinderManager.GetClosestPathNode(transform.position);
		Wander_RandomizeTarget();
		id.movement_vision *= 0f;
	}

	public override void ActionUpdate()
	{
		if (Wander_IsInTarget())
			Wander_ChangeTaget();
		Wander_WalkToTarget();
	}

	private void Wander_WalkToTarget()
	{
		var speed = id.movement_isFalling? 1f: 0.75f;
		id.MoveTowardsForward(path_target, speed, 360f);
		if (path_mustJump && path_current.IsInRadiusOfJump(transform.position, 2f))
			id.Jump();
	}

	private void Wander_ChangeTaget()
	{
		if(Random.value > 0.3f)
		{
			if (Random.value < 0.3f)
				path_current = PathFinderManager.GetClosestPathNode(transform.position);
			var listOfPaths = path_current.AllSurroundingPaths();

			var change_path = listOfPaths[Random.Range(0, listOfPaths.Count)];
			path_mustJump = path_current.jump.Contains(change_path);

			path_current = change_path;
		}


		Wander_RandomizeTarget();

	}

	private void Wander_RandomizeTarget()
	{
		float RandomVector() => Random.Range(-1f, 1f) * path_current.radius;
		path_target = path_current.position;
		path_target.x += RandomVector();
		path_target.z += RandomVector();
	}

	private bool Wander_IsInTarget()
	{
		var distance = path_target - transform.position;
		distance.y = 0f;

		return distance.sqrMagnitude < 2f;
	}
}
