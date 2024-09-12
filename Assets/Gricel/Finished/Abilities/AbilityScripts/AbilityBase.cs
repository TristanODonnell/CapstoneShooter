using UnityEngine;


namespace Abilities
{
	public abstract class AbilityBase : MonoBehaviour
	{
		protected CharacterController controller;
		protected GravitationalBehaviour gravitation;
		[SerializeField] private KeyCode button;

		public static AbilityBase Ability_Set(AbilityBase reference, CharacterController controller, GravitationalBehaviour gravitation)
		{
			var ability = Instantiate(reference);
			ability.controller = controller;
			ability.gravitation = gravitation;
			ability.transform.position = controller.transform.position;
			ability.transform.rotation = controller.transform.rotation;
			ability.transform.parent = controller.transform.parent;
			return ability;
		}
		public void AbilityCustomizeKey(KeyCode key) => button = key;

		public void Ability_Use()
		{
			if (CanBeUsed())
			{
				if (Input.GetKey(button))
					UseHold();
				if (Input.GetKeyDown(button))
					UsePress();
			}
		}


		public abstract void Ability_CooldownOverride(float multiplier = 1f);
		protected abstract bool CanBeUsed();
		protected virtual void UseHold(){}
		protected virtual void UsePress() { }
	}
}