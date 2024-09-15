using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLocation : MonoBehaviour, IInteractable

{
    public void Interact(PlayerController player, WeaponHolder weaponHolder)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(PlayerController player, EquipmentData equipmentData)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(PlayerController player, ShopLocation shopLocation)
    {
        if(ShopManager.singleton.isShopOpen == false)
        {
            ShopManager.singleton.OpenShop();
            Debug.Log("Shop opened");
        }
        
    }

    public void OnHoverEnter()
    {
        Debug.Log("Open Shop Here");
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
