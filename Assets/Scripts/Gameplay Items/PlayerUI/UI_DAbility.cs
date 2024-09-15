using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DAbility : MonoBehaviour
{
    [SerializeField] private Image ability;
    private Player_AbilityBehaviour player_ability;


    void Update()
	{
        if (player_ability == null)
        {
            ability.fillAmount = 0;
            D_LocatePlayer();
            return;
        }
        if (player_ability.abilitySelected == null)
        {
            ability.fillAmount = 0;
            return;
        }

		D_UpdateAbility();
}

	private void D_UpdateAbility()
	{
        var pability = player_ability.abilitySelected;
        ability.sprite = pability.icon;

        if(pability.icon == null)
        {
            ability.fillAmount = 0;
            return;
        }
        ability.fillAmount = Mathf.Clamp01(pability.Ability_Normalized());
	}

	private void D_LocatePlayer()
	{
        player_ability = FindObjectOfType<Player_AbilityBehaviour>();
	}
}
