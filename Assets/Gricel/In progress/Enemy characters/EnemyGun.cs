using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
	[SerializeField] Countdown shoot_firerate = new(0.2f);
	public void Shoot()
	{
		if (shoot_firerate.CountdownReturn())
			Debug.Log("Pew");

	}
}
