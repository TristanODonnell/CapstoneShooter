using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Chase : Enemy_State
{

    [Header("Pathfinding")]
    private PathNode path_current;
    private List<PathNode> path_moveTowards = new();
    private PathNode path_next => path_moveTowards.Count > 0 ? path_moveTowards[0] : null;
    private bool mustJump;

    [SerializeField] private Countdown fight_beginChaseDelay = new(4);
	private Vector3 path_Target;
    private Vector3 path_LastEffort;

	public override void ActionStart()
    {
        path_current = PathFinderManager.GetClosestPathNode(transform.position);
        fight_beginChaseDelay.Countdown_Restart();
        id.movement_walk = 0;
    }

	public override void ActionUpdate()
    {
        if (!path_current)
        {
            fight_beginChaseDelay.Countdown_Restart();
            SetAction_Next();
            return;
        }

        if (!fight_beginChaseDelay.CountdownReturn())
        {
            if (fight_beginChaseDelay.mReviseCountdownIsOver) {
                path_LastEffort = Player_Detection.singleton.transform.position;
                path_moveTowards = PathNode.Pathfind_List(path_current, Player_Detection.singleton.transform.position);
            }
            return;
        }



        id.MoveTowardsForward(path_Target, degreesPerSecond: 180f);


        var distanceToTarget = (path_Target - transform.position);
        distanceToTarget.y = 0f;
        var minDistance = 1.25f;
        if (distanceToTarget.sqrMagnitude < minDistance * minDistance)
        {

            mustJump = path_current.jump.Contains(path_next);
            path_moveTowards.Remove(path_current);
            path_current = path_next;

            if (path_current)
            {
                var randomVector = Player_Detection.singleton.transform.position - transform.position;
                path_Target = path_current.transform.position + randomVector.normalized * 0.7f * path_current.radius;
                if (path_moveTowards.Count == 1)
                    path_Target = path_LastEffort;
            }
        }

        var movement = id.movement_speed * id.movement_speed;
        if (mustJump && (path_current.transform.position - transform.position).sqrMagnitude < movement)
            id.Jump();
    }
}
