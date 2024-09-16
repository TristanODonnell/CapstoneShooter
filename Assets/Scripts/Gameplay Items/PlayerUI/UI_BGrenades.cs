using System;
using System.Collections;
using System.Collections.Generic;
using throwables;
using throwables.Items;
using UnityEngine;
using UnityEngine.UI;

public class UI_BGrenades : MonoBehaviour
{
    GrenadeManager manager;
    ThrowableItem grenade => manager.currentGrenade;
    int ammount { get {
            try
            {
                return manager.grenadeCounts[DataManager.Singleton.grenades.IndexOf(grenade)];
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
        if(manager == null)
        {
			B_TryLocateGrenadeManager();
            return;
        }
        if (grenade == null)
        {
            B_CancelIcon();
            return;
        }

		B_UpdateNumbers();
    }

	private void B_UpdateNumbers()
	{
        ui_Icon.sprite = grenade.icon;
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
        manager = FindObjectOfType<GrenadeManager>();
	}
}
