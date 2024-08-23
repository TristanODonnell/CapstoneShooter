using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class pathFinding : MonoBehaviour
{
    private PathNode spot_Current;
    private List<PathNode> spot_GoTo = new();

    [SerializeField]
    private GravitationalBehaviour gravitation;
    public float movement_speed;
    public CharacterController movement;
    public Transform movement_target;

    public float jumpForce;
    [SerializeField]
    private float jump;

    private void OnDrawGizmos()
	{
        Vector3 prevPos = transform.position;
        Gizmos.color = Color.blue;
		foreach (var item in spot_GoTo)
		{
            Gizmos.DrawLine(prevPos, item.transform.position);
            prevPos = item.transform.position;
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        spot_Current = PathFinderManager.GetClosestPathNode(transform.position);
        LookTarget();
    }

    void LookTarget()
	{
        var dir = spot_Current.transform.position - transform.position;
        dir.y = 0;
        transform.forward = dir;
    }
    bool mustJump = false;


    [SerializeField]
    private SlowRequest spot_delayNewPoint;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (spot_GoTo.Count == 0)
            if (spot_delayNewPoint.ExecuteRequest())
                spot_GoTo = PathNode.Pathfind_List(spot_Current, movement_target.transform.position);
        var inRadius = spot_Current.IsInRadius(transform.position);
        if(mustJump && spot_Current)
            inRadius = spot_Current.IsInRadius(transform.position, 1.25f);



        Fall();
        if (spot_GoTo.Count > 0 && !inRadius)
            MoveToTarget();

        


        if (inRadius)
        {
            mustJump = false;
            if (spot_GoTo.Count > 1)
            {
                var jumpNode = spot_GoTo[1];
                mustJump = spot_Current.jump.Contains(jumpNode);
            }

            if (spot_GoTo.Count == 0)
            {
                MoveInsideCurrent();
                return;
            }

            spot_GoTo.RemoveAt(0);

            if (spot_GoTo.Count == 0)
                return;
            spot_Current = spot_GoTo[0];
        }

    }
    void MoveInsideCurrent()
	{

	}
    void MoveToTarget()
	{
        var prevTrans = transform.position;
        LookTarget();
        movement.Move((transform.forward * movement_speed) * Time.deltaTime);
        jump += Time.deltaTime * Physics.gravity.y;

        
        //Emergency jump
        if ((prevTrans - transform.position).sqrMagnitude < Time.deltaTime * Time.deltaTime * movement_speed * 0.01f)
        {
            Jump();
            spot_GoTo = PathNode.Pathfind_List(spot_Current, transform.position + transform.forward * 10f);
        }
    }
    void Jump()
	{
        jump = jumpForce;
    }
    void Fall()
	{
        movement.Move(transform.up * jump * Mathf.Abs(jump) * Time.deltaTime);
        jump += Time.deltaTime * gravitation.currentGravity;

        if (movement.isGrounded)
        {
            jump = gravitation.currentGravity;
            if (spot_Current.IsInRadiusOfJump(transform.position + Vector3.down, movement_speed * 0.5f) && mustJump)
                jump = jumpForce;

        }
    }
}
