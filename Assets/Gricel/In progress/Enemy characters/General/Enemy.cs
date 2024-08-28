using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gricel
{
    public class Enemy : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected CharacterController controller;
        [SerializeField] protected GravitationalBehaviour gravity;
        [SerializeField] protected RagdollRig ragdoll;
        [SerializeField] protected gricel.HealthSystem health;


        [Header("Fighting style")]
        [SerializeField] private float fight_Range_Short_distance = 12f;
        [SerializeField] private float fight_Range_Long_distance = 20f;
        [SerializeField] private float fight_Detection_distance = 20f;
        [SerializeField] private Countdown_AutoReset fight_attemptDetect = new(1f);
        [SerializeField] private Countdown fight_beginChaseDelay = new(4);
        [SerializeField] private LayerMask fight_wallGroundLayer;
        private Vector3 fight_playerLastKnownPosition;

        [Header("Pathfinding")]
        private PathNode path_current;
        private List<PathNode> path_moveTowards = new();
        private PathNode path_next => path_moveTowards.Count > 0? path_moveTowards[0] : null;
        private Vector3 path_Target;
        private bool mustJump;
        [SerializeField] private float path_JumpForce = 8f;




        [SerializeField] protected Vector2 movement_vision
		{
			get
			{
                var r = Vector2.zero;
                r.x = animator.GetFloat("LookX");
                r.y = animator.GetFloat("LookY");
                return r;
            }
			set
			{
                value = value.normalized;
                animator.SetFloat("LookX", value.x);
                animator.SetFloat("LookY", value.y);
            }
		}
        [SerializeField] protected float movement_walk
        {
			get => animator.GetFloat("WalkZAxis");
			set => animator.SetFloat("WalkZAxis", Mathf.Clamp(value, -1f, 1f));
        }
        [SerializeField]
        protected bool movement_specialAnimation
        {
            get => animator.GetBool("SpecialMovement");
            set => animator.SetBool("SpecialMovement", value);
        }
        [SerializeField]
        protected bool movement_isFalling
        {
            get => animator.GetBool("isOnAir");
            set => animator.SetBool("isOnAir", value);
        }

        [SerializeField] private float movement_speed = 4f;

        protected enum States
		{
            wander,
            chase,
            fight
		}
        [SerializeField] protected States enemy_State = States.wander;



        [Header("Wandering")]
        [SerializeField] protected Countdown wander_ThinkWhereToGoNext = new(6);
        [SerializeField] protected Countdown_AutoReset wander_LookAroundTime = new(2);
        [SerializeField][Range(1f, 12f)] private float wander_MaxwalkTime = 4f;
        private float wander_walkTime = 0;
        private Vector2 wander_randomLookTarget;

        protected void OnState_Wander()
        {

            if (!path_current)
                path_current = PathFinderManager.GetClosestPathNode(transform.position);
            if (!wander_ThinkWhereToGoNext.CountdownReturn())
            {

                if (wander_LookAroundTime.CountdownReturn())
                {
                    wander_randomLookTarget = Vector2.zero;
                    wander_randomLookTarget.x = Random.Range(-1f, 1f);
                    wander_randomLookTarget.y = Random.Range(-1f, 1f);
                }

                movement_walk = 0f;
                movement_specialAnimation = false;

                movement_vision = Vector2.Lerp(movement_vision, wander_randomLookTarget, Time.deltaTime);

                if (wander_ThinkWhereToGoNext.mReviseCountdownIsOver)
                {
                    if(Random.value < 0.9f)
					{
                        if (path_current.IsInRadius(transform))
                        {
                            List<PathNode> l = new();
                            l.AddRange(path_current.next);
                            l.AddRange(path_current.drop);
                            path_current = l[Random.Range(0, l.Count)];
                        }
                        else
                            path_current = PathFinderManager.GetClosestPathNode(transform.position);
					}
                    var targerDirection = Vector3.zero;
                    targerDirection.x = Random.Range(-1f, 1f);
                    targerDirection.y = Random.Range(-1f, 1f);

                    path_Target = path_current.transform.position + targerDirection * path_current.radius;
                    wander_walkTime = Random.Range(0.5f, 1f) * wander_MaxwalkTime;

                    movement_vision = Vector2.zero;

                    RotateToWardsPosition(path_Target, 90f);

                }

                return;
            }



            movement_specialAnimation = true;
            movement_walk = 0.3f;

            MoveForward();



            wander_walkTime -= Time.deltaTime;
            if (wander_walkTime < 0)
                wander_ThinkWhereToGoNext.Countdown_Restart();

            if (!path_current.IsInRadius(transform))
            {
                path_Target = path_current.transform.position;
                RotateToWardsPosition(path_Target, 180f);
                wander_walkTime = 0.2f;
            }

        }

        public void RotateToWardsPosition(Vector3 otherPosition, float mdd = float.MaxValue)
		{
            var direction = otherPosition - transform.position;
            direction.y = 0;
            Quaternion rotationTowards = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), mdd * Time.deltaTime);
            transform.rotation = rotationTowards;
        }
        protected void OnState_Chase()
        {
            if (!path_current)
			{
                fight_beginChaseDelay.Countdown_Restart();
                enemy_State = States.wander;
                return;
            }

            if (!fight_beginChaseDelay.CountdownReturn())
			{
                if(fight_beginChaseDelay.mReviseCountdownIsOver)
                    path_moveTowards = PathNode.Pathfind_List(path_current, Player_Detection.singleton.transform.position);
                return;
            }



            var stop = (path_current.transform.position - transform.position).sqrMagnitude < Time.deltaTime * movement_speed * Time.deltaTime * movement_speed;
            movement_walk = stop? movement_walk - Time.deltaTime * 2f: 1f;
            RotateToWardsPosition(path_Target, 720f);
            MoveForward();


            if(path_current.IsInRadius(transform))
                if (path_moveTowards.Count > 0)
                {

                    mustJump = path_current.jump.Contains(path_next);
                    path_moveTowards.Remove(path_current);
                    path_current = path_next;
                    if (path_current)
                        path_Target = path_current.transform.position;
                }
			    else
			    {
                    path_Target = fight_playerLastKnownPosition;
                    if (Vector3.Distance(path_Target, transform.position) < fight_Range_Short_distance)
                        path_current = null;
                }


            var movement = movement_speed * movement_speed;
            if (mustJump && (path_current.transform.position - transform.position).sqrMagnitude < movement)
                Jump();
        }
        protected void OnState_Fight()
        {
            if (!path_current)
                path_current = PathFinderManager.GetClosestPathNode(transform.position);
            movement_walk = 0f;

            fight_attemptDetect.Countdown_Restart();

            RotateToWardsPosition(fight_playerLastKnownPosition);
        }



        protected void MoveForward()
		{
            var mW = movement_walk;
            if (Mathf.Abs(mW) <= 0.05f)
                return;

            var pPos = transform.position;
            var movement = movement_speed * mW * Time.deltaTime;
            controller.Move(transform.forward * movement);
            var comparisonMovement = movement * 0.5f;
            if ((pPos - transform.position).sqrMagnitude < comparisonMovement * comparisonMovement)
                Jump();
        }
        private void Jump()
		{
            if (!gravity.isJumping)
                gravity.Jump(path_JumpForce);
        }


        protected bool TryToDetectPlayer()
		{
            if (!fight_attemptDetect.CountdownReturn() || !Player_Detection.singleton)
                return false;
            if (!IsInRadius(fight_Detection_distance))
                return false;

            ShootEyeRaycast(out var hit, fight_Detection_distance);
            var dir = Player_Detection.singleton.transform.position - transform.position;
            var angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir));
            if (angle > 180)
                return false;

            if (hit.collider)
                return false;

            ForcePlayerDetection();
            return true;
        }
        private bool ShootEyeRaycast(out RaycastHit hit, float maxDistance)
		{
            var pos = transform.position + Vector3.up * controller.height * 0.75f;
            var tpos = Player_Detection.singleton.transform.position;
            var dir = tpos - pos;
            var rc = Physics.Raycast(pos, dir, out hit, maxDistance, fight_wallGroundLayer);

            return rc;
        }
        private void ForcePlayerDetection()
		{
            enemy_State = States.fight;
            path_current = PathFinderManager.GetClosestPathNode(transform.position);
            fight_playerLastKnownPosition = Player_Detection.singleton.transform.position;
            RotateToWardsPosition(fight_playerLastKnownPosition);
        }

        protected bool IsInRadius(float radius)
		{
            var playerPos = Player_Detection.singleton.transform.position;
            var thisPos = transform.position;

            var distance = playerPos - thisPos;


            return distance.sqrMagnitude < radius * radius;
        }

        public void Component_Enforce<T>(ref T setVariable) where T: Component
		{
            if (!setVariable)
            {
                setVariable = gameObject.GetComponent<T>();
                if (!setVariable)
                    setVariable = gameObject.AddComponent<T>();
            }
        }
        private void OnValidate()
        {
            Component_Enforce(ref animator);
            Component_Enforce(ref controller);
            Component_Enforce(ref health);
            Component_Enforce(ref gravity);
        }
        private void Start()
		{
            path_Target = transform.position;
            path_current = PathFinderManager.GetClosestPathNode(transform.position + transform.forward * wander_MaxwalkTime);

        }

		// Update is called once per frame
		void Update()
        {

            movement_isFalling = gravity.isJumping;
            if (!TryToDetectPlayer() && enemy_State == States.fight)
                enemy_State = States.chase;
            switch (enemy_State)
            {
                case States.wander:
                    OnState_Wander();
                    break;
                case States.chase:
                    OnState_Chase();
                    break;
                case States.fight:
                    OnState_Fight();
                    break;
            }
		}
    }
}