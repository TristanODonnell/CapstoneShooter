using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_FCrosshair : MonoBehaviour
{
    [SerializeField] private Image ui_Scope;
    [SerializeField] private Image ui_Crosshair;

    private ShootBehavior player_shootBehaviour;

	void PlayerDoesNotWork()
	{
		ui_Crosshair.gameObject.SetActive(false);
		ui_Scope.gameObject.SetActive(false);
		
		var player = FindObjectOfType<PlayerController>();
		if (player != null)
			player_shootBehaviour = player.GetComponentInChildren<ShootBehavior>();
	}
	void PlayerFight()
	{
		ui_Scope.gameObject.SetActive(Input.GetMouseButton(1));
		ui_Crosshair.gameObject.SetActive(true);
		ui_Crosshair.sprite = player_shootBehaviour.currentWeapon.v_crosshair;
	}

    void Update()
	{
		if (player_shootBehaviour == null)
		{
			PlayerDoesNotWork();
			return;
		}
		if(player_shootBehaviour.currentWeapon == null)
		{
			PlayerDoesNotWork();
			return;
		}
		PlayerFight();
	}
}
