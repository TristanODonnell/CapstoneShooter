using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{
    public void Interact(PlayerController player, WeaponHolder weaponHolder);

    public void Interact(PlayerController player, EquipmentData equipmentData);

    public void Interact(PlayerController player, PassiveData passiveData)
    {
        
    }

    public void Interact(PlayerController player, ShopLocation shopLocation);
    
    public void OnHoverEnter();

    public void OnHoverExit();
}
