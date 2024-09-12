using Abilities;
using gricel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.VFX;

public class Player_AbilityBehaviour : MonoBehaviour
{
	public CharacterController controller;
	public GravitationalBehaviour gravitation;

	[System.Serializable]
	public class AbilityReferences
	{
		public Ability_JetBurst jetburst;
		public Ability_Teleporter teleporter;
		public Ability_Dash dash;
		public Ability_GroundPound groundPound;
		public Ability_ExtraBatery extraBatery;
		public Ability_Jetpack jetpack;
	}
	public AbilityReferences abilityReferences = new();


	public Abilities.AbilityBase abilitySelected;
	public void SetAbility(AbilityBase ability)
	{
		if (abilitySelected)
			Destroy(abilitySelected.gameObject);
		abilitySelected = Abilities.AbilityBase.Ability_Set(ability, controller, gravitation);
	}

	// Update is called once per frame
	void Update()
    {
		if (abilitySelected == null) return;

		abilitySelected.Ability_Use();
    }
}