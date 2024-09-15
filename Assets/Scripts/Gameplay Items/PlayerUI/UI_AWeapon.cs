using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UI_AWeapon : MonoBehaviour
{
    ShootBehavior playerWeapon;

    [SerializeField] Image ui_weaponIcon;
    [SerializeField] Text ui_weaponMagazine;
    [SerializeField] Text ui_weaponAmmo;


    void Update()
    {
        if (playerWeapon == null)
        {
			A_TryLocateWeapon();
            A_CancelIcons();
            return;
        }

        if (playerWeapon.currentWeapon == null)
        {
            A_CancelIcons();
            return;
        }
		A_Update();
    }

	private void A_Update()
	{
        ui_weaponIcon.gameObject.SetActive(true);
        ui_weaponIcon.sprite = playerWeapon.currentWeapon.v_Icon;
        ui_weaponMagazine.text = playerWeapon.currentWeapon.currentMagazineAmmo + "";
        ui_weaponAmmo.text = playerWeapon.currentWeapon.totalAmmo + "";
	}

	private void A_TryLocateWeapon()
	{
        var player = FindObjectOfType<PlayerController>();
        if (player == null)
            return;

        playerWeapon = player.GetComponentInChildren<ShootBehavior>();
	}

    private void A_CancelIcons()
    {
        ui_weaponAmmo.text = ui_weaponMagazine.text = "";
        ui_weaponIcon.gameObject.SetActive(false);
    }
}
