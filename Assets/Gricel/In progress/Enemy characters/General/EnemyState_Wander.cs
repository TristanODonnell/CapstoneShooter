using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Wander : Enemy_State
{

    [Header("Wandering")]
    [SerializeField] private Countdown wander_ThinkWhereToGoNext = new(6);
    [SerializeField] private Countdown_AutoReset wander_LookAroundTime = new(2);
    [SerializeField] private Countdown_AutoReset wander_JumpToState = new(300);


    private Vector2 wander_randomLookTarget;
    private byte giveUp;
    private float distanceToGiveUp;
    private bool mustJump;
    private bool TryGivingUp()
	{
        var prevDistanceToGiveUp = distanceToGiveUp;
        distanceToGiveUp = (path_Target - transform.position).sqrMagnitude;
        if (distanceToGiveUp > prevDistanceToGiveUp && giveUp > 0)
            giveUp--;
        return giveUp == 0;
	}
    private void ResetGiveUp()
	{
        giveUp = 10;
	}
    private float current_walkNormalizedSpeed;

    [Header("Pathfinding")]
    private PathNode path_current;
    private Vector3 path_Target;

    public override void ActionUpdate()
    {
        if (wander_JumpToState.CountdownReturn())
            SetAction_Next();
        if (!wander_ThinkWhereToGoNext.CountdownReturn())
        {

            if (wander_LookAroundTime.CountdownReturn())
                SetRandomLookPlace();
            LookAroundInPlace();

            if (wander_ThinkWhereToGoNext.mReviseCountdownIsOver)
            {
                if (Random.value < 0.9f)
                {
                    if (path_current.IsInRadius(transform) || path_current.radius < 3)
                        SetRandomSpotToWalk_FromNext();
                    else
                        path_current = PathFinderManager.GetClosestPathNode(transform.position);
                }
                SetRandomSpotToWalkTo();

            }

            return;
        }

        Walk_ToTarget();
        if ((path_Target - transform.position).sqrMagnitude * 1.1f < path_current.radius * path_current.radius && mustJump)
            id.Jump();
    }
	private void OnDrawGizmos()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(path_Target, Vector3.one);
	}
	private void Walk_ToTarget()
    {
        id.movement_specialAnimation = true;


        var pathCalculation = path_Target - transform.position;
        pathCalculation.y = 0;
        if (((pathCalculation).sqrMagnitude < 1f && path_current.IsInRadius(transform)) || TryGivingUp())
        {
            wander_ThinkWhereToGoNext.Countdown_Restart();
            id.movement_vision = Vector2.zero;
            id.movement_specialAnimation = false;
            id.movement_walk = 0;
            ResetGiveUp();
        }
		else
            id.MoveTowardsForward(path_Target, current_walkNormalizedSpeed, 360f * current_walkNormalizedSpeed);

    }

    private void SetRandomSpotToWalk_FromNext()
	{
        List<PathNode> l = new();
        l.AddRange(path_current.next);
        l.AddRange(path_current.drop);
        l.AddRange(path_current.jump);
        var prevPath = path_current;
        if (l.Count > 0)
            path_current = l[Random.Range(0, l.Count)];
        mustJump = prevPath.jump.Contains(path_current);
        current_walkNormalizedSpeed = Random.Range(0.3f, 1f);
    }
    private void SetRandomSpotToWalkTo()
	{
        var targerDirection = Vector3.zero;
        targerDirection.x = Random.Range(-1f, 1f);
        targerDirection.y = Random.Range(-1f, 1f);

        path_Target = path_current.transform.position + targerDirection * path_current.radius;

        id.movement_vision = Vector2.zero;
    }
	private void SetRandomLookPlace()
	{
        wander_randomLookTarget = Vector2.zero;
        wander_randomLookTarget.x = Random.Range(-1f, 1f);
        wander_randomLookTarget.y = Random.Range(-1f, 1f);
    }
    
	private void LookAroundInPlace()
    {
        id.movement_walk = 0f;
        id.movement_specialAnimation = false;

        id.movement_vision = Vector2.Lerp(id.movement_vision, wander_randomLookTarget, Time.deltaTime);
    }

	public override void ActionStart()
    {
        path_current = PathFinderManager.GetClosestPathNode(transform.position);
        path_Target = transform.position;
        path_Target.y = path_current.transform.position.y;
        current_walkNormalizedSpeed = 0.5f;
        ResetGiveUp();
    }
}
