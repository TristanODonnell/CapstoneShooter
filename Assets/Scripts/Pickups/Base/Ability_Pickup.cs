using Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Pickup : Pickup
{
	[SerializeField] private AbilityBase prefabAbility;
	[SerializeField] private SpriteRenderer icon;
	[SerializeField] private float iconDistance = 3f;
	private bool keyPressed;
	private Camera playerController;
	protected override bool TryPickUp(Collider other)
	{
		return false;
	}

	private void OnValidate()
	{
		try
		{
			icon.sprite = prefabAbility.icon;
		}
		catch { }
	}

	private void FixedUpdate()
	{
		if(playerIsInside && Input.GetKey(KeyCode.Return))
		{
			if (keyPressed) return;
			try
			{
				var player = FindObjectOfType<Player_AbilityBehaviour>();
				var sel = prefabAbility;

				AbilityBase[] allabilities =
				{
					player.abilityReferences.jetburst,
					player.abilityReferences.jetpack,
					player.abilityReferences.extraBatery,
					player.abilityReferences.dash,
					player.abilityReferences.teleporter,
					player.abilityReferences.groundPound
				};

				foreach (var item in allabilities)
					if (player.abilitySelected.icon == item.icon)
						prefabAbility = item;

				player.SetAbility(sel);

				icon.sprite = prefabAbility.icon;
				keyPressed = true;
			}
			catch { }
		}
		else
		{
			keyPressed = false;
		}

		if (!playerController)
		{
			playerController = FindObjectOfType<Camera>(false);
			return;
		}
		icon.sprite = prefabAbility.icon;
		icon.transform.position = transform.position + Vector3.up * iconDistance;
		icon.transform.forward = playerController.transform.position - icon.transform.position;
	}
}
