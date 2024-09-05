using UnityEngine;

public class EnemyState_ShootInPlace : Enemy_State
{
    [SerializeField] private Countdown_AutoReset state_goToNext = new(5);
    [SerializeField] private Countdown shoot_Hold = new(1);
    [SerializeField] private Countdown shoot_Release = new(2);
    [SerializeField] private float shoot_degreesPerSecond = 30f;
    public override void ActionStart()
    {
    }

	private void Fight_KeepEyesLookingAtPlayer()
	{
		var toPlayer = Player_Detection.position - transform.position;
		var multiplication = Quaternion.FromToRotation(transform.forward, toPlayer);

		var toVision = Vector3.zero;
		toVision.Set(multiplication.y * 2f, multiplication.x * 2f, 0f);
		id.movement_vision = toVision;

		id.movement_walk = 0;
	}
	public override void ActionUpdate()
    {
        if (state_goToNext.CountdownReturn())
            SetAction_Next();

        id.RotateToWardsPosition(Player_Detection.position, shoot_degreesPerSecond);
        Fight_KeepEyesLookingAtPlayer();
        if (!shoot_Release.CountdownReturn())
        {
            shoot_Hold.Countdown_Restart();
            return;
        }
        if (!shoot_Hold.CountdownReturn())
        {
            id.ShootWeapon();
            if (shoot_Hold.mReviseCountdownIsOver)
                shoot_Release.Countdown_Restart();
        }
    }
}