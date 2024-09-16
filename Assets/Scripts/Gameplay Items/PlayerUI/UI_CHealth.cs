using gricel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CHealth : MonoBehaviour
{
    [SerializeField] private Image[] ui_healthbars;
    private HealthSystem playerHealth;

	[SerializeField]
	private Color
		uiC_EnergyShield,
		uiC_Flesh,
		uiC_Armor;



    void Update()
    {
		if (playerHealth == null)
		{
			C_TryLocatePlayer(); 
			return ;
		}
        C_UpdateHealthBars();
    }

	private void C_UpdateHealthBars()
	{
		for (int i = 0; i < ui_healthbars.Length; i++)
		{
			if (playerHealth.healthBars.Length <= i)
				C_HealthbarFailed(i);
			else
				C_HealhbarUpdate(i);
		}
	}

	private void C_HealhbarUpdate(int i)
	{
		var uih = ui_healthbars[i];
		var phs = playerHealth.healthBars[i];

		uih.gameObject.SetActive(true);

		var vectorGrowth = Vector3.one;
		vectorGrowth.x = 0;
		var normalHealth = 0f;
		if (phs.health_Max > 0)
			normalHealth = Mathf.Clamp01(phs.health / phs.health_Max);

		switch (phs.protection)
		{
			case HealthSystem.Health.HealthT.unprotected:
				uih.color = uiC_Flesh;
				break;
			case HealthSystem.Health.HealthT.energyShield:
				uih.color = uiC_EnergyShield;
				break;
			case HealthSystem.Health.HealthT.armor:
				uih.color = uiC_Armor;
				break;
		}

		uih.transform.localScale = vectorGrowth + Vector3.right * normalHealth;
	}

	private void C_HealthbarFailed(int i)
	{
		ui_healthbars[i].gameObject.SetActive(false);
	}



	private void C_TryLocatePlayer()
	{

		var player = FindObjectOfType<PlayerController>();
		if(player!=null)
			playerHealth = player.GetComponentInChildren<HealthSystem>();

		foreach (var item in ui_healthbars)
			item.gameObject.SetActive(false);
	}
}
