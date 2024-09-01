using UnityEngine;

public class EnemyState_ShootInPlace : Enemy_State
{
    [SerializeField] private Countdown_AutoReset state_goToNext = new(5);
    [SerializeField] private Countdown shoot_Hold = new(1);
    [SerializeField] private Countdown shoot_Release = new(2);
    private bool fight_facePlayerWalkBack;

    public override void ActionStart()
    {
    }

    public override void ActionUpdate()
    {

    }
}