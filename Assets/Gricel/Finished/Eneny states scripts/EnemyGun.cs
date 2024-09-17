using gricel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
	[SerializeField] ShootBehavior[] weapons = new ShootBehavior[0];
	Countdown_AutoReset shooting = new(0.2f);
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
		enemy = GetComponentInParent<gricel.Enemy>();
		weapons = enemy.GetComponentsInChildren<ShootBehavior>();
		shooting.maximumCount = weapons[0].currentWeapon.fireRate;
	}
	public void Shoot()
	{
		if(shooting.CountdownReturn())
			foreach (var weapon in weapons)
			{
				float R() => Random.Range(-1f, 1f) * missAngle;
				var rotMiss = Quaternion.Euler(R(), R(), R()).eulerAngles +
				weapon.weaponTip.transform.rotation.eulerAngles;

				weapon.weaponTip.transform.forward = Player_Detection.position - weapon.weaponTip.transform.position;



				EnemyProjectileFML.EP_Call(weapon.currentWeapon, weapon.weaponTip.position, weapon.weaponTip.forward);
			}
	}
	private void Update()
	{
	}
}
