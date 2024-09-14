using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveHolder : MonoBehaviour, IInteractable
{
    public PassiveData mypassiveData;

    public void Interact(PlayerController player, WeaponHolder weaponHolder)
    {
        
    } 

    public void Interact(PlayerController player, EquipmentData equipmentData)
    {
        
    }
    public void Interact(PlayerController player, PassiveData passiveData)
    {
        player.passive.SwapPassive(passiveData);
        
    }

    public void OnHoverEnter()
    {
        Debug.Log("Passive Pickup Available");
    }

    public void OnHoverExit()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
