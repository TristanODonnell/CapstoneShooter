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
	private bool hasStarted = false;
	private enum StartAbility
	{
		nothing,
		jetburst,
		teleporter,
		dash,
		groundpound,
		extraBatery,
		jetpack
	}
	[SerializeField] private StartAbility startAbility;

	private void Start()
	{
		if (hasStarted)
			return;
		var ability = abilitySelected;
		switch (startAbility)
		{
			case StartAbility.nothing:
				ability = null;
				break;
			case StartAbility.jetburst:
				ability = abilityReferences.jetburst;
				break;
			case StartAbility.teleporter:
				ability = abilityReferences.teleporter;
				break;
			case StartAbility.dash:
				ability = abilityReferences.dash;
				break;
			case StartAbility.groundpound:
				ability = abilityReferences.groundPound;
				break;
			case StartAbility.extraBatery:
				ability = abilityReferences.extraBatery;
				break;
			case StartAbility.jetpack:
				ability = abilityReferences.jetpack;
				break;
		}
		if (ability)
			SetAbility(ability);
	}

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


	public Abilities.AbilityBase abilitySelected { get; private set; }
	public void SetAbility(AbilityBase ability)
	{
		hasStarted = true;
		if (abilitySelected)
			Destroy(abilitySelected.gameObject);
		abilitySelected = Abilities.AbilityBase.Ability_Set(ability, controller, gravitation);
	}

	public float Ability_NormalCooldown()
	{
		if (abilitySelected)
			return abilitySelected.Ability_Normalized();
		return 0;
	}


	// Update is called once per frame
	void Update()
    {
		if (abilitySelected == null) return;

		abilitySelected.Ability_Use();
    }
}