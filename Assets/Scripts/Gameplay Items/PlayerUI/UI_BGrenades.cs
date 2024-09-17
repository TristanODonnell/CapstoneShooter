using System;
using System.Collections;
using System.Collections.Generic;
using throwables;
using throwables.Items;
using UnityEngine;
using UnityEngine.UI;

public class UI_BGrenades : MonoBehaviour
{
    Player_GrenadeThrow player;

    int ammount { get {
            try
            {
                return player.g_currentAmmount;
            }
            catch {
                return 0;
            }
       }
    }
    [SerializeField] private Image ui_Icon;
    [SerializeField] private Text ui_Ammount;


    void Update()
    {
        if(player == null)
        {
			B_TryLocateGrenadeManager();
            return;
        }

		B_UpdateNumbers();
    }

	private void B_UpdateNumbers()
	{
        ui_Icon.sprite = player.g_currentIcon;
        ui_Ammount.text = "" + ammount;
        ui_Icon.gameObject.SetActive(true);

	}

	private void B_CancelIcon()
	{
        ui_Ammount.text = "";
        ui_Icon.gameObject.SetActive(false);
	}

	private void B_TryLocateGrenadeManager()
	{
        B_CancelIcon();
        player = FindObjectOfType<Player_GrenadeThrow>();
	}
}
