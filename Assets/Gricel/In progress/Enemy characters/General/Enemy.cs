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
        [SerializeField] private Transform eyes;
        [SerializeField] private EnemyGun gun;


        [Header("Fighting style")]
        [SerializeField] private float fight_Range_Short_distance = 12f;
        [SerializeField] private float fight_Detection_distance = 20f;
        [Range(10, 180)] [SerializeField]private float fight_Detection_visionAngle = 90f;
        [SerializeField] private Countdown_AutoReset fight_attemptDetect = new(1f);
        [SerializeField] private Countdown fight_beginChaseDelay = new(4);
        [SerializeField] private Countdown_AutoReset fight_changePosition = new(4);
        [SerializeField] private Countdown fight_shootInPlace = new(4);
        [SerializeField] private Countdown fight_shootTriggerHold = new(1);
        [SerializeField] private Countdown fight_shootTriggerRelease = new(2);
        private bool fight_facePlayerWalkBack;

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

        protected enum States : int
		{
            wander,
            chase,
            Aggresive,
            fight,
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
            enemy_State = TryToDetectPlayer();
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
                    movement_walk = Random.value * 0.9f + 0.1f;

                    RotateToWardsPosition(path_Target, 90f);

                }

                return;
            }



            movement_specialAnimation = true;

            MoveForward();



            wander_walkTime -= Time.deltaTime;
            if (wander_walkTime < 0)
            {
                wander_ThinkWhereToGoNext.Countdown_Restart();
                movement_vision = Vector2.zero;
            }

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
            enemy_State = TryToDetectPlayer();
            if (!path_current)
			{
                fight_beginChaseDelay.Countdown_Restart();
                enemy_State = States.wander;
                return;
            }

            if (!fight_beginChaseDelay.CountdownReturn())
			{
                if (fight_beginChaseDelay.mReviseCountdownIsOver)
                {
                    path_current = PathFinderManager.GetClosestPathNode(transform.position);
                    path_moveTowards = PathNode.Pathfind_List(path_current, Player_Detection.singleton.transform.position);
                }
                return;
            }



            movement_walk = 1f;
            RotateToWardsPosition(path_Target, 720f);
            MoveForward();


            var distanceToTarget = (path_Target - transform.position);
            distanceToTarget.y = 0f;
            var minDistance = movement_speed * 1.25f;
            if (path_current.IsInRadius(transform) && distanceToTarget.sqrMagnitude < minDistance * minDistance)
            {

                mustJump = path_current.jump.Contains(path_next);
                path_moveTowards.Remove(path_current);
                path_current = path_next;
            }
            if(path_current)
				if (!path_current.IsInRadius(path_Target))
				{
                    var randomVector = Player_Detection.singleton.transform.position - transform.position;
                    path_Target = path_current.transform.position + randomVector.normalized * 0.7f * path_current.radius;
                }

            var movement = movement_speed * movement_speed;
            if (mustJump && (path_current.transform.position - transform.position).sqrMagnitude < movement)
                Jump();
        }
        protected void OnState_Fight()
        {
            enemy_State = TryToDetectPlayer();
            if (fight_attemptDetect.mReviseCountdownIsOver)
                --enemy_State;

            var visionDirection = (Player_Detection.singleton.transform.position - 
                transform.position);

            movement_vision = visionDirection;
            movement_specialAnimation = false;

            if (!fight_shootInPlace.CountdownReturn())
            {
                RotateToWardsPosition(Player_Detection.singleton.transform.position, 60f);
                Attack_HandleShooting();
                movement_walk = 0;
                path_current = null;
                return;
			}


            if (!path_current)
			{
                path_current = PathFinderManager.GetClosestPathNode(transform.position + transform.forward * (Random.value-0.5f) );
                Attack_ChangePosition();
            }







            if (fight_facePlayerWalkBack)
            {
                var dir = transform.position - path_Target;
                dir.y = 0;
                var nRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, nRot, 45 * Time.deltaTime);
            }
			else
                RotateToWardsPosition(path_Target, 90);

            Attack_HandleShooting();




            var distanceToTarget = Player_Detection.singleton.transform.position - transform.position;
            MoveForward();
            if(distanceToTarget.sqrMagnitude < movement_speed * movement_speed ||
                fight_changePosition.CountdownReturn()||
                (path_Target-transform.position).sqrMagnitude < movement_speed*movement_speed)
			{
                var randomOption = Random.value;
                if (randomOption > 0.9)
                    path_current = path_current.next[Random.Range(0, path_current.next.Count)];
                else if (randomOption > 0.5)
                {
                    Attack_ChangePosition();
                    if (Random.value > 0.7)
                        Jump();
                }
                else if (randomOption > 0.25)
                    path_current = null;
                else
                {
                    fight_shootInPlace.Countdown_Restart();
                }
                return;
			}
        }

        protected void Attack_ChangePosition()
		{

            var randomSpot = Vector3.zero;
            randomSpot.x = Random.Range(-1f, 1f);
            randomSpot.z = Random.Range(-1f, 1f);
            randomSpot *= path_current.radius * 1.01f;
            var visionDirection = Player_Detection.singleton.transform.position -
                eyes.transform.position;

            var q = Quaternion.Euler(transform.forward);
            var r = Quaternion.Euler(visionDirection);
            fight_facePlayerWalkBack = Quaternion.Angle(q, r) > 90f;

            path_Target = path_current.transform.position + randomSpot;
            if (!path_current.IsInRadius(transform))
                path_Target = path_current.transform.position;
            movement_walk = Random.Range(0.1f, 1f) * (fight_facePlayerWalkBack ? -1f : 1f);
        }
        protected void Attack_HandleShooting()
		{

			if (!fight_shootTriggerHold.CountdownReturn())
			{
                fight_shootTriggerRelease.Countdown_Restart();
                gun.Shoot();
                return;
			}
            if (fight_shootTriggerRelease.CountdownReturn())
                fight_shootTriggerHold.Countdown_Restart();


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
            path_current = PathFinderManager.GetClosestPathNode(transform.position);
        }

		private void OnDrawGizmosSelected()
		{
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, path_Target);
		}
		protected States TryToDetectPlayer()
        {
            if (!fight_attemptDetect.CountdownReturn())
                return enemy_State;

            var rOrigin = eyes.position;
            var rEnd = Player_Detection.singleton.transform.position;
            var rDirection = rEnd - rOrigin;

            if (rDirection.sqrMagnitude > fight_Detection_distance * fight_Detection_distance)
                return enemy_State;


            var angOrigin = eyes.rotation;
            angOrigin.x = 0;
            angOrigin.z = 0;
            var angEnd = Quaternion.LookRotation(rDirection);
            angEnd.x = 0;
            angEnd.z = 0;
            var angTotal = Quaternion.Angle(angOrigin, angEnd);

            if (angTotal > fight_Detection_visionAngle)
                return enemy_State;



            var raycasts = Physics.RaycastAll(rOrigin, rDirection,
                fight_Detection_distance);

            foreach (var r in raycasts)
            {
                if (r.collider.gameObject == Player_Detection.singleton.gameObject)
                {
                    path_moveTowards.Clear();
                    return States.fight;
                }
                if (!r.collider.GetComponent<Enemy>() && !r.collider.GetComponent<Hitbox>())
                    return enemy_State;
            }
            return enemy_State;
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
            var v3 = Vector3.zero;
            v3.x = Random.Range(-1f, 1f);
            v3.z = Random.Range(-1f, 1f);
            transform.forward = v3;
            path_Target = transform.position + transform.forward * 100;
            path_current = PathFinderManager.GetClosestPathNode(transform.position + transform.forward * wander_MaxwalkTime);
            fight_changePosition.Countdown_ForceSeconds(0);
        }

		// Update is called once per frame
		void Update()
        {
            movement_isFalling = gravity.isJumping;
            switch (enemy_State)
            {
                case States.wander:
                    OnState_Wander();
                    break;
                case States.chase:
                    OnState_Chase();
                    break;
                case States.fight:
                case States.Aggresive:
                    OnState_Fight();
                    break;
            }
		}
    }
}