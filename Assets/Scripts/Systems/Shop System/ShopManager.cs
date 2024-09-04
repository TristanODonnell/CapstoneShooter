using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static ShopManager;

public class ShopManager : MonoBehaviour
{
    private PlayerController player;
    public static ShopManager singleton;
    public ItemSpawner itemSpawner;
    public bool isShopOpen;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        CurrencyManager.singleton.SetTotalCurrency(5000); //testing
    }
     
    public void BuyWeapon(int weaponIndex)
    {
        //do the selection via assigned from button and data manager list
        WeaponData purchasedWeaponData = DataManager.Singleton.weapons[weaponIndex];
        int weaponItemCost = purchasedWeaponData.ItemCost;
        if (canAffordPurchase(weaponItemCost))
        {
            itemSpawner.SpawnWeapon(purchasedWeaponData);
            CurrencyManager.singleton.SubtractCurrency(weaponItemCost);
        }
        else
        {
            UnityEngine.Debug.Log("Not enough currency to buy ");
        }
    }

    public void BuyEquipment(int equipmentIndex)
    {
        //do the selection via assigned from button and data manager list
        EquipmentData purchasedEquipmentData = DataManager.Singleton.equipment[equipmentIndex];
        int equipmentItemCost = purchasedEquipmentData.ItemCost;
        if (canAffordPurchase(equipmentItemCost))
        {
            itemSpawner.SpawnEquipment(purchasedEquipmentData);
            CurrencyManager.singleton.SubtractCurrency(equipmentItemCost);
        }
        else
        {
            UnityEngine.Debug.Log("Not enough currency to buy ");
        }
    }

    public void BuyPassive(int passiveIndex)
    {
        //do the selection via assigned from button and data manager list
        PassiveData purchasedPassiveData = DataManager.Singleton.passive[passiveIndex];
        int passiveItemCost = purchasedPassiveData.ItemCost;
        if (canAffordPurchase(passiveItemCost))
        {
            itemSpawner.SpawnPassive(purchasedPassiveData);
            CurrencyManager.singleton.SubtractCurrency(passiveItemCost);
        }
        else
        {
            UnityEngine.Debug.Log("Not enough currency to buy ");
        }
    }
    public int totalGrenades = 6;
    public void BuyGrenadeRefill()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        int grenadeItemCost = DataManager.Singleton.grenades[0].ItemCost;
        int refillCost = grenadeItemCost * totalGrenades;
        if (canAffordPurchase(refillCost))
        {
            GameManager.Singleton.grenadeManager.GrenadeRefill();
            CurrencyManager.singleton.SubtractCurrency(refillCost);
        }
        else
        {
            UnityEngine.Debug.Log("Not enough currency to buy ");
        }
    }

    private bool canAffordPurchase(int itemCost)
    {
        int playerCurrency = CurrencyManager.singleton.totalCurrency;

        return playerCurrency >= itemCost;
    }

    public void OpenShop()
    {
        isShopOpen = true;
        //OPEN UI Shop here, ititialize anything
        //have buttons active here 
    }

    public void CloseShop()
    {
        isShopOpen = false;
        //Disable shop, disable anything here 
    }


}
