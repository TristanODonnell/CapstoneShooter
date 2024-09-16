using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OtherEnemy = Enemy;
namespace gricel
{
    public class Enemy : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] public CharacterController controller;
        [SerializeField] private GravitationalBehaviour gravity;
        [SerializeField] private RagdollRig ragdoll;
        [SerializeField] public gricel.HealthSystem health;
        [SerializeField] private Transform eyes;
        [SerializeField] public EnemyGun gun;
        [SerializeField] private OtherEnemy enemyCosts;



        [Header("States")]
        [SerializeField] private Enemy_State state_Base;
        [SerializeField] private Enemy_State state_detection;
        private System.Action action;

        [SerializeField] private Countdown_AutoReset detectionAttempt_Timer = new(0.2f);
        [SerializeField] private float detectionAttempt_VisionDistance = 20f;
        [Range(10, 180)] [SerializeField] private float detectionAttempt_VisionAngle = 90f;
        public float GetVisionDistance() => detectionAttempt_VisionDistance;
        private bool detectFull360 = false;


        public Vector2 movement_vision
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
        public float movement_walk
        {
			get => animator.GetFloat("WalkZAxis");
			set => animator.SetFloat("WalkZAxis", Mathf.Clamp(value, -1f, 1f));
        }
        public bool movement_specialAnimation
        {
            get => animator.GetBool("SpecialMovement");
            set => animator.SetBool("SpecialMovement", value);
        }
        public bool movement_isFalling
        {
            get => animator.GetBool("isOnAir");
            set => animator.SetBool("isOnAir", value);
        }

        [Header("Movement")]
        [SerializeField] public float movement_speed = 4f;
		[SerializeField]private float path_JumpForce = 5f;



		private void OnDrawGizmosSelected()
		{
            if (eyes) {
                Gizmos.color = Color.blue;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawSphere(eyes.localPosition, 0.4f);
                Gizmos.DrawFrustum(eyes.localPosition, detectionAttempt_VisionAngle, detectionAttempt_VisionDistance, 0, 0.999f);
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(eyes.localPosition, detectionAttempt_VisionDistance);
            }
		}
		public void SetState(Enemy_State state)
        {
            if (state == null)
                return;
            if (action == state.ActionUpdate)
                return;
            state.ActionStart();
            action = state.ActionUpdate;
        }
        public void SetBaseState()
        {
            SetState(state_Base);
        }
        public void SetAlertedState()
        {
            SetState(state_detection);
        }
        public void ShootWeapon()
		{
            if(gun)
                gun.Shoot();
		}

        public void RotateToWardsPosition(Vector3 otherPosition, float mdd = float.MaxValue)
		{
            var direction = otherPosition - transform.position;
            direction.y = 0;
            Quaternion rotationTowards = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), mdd * Time.deltaTime);
            transform.rotation = rotationTowards;
        }
        public void MoveTowardsForward(Vector3 pos, float normalSpeed = 1, float degreesPerSecond = 90f, float stopDistance = 0f)
        {
            if (normalSpeed < 0)
                normalSpeed *= -1;
            
            pos -= transform.position;
            pos.y = 0;
            var mW = movement_walk = normalSpeed;

            if (normalSpeed == 0 || pos.sqrMagnitude < stopDistance * stopDistance)
                return;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(pos), degreesPerSecond * Time.deltaTime);

            var pPos = transform.position;
            var movement = movement_speed * mW * Time.deltaTime;
            controller.Move(transform.forward * movement);
            var comparisonMovement = movement * 0.5f;
            if ((pPos - transform.position).sqrMagnitude < comparisonMovement * comparisonMovement)
                Jump();
        }
        public void MoveTowardsBackward(Vector3 pos, float normalSpeed = 1, float degreesPerSecond = 60f, float stopDistance = 0.05f)
        {
            if (normalSpeed > 0)
                normalSpeed *= -1;
            pos -= transform.position;
            pos.y = 0;
            var mW = movement_walk = normalSpeed;

            if (normalSpeed == 0 || pos.sqrMagnitude < stopDistance * stopDistance)
                return;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(-pos), degreesPerSecond * Time.deltaTime);

            var pPos = transform.position;
            var movement = movement_speed * mW * Time.deltaTime;
            controller.Move(transform.forward * movement);
            var comparisonMovement = movement * 0.4f;
            if ((pPos - transform.position).sqrMagnitude < comparisonMovement * comparisonMovement)
                Jump();
        }
        public bool Jump()
		{
            if (!gravity.isJumping)
            {
                gravity.Jump(path_JumpForce);
                return true;
            }
            return false;
        }

		public bool TryToDetectPlayer(bool cancelCountdown = false)
        {
            if (!detectionAttempt_Timer.CountdownReturn() && !cancelCountdown)
                return false;

            var rOrigin = eyes.position;
            var rEnd = Player_Detection.singleton.transform.position;
            var rDirection = rEnd - rOrigin;

            if (rDirection.sqrMagnitude > detectionAttempt_VisionDistance * detectionAttempt_VisionDistance && !detectFull360)
                return false;


            var angOrigin = eyes.rotation;
            angOrigin.x = 0;
            angOrigin.z = 0;
            var angEnd = Quaternion.LookRotation(rDirection);
            angEnd.x = 0;
            angEnd.z = 0;
            var angTotal = Quaternion.Angle(angOrigin, angEnd);
            detectFull360 = false;


            if (angTotal > detectionAttempt_VisionAngle)
                return false;


            var raycasts = Physics.RaycastAll(rOrigin, rDirection,
                detectionAttempt_VisionDistance, -1, QueryTriggerInteraction.Ignore);

            foreach (var r in raycasts)
                if (!r.collider.GetComponent<CharacterController>())
                    return false;
            return true;
        }
        public void DetectPlayer_CancelDetection()
        {
            detectionAttempt_Timer.Countdown_ForceSeconds(detectionAttempt_Timer.maximumCount);
        }
        public void DetectPlayer_SeeInFull360DegreesOnce()
        {
            detectFull360 = true;
        }
        public void UseSpecialAnimation(bool loop = false)
        {
            animator.Play("Punch", 0);
            movement_specialAnimation = loop;
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
            SetBaseState();
            transform.forward = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            health.onDamaged.AddListener(SetAlertedState);
            health.onDeath.AddListener(Death);
        }

        private void Death()
        {
            enemyCosts.AddCurrency();
            enemyCosts.AddXP();
            enemyCosts.SpawnLoot();
            ragdoll.Spawn(transform);
            Destroy(gameObject);
        }


		void Update()
        {
            movement_isFalling = gravity.isJumping || health.isStunned;
            if (health.isStunned)
                return;
            if (action != state_detection.ActionUpdate)
                if (TryToDetectPlayer())
                    SetState(state_detection);
            if(action != null)
                action.Invoke();
		}
    }
}