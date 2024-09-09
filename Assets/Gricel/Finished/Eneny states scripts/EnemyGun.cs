using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
	[SerializeField] ShootBehavior[] weapons;
	Countdown shooting = new(0.2f);

	private void Start()
	{
		shooting.Countdown_ForceSeconds(-1);
	}
	public void Shoot()
	{
		shooting.Countdown_Restart();
	}
	private void Update()
	{
		foreach (var weapon in weapons)
		{
			if (shooting.CountdownReturn())
				weapon.StartShooting();
			else weapon.StopShooting();
		}
	}
}
