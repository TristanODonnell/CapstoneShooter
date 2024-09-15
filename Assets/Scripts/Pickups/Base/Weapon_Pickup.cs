using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WeaponHolder))]
public class Weapon_Pickup : Pickup
{
	[SerializeField] private WeaponHolder weaponHolder;
	[SerializeField] GameObject weaponModel;

	protected override bool TryPickUp(Collider other)
	{
		var playerWeapon = other.GetComponent<ShootBehavior>();
		if (playerWeapon == null) return false;

		if (weaponHolder != null)
		{
			foreach (WeaponData weapon in playerWeapon.weapons)
			{
				if (weaponHolder.myweaponData == weapon)
				{
					int groundTotalAmmo = weaponHolder.groundTotalAmmo;

					while (groundTotalAmmo > 0 && weapon.totalAmmo < weapon.maxAmmo)
					{
						int ammoToTransfer = Mathf.Min(weapon.maxAmmo - weapon.totalAmmo, groundTotalAmmo);
						weapon.totalAmmo += ammoToTransfer;
						groundTotalAmmo -= ammoToTransfer;

						weaponHolder.groundTotalAmmo = groundTotalAmmo;
					}

					if (groundTotalAmmo <= 0)
						return true;
					break;
				}
			}
		}
		return false;
	}

}
