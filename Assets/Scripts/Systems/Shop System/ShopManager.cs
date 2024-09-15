using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using static ShopManager;

public class ShopManager : MonoBehaviour
{
    private PlayerController playerController;
    public GameObject shopMenu;
    private PlayerController player;
    public static ShopManager singleton;
    public ItemSpawner itemSpawner;
    public bool isShopOpen;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
       
        //CurrencyManager.singleton.SetTotalCurrency(5000); //testing
    }
    private void Update()
    {
        CloseShop();
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

    public void BuyAbility(int equipmentIndex)
    {
        /*
        //do the selection via assigned from button and data manager list
       // EquipmentData purchasedEquipmentData = DataManager.Singleton.equipment[equipmentIndex];
        int abilityItemCost = purchasedEquipmentData.ItemCost;
        if (canAffordPurchase(equipmentItemCost))
        {
            itemSpawner.SpawnEquipment(purchasedEquipmentData);
            CurrencyManager.singleton.SubtractCurrency(equipmentItemCost);
        }
        else
        {
            UnityEngine.Debug.Log("Not enough currency to buy ");
        }
        */
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
    {/*
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
        */
    }

    public void BuyAmmoRefill()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        int ammoRefillCost = DataManager.Singleton.weapons[0].AmmoRefillCost;
        if (canAffordPurchase(ammoRefillCost))
        {
            for (int i = 0; i < player.shoot.weapons.Count; i++)
            {
                CurrencyManager.singleton.SubtractCurrency(ammoRefillCost);
                player.shoot.SetUpWeaponAmmo(player.shoot.weapons[i]); // Set up ammo for each weapon
            }
        }
        else
        {
            UnityEngine.Debug.Log("Not enough currency to buy ");
        }
    }

    public void BuySkillTreeTier(SkillTreeButton button)
    {
        
        SkillTreeButton skillTreeButton = button.GetComponent<SkillTreeButton>();
        if (ModifierManager.Singleton.IsMaxLevel(button.skillTreeType))
        {
            return;
        }
        int currentLevel = ModifierManager.Singleton.GetCurrentLevel(button.skillTreeType);
        if (skillTreeButton != null)
        {
            if(canAffordPurchase((int)skillTreeButton.itemCost))
            {
                CurrencyManager.singleton.SubtractCurrency((int)skillTreeButton.itemCost);
                ModifierManager.SkillTreeType currentType = skillTreeButton.skillTreeType;
                UnityEngine.Debug.Log("Buying skill tree tier: " + currentType);
                ModifierManager.Singleton.UpdateSkillTreeType(currentType);

                if (ModifierManager.Singleton.IsMaxLevel(currentType))
                {
                    skillTreeButton.DisableButton();
                }
            }
            else
            {
                UnityEngine.Debug.Log("Not enough currency to buy ");
                return;
            }
        }
    }

    
    private bool canAffordPurchase(int itemCost)
    {
        return CurrencyManager.singleton.totalCurrency >= itemCost;
    }

    public void OpenShop()
    {
        isShopOpen = true;
        shopMenu.SetActive(true); 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        //OPEN UI Shop here, ititialize anything
        //have buttons active here 
    }

    public void CloseShop()
    {
        if (isShopOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                shopMenu.SetActive(false);
                isShopOpen = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                
            }
        }
        

        //Disable shop, disable anything here 
    }

    

}
