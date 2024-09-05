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
    private float walkAxisPriority;
    [SerializeField] private float rotationPerSecond = 720f;


    [Header("Pathfinding")]
    private PathNode path_current;
    private Vector3 path_target;
    private bool mustJump;

	public override void ActionStart()
    {
        path_current = PathFinderManager.GetClosestPathNode(transform.position);
        path_target = path_current.transform.position;
        walkAxisPriority = 0;
        id.movement_specialAnimation = false;
    }

    public override void ActionUpdate()
    {

        if (state_goToNext.CountdownReturn())
        {
            SetAction_Next();
            return;
        }

		Fight_UseGun();

        Fight_KeepEyesLookingAtPlayer();
        id.DetectPlayer_CancelDetection();
        if (fight_runToNextPathnode.CountdownReturn())
            Fight_Run();
        else
            id.movement_walk = 0;

        
        


        if(path_current)
            if (path_current.IsInRadiusOfJump(transform.position, 2f) && mustJump)
                id.Jump();


		var distanceToTarget = path_target - transform.position;
		distanceToTarget.y = 0;
		if ((distanceToTarget).sqrMagnitude < stopDistance * stopDistance)
			Fight_ChangeTarget();
	}

    private void Fight_UseGun()
	{

        if (fight_shootTriggerRelease.CountdownReturn())
            return;
        if (fight_shootTriggerHold.CountdownReturn())
            id.ShootWeapon();
    }

	private void Fight_ChangeTarget()
	{

        List<PathNode> l = new();
        if(!path_current)
            path_current = PathFinderManager.GetClosestPathNode(transform.position);
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

            var rOrgn = selected.transform.position + Vector3.up* 2f;
            var rEnd = Player_Detection.position;
            var rDir = rEnd - rOrgn;
            var raycast = Physics.RaycastAll(rOrgn, rDir, rDir.magnitude, -1, QueryTriggerInteraction.Ignore);

            var desPosDistToPlayer = rEnd - desiredPosition;
            desPosDistToPlayer.y = 0f;

            var hasHitWall = false;
            foreach (var r in raycast)
            {
                if (!r.collider.GetComponent<CharacterController>())
                {
					Debug.Log($"{selected.name} => hit {r.collider.name}");

					hasHitWall = true;
                    break;
                }
            }

            if ((desPosDistToPlayer).sqrMagnitude >= (id.GetVisionDistance() * id.GetVisionDistance()) || hasHitWall)
            {
                l.Remove(selected);
                if (l.Count == 0)
				{
                    state_goToNext.Countdown_ForceSeconds(-1f);
                    id.DetectPlayer_CancelDetection();
					return;
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



        if (totalRotation < 90)
        {
            walkAxisPriority -= Time.deltaTime;
        }
        else
        {
            walkAxisPriority += Time.deltaTime*2f;
        }
        walkAxisPriority = Mathf.Clamp(walkAxisPriority, -1f, 1f);

        if (walkAxisPriority < 0)
            id.MoveTowardsBackward(path_target, 1, rotationPerSecond, stopDistance * 0.5f);
        else
            id.MoveTowardsForward(path_target, 1, rotationPerSecond, stopDistance * 0.5f);
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
