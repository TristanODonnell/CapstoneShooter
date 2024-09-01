using UnityEngine;
using System.Collections.Generic;

public class EnemyState_Fight : Enemy_State
{
    [SerializeField] Enemy_State playerOffSight;
    [Header("Fighting style")]
    [SerializeField] private Countdown_AutoReset state_goToNext = new(4);
    [SerializeField] private Countdown fight_runToNextPathnode = new(1);
    [SerializeField] private Countdown fight_shootTriggerHold = new(1);
    [SerializeField] private Countdown fight_shootTriggerRelease = new(8);
    [SerializeField] private float stopDistance = 1f;
    [SerializeField] private float walkAxisPriority;


    [Header("Pathfinding")]
    private PathNode path_current;
    private Vector3 path_target;
    private bool mustJump;

	private void OnDrawGizmos()
	{
        Gizmos.color = Color.red;
        Gizmos.DrawCube(path_target, Vector3.one);
	}

	public override void ActionStart()
    {
        path_current = PathFinderManager.GetClosestPathNode(transform.position);
        path_target = path_current.transform.position;
        walkAxisPriority = 0;
        id.movement_specialAnimation = false;
    }

    public override void ActionUpdate()
    {
        Fight_KeepEyesLookingAtPlayer();
        if (fight_runToNextPathnode.CountdownReturn())
            Fight_Run();
        else
            id.movement_walk = 0;

        var distanceToTarget = path_target - transform.position;
        distanceToTarget.y = 0;
        if ((distanceToTarget).sqrMagnitude < stopDistance * stopDistance)
            Fight_ChangeTarget();
        if (state_goToNext.CountdownReturn())
            SetAction_Next();
        if(path_current)
            if (path_current.IsInRadiusOfJump(transform.position, 2f) && mustJump)
                id.Jump();
    }

	private void Fight_ChangeTarget()
	{
		if (!id.TryToDetectPlayer(true))
		{
            SetAction_Custom(playerOffSight);
            return;
		}

        List<PathNode> l = new();
        l.Add(path_current);
        l.AddRange(path_current.jump);
        l.AddRange(path_current.next);
        l.AddRange(path_current.drop);
        walkAxisPriority = 0f;
        fight_runToNextPathnode.Countdown_Restart();
        var prev = path_current;
        path_current = null;

        var randomPosition = Vector3.zero;
        randomPosition.x = Random.Range(-1f, 1f);
        randomPosition.z = Random.Range(-1f, 1f);
        while (!path_current)
		{
            var selected = l[Random.Range(0, l.Count)];

            var desiredPosition = selected.radius * randomPosition + selected.transform.position;

            var rOrgn = selected.transform.position + Vector3.up;
            var rEnd = Player_Detection.position;
            var rDir = rEnd - rOrgn;
            var raycast = Physics.RaycastAll(rOrgn, rEnd, id.GetVisionDistance(), -1, QueryTriggerInteraction.Ignore);

            var hasHitWall = false;
			foreach (var r in raycast)
				if (!r.collider.GetComponent<CharacterController>())
				{
                    hasHitWall = true;
                    break;
				}

            if (desiredPosition.sqrMagnitude >= id.GetVisionDistance() * id.GetVisionDistance() || hasHitWall)
            {
                l.Remove(selected);
                if (l.Count == 0)
                {
                    SetAction_Custom(playerOffSight);
                    break;
                }
                continue;
            }
            path_current = selected;
            path_target = desiredPosition;
		}
        mustJump = prev.jump.Contains(path_current);
	}

	private void Fight_Run()
    {
        var rotPlayerToEnemy = Quaternion.LookRotation(transform.position - Player_Detection.position);
        rotPlayerToEnemy.x = 0;
        rotPlayerToEnemy.z = 0;
        var rotEnemyToTarget = Quaternion.LookRotation(path_target - transform.position);
        rotEnemyToTarget.x = 0;
        rotEnemyToTarget.z = 0;

        var totalRotation = Quaternion.Angle(rotPlayerToEnemy, rotEnemyToTarget);



        Debug.Log($"Angle {totalRotation}");
        if (totalRotation < 90)
        {
            walkAxisPriority -= Time.deltaTime;
            Debug.Log("X--");
        }
        else
        {
            walkAxisPriority += Time.deltaTime*2f;
            Debug.Log("X++");
        }
        walkAxisPriority = Mathf.Clamp(walkAxisPriority, -1f, 1f);

        if (walkAxisPriority < 0)
            id.MoveTowardsBackward(path_target, 1, 360f, stopDistance * 0.5f);
        else
            id.MoveTowardsForward(path_target, 1, 360f, stopDistance * 0.5f);
    }

	private void Fight_KeepEyesLookingAtPlayer()
	{
        var toPlayer = Player_Detection.position - transform.position;
        var multiplication = Quaternion.FromToRotation(transform.forward, toPlayer);

        var toVision = Vector3.zero;
        toVision.Set(multiplication.y * 2f, multiplication.x, 0f);
        id.movement_vision = toVision;
	}
}
