using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_State : MonoBehaviour
{
	public gricel.Enemy id { get; private set; }
	[SerializeField] private Enemy_State state_next;
	protected Vector3 playerPosition => Player_Detection.position;
    private void OnValidate()
	{
		if (!id)
			id = GetComponent<gricel.Enemy>();
	}


	protected void SetAction_Next()
	{
		if(state_next)
			id.SetState(state_next);
	}
	protected void SetAction_Base()
	{
		id.SetBaseState();
	}
	protected void SetAction_Custom(Enemy_State customState)
	{
		id.SetState(customState);
	}

	public abstract void ActionStart();
	public abstract void ActionUpdate();
}
