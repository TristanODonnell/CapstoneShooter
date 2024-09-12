using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentHolder : MonoBehaviour,  IInteractable
{
    public EquipmentData myequipmentData;


    public void Interact(PlayerController player, WeaponHolder weaponHolder)
    {

    }

    public void Interact(PlayerController player, EquipmentData equipmentData)
    {

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            //player.equipment.ReplaceEquipment(0, equipmentData);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
           // player.equipment.ReplaceEquipment(1, equipmentData);
        }


        // player.equipmentSwapPrompt.replaceEquipment1Button.gameObject.SetActive(true);
        // player.equipmentSwapPrompt.replaceEquipment2Button.gameObject.SetActive(true);

    }

    public void OnHoverEnter()
    {
        Debug.Log("equipment pickup available");
    }

    public void OnHoverExit()
    {

    }

    public virtual void UseEquipment()
    {

    }
}
