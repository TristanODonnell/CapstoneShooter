using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(WeaponHolder))]
public class Weapon_Pickup : Pickup
{
	[SerializeField] private WeaponHolder weaponHolder;
	[SerializeField] GameObject weaponModel;
	private ShootBehavior player;
	private bool wasPressed;
	private bool playerHasThisWeapon;
	protected override bool TryPickUp(Collider other)
	{
		if (player == null)
		{
			player = other.GetComponent<PlayerController>().GetComponent<ShootBehavior>();
		}

		if (weaponHolder != null)
		{
			foreach (WeaponData weapon in player.weapons)
			{
				if (weaponHolder.myweaponData == weapon)
				{
					playerHasThisWeapon = true;
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


	private void FixedUpdate()
	{
		if (playerIsInside && player && Input.GetKey(KeyCode.F))
		{
			if (wasPressed) return;
			wasPressed = true;
			try
			{
				if (player.weapons.Contains(weaponHolder.myweaponData)) return;
				var oldPlayerData = player.currentWeapon;

				var destroyForever = true;
				if (player.weapons.Count >= 3 && oldPlayerData)
				{
					weaponModel.GetComponent<MeshRenderer>().material = player.currentWeapon.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
					weaponModel.GetComponent<MeshFilter>().mesh = player.currentWeapon.weaponModel.GetComponent<MeshFilter>().sharedMesh;
					destroyForever = false;
					player.weapons.RemoveAt(player.currentWeaponIndex);
				}

				if(destroyForever)
					player.PickUpWeapon(weaponHolder);
				if(!destroyForever)
					player.ChangeWeapon(player.weapons.IndexOf(weaponHolder.myweaponData));
				weaponHolder.myweaponData = oldPlayerData;

				if (destroyForever)
				{
					player.currentWeapon.currentMagazineAmmo = player.currentWeapon.originalMagazineSize;
					player.currentWeapon.totalAmmo = player.currentWeapon.originalMagazineSize;
					Destroy(gameObject);
				}
			}
			catch { }
		}
		else
			wasPressed = false;
	}
}
