using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
	[SerializeField] ShootBehavior[] weapons = new ShootBehavior[0];
	Countdown shooting = new(0.2f);

	private void OnValidate()
	{
		if (weapons.Length == 0)
		{
			var enemy = GetComponentInParent<gricel.Enemy>();
			if (enemy == null) return;
			enemy.gun = this;
			weapons = enemy.GetComponentsInChildren<ShootBehavior>();
		}
	}

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
