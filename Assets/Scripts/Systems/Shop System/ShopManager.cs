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
   
    public bool isShopOpen;

    public int grenadeRefillCost;
    public int abilityCost;

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
            ItemSpawner.instance.SpawnWeapon(purchasedWeaponData);
            CurrencyManager.singleton.SubtractCurrency(weaponItemCost);
        }
        else
        {
            UnityEngine.Debug.Log("Not enough currency to buy ");
        }
    }

    public void BuyAbility(int abilityIndex)
    {
        if (canAffordPurchase(abilityCost))
        {
           playerController = FindObjectOfType<PlayerController>();
            playerController.abilityBehaviour.SetAbility(DataManager.Singleton.abilities[abilityIndex]);
            CurrencyManager.singleton.SubtractCurrency(abilityCost);
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
            ItemSpawner.instance.SpawnPassive(purchasedPassiveData);
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

        if (canAffordPurchase(grenadeRefillCost))
        {
            GrenadeManager.instance.GrenadeRefill();
            CurrencyManager.singleton.SubtractCurrency(grenadeRefillCost);
        }
        else
        {
            UnityEngine.Debug.Log("Not enough currency to buy ");
        }
        
    }

    public void BuyAmmoRefill()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        int ammoRefillCost = DataManager.Singleton.weapons[0].AmmoRefillCost;
        if (canAffordPurchase(ammoRefillCost))
        {
            for (int i = 0; i < player.shoot.weapons.Count; i++)
            {
                player.shoot.SetUpWeaponAmmo(player.shoot.weapons[i]);
                player.shoot.weapons[i].totalAmmo = player.shoot.weapons[i].maxAmmo;
                CurrencyManager.singleton.SubtractCurrency(ammoRefillCost);
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
