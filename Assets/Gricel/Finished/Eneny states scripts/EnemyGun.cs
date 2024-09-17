using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
	[SerializeField] ShootBehavior[] weapons = new ShootBehavior[0];
	Countdown shooting = new(0.2f);
	[SerializeField] private float missAngle = 10f;
	[SerializeField] gricel.Enemy enemy;

	private void OnValidate()
	{
		if (weapons.Length == 0 && enemy)
		{
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
			{
				weapon.weaponTip.transform.forward = Player_Detection.position - transform.position;
				float R() => Random.Range(-1f, 1f) * missAngle;


				var rotMiss = Quaternion.Euler(R(), R(), R()).eulerAngles + 
				weapon.weaponTip.transform.rotation.eulerAngles;

				weapon.weaponTip.transform.position = rotMiss;


				if(Quaternion.Angle(weapon.weaponTip.rotation, enemy.transform.rotation) <  180f)
					weapon.StartShooting(false);
			}
			else weapon.StopShooting(false);
		}
	}
}
