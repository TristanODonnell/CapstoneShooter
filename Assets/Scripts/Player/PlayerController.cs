using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using gricel;
using Unity.VisualScripting;
using UnityEngine;
using static gricel.HealthSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Initialization")]
    public PlayerData attachedPlayerData;


    [Header("Player Health")]
    public HealthSystem healthSystem;
    public Hitbox hitBox;


    [Header("Player Behavior")]
    public LookBehavior look;
    public MovementBehavior move;
    public ShootBehavior shoot;
    public PassiveBehavior passive;
    public RecursiveBashBehavior recursiveBashBehavior;
    public GravitationalBehaviour gravitational;
    public float jumpForce;

    public GrenadeManager grenadeManager;
    public GrenadeBehavior grenade;
    public Player_AbilityBehaviour abilityBehaviour;

    [Header("Camera/Interactions")]
    [SerializeField] private Camera myCamera;
    [SerializeField] private LayerMask interactableFilter;
    public IInteractable selectedInteraction;

    [Header("Shop Settings")]
    [SerializeField] private ShopBehavior shop;
    private bool isShopOpen = false;
    public int totalPlayerCurrency;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Start()
    {
        grenadeManager = GrenadeManager.instance;

    }

    public void ModifierInputTest()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            SetPlayerHealthModifier();
            SetPlayerJumpModifier();
        }
    }
    public void SetJumpPassiveModifier(float jumpModifier)
    {
        float modifiedJump = jumpModifier * jumpForce;
        jumpForce = modifiedJump;
    }
    public void SetPlayerJumpModifier()
    {
        int currentJumpLevel = ModifierManager.Singleton.currentJumpHeightLevel;

        float jumpModifier = ModifierManager.Singleton.jumpingHeightModifiers[currentJumpLevel - 1];

        float modifiedJumpForce = jumpForce * jumpModifier;
        jumpForce = modifiedJumpForce;
    }
    public void SetHealthPassiveModifier(float armorHealthPercentage, float armorModifier, float fleshHealthPercentage, float fleshModifier, float energyHealthPercentage, float energyShieldModifier, float totalHealthMultiplier = 200f)
    {
        if (healthSystem == null || healthSystem.healthBars == null || healthSystem.healthBars.Length == 0)
        {
            Debug.LogError("healthSystem or healthBars is not ready!");
            return;
        }
        if (armorHealthPercentage + fleshHealthPercentage + energyHealthPercentage != 1.0f)
        {
            Debug.LogError("Health percentages do not add up to 100%");
            return;
        }

        float totalHealth = 0f;
        foreach (var health in healthSystem.healthBars)
        {
            totalHealth += health.health_Max;
        }
        totalHealth *= totalHealthMultiplier;
        float armorHealth = totalHealth * armorHealthPercentage * armorModifier;
        float fleshHealth = totalHealth * fleshHealthPercentage * fleshModifier;
        float energyHealth = totalHealth * energyHealthPercentage * energyShieldModifier;
        foreach (var health in healthSystem.healthBars)
        {
            switch (health.protection)
            {
                case Health.HealthT.unprotected:
                    health.health_Max = fleshHealth;
                    break;
                case Health.HealthT.armor:
                    health.health_Max = armorHealth;
                    break;
                case Health.HealthT.energyShield:
                    health.health_Max = energyHealth;
                    break;
            }
        }
    }
    public void SetPlayerHealthModifier() // -- worked on input but not Start 
    {
        if (healthSystem == null || healthSystem.healthBars == null || healthSystem.healthBars.Length == 0)
        {
            Debug.LogError("healthSystem or healthBars is not ready!");
            return;
        }
        int currentArmorLevel = ModifierManager.Singleton.currentArmorLevel;
        int currentFleshLevel = ModifierManager.Singleton.currentFleshLevel;
        int currentEnergyShieldLevel = ModifierManager.Singleton.currentEnergyShieldLevel;

        float armorModifier = ModifierManager.Singleton.armorModifiers[currentArmorLevel - 1];
        float fleshModifier = ModifierManager.Singleton.fleshModifiers[currentFleshLevel - 1];
        float energyShieldModifier = ModifierManager.Singleton.energyShieldModifiers[currentEnergyShieldLevel - 1];
        Debug.Log("Modifiers: armor=" + armorModifier + ", flesh=" + fleshModifier + ", energyShield=" + energyShieldModifier);
        foreach (var health in healthSystem.healthBars)
        {
            Debug.Log("Before modification: health_Max = " + health.health_Max);
            switch (health.protection)
            {
                case Health.HealthT.unprotected:
                    health.health_Max *= fleshModifier;
                    break;
                case Health.HealthT.energyShield:
                    health.health_Max *= energyShieldModifier;
                    break;
                case Health.HealthT.armor:
                    health.health_Max *= armorModifier;
                    break;
            }
            Debug.Log("After modification: health_Max = " + health.health_Max);
        }
    }
    // Update is called once per frame
    void Update()
    {
        ModifierInputTest();
        CheckMoveInput();
        CheckShootInput();
        CheckSprintInput();
        CheckGrenadeInput();
        CheckRecursiveBashInput();
        
        CheckJumpInput();
        CheckLookInput();
        CheckAimDownSightInput();
        CheckReloadInput();
        
        ChangeWeaponInput();
        CheckShopInput();

//EquipmentInteract();
        WeaponInteract();
        PassiveInteract();
        CurrentGrenadeSelectInput();


        totalPlayerCurrency = CurrencyManager.singleton.totalCurrency;
    }

    public void AssignPlayerData(PlayerData playerData)
    {
        Debug.Log("AssignPlayerData called");
        Debug.Log("playerData.playerWeaponData: " + playerData.playerWeaponData);
        Debug.Log("playerData.playerWeaponData.Count: " + playerData.playerWeaponData.Count);
        attachedPlayerData = playerData;
        //initialize passive onto passivebehavior
        passive.attachedPassive = playerData.playerPassiveData;
        //apply the passive
        PassiveAttribute currentPassiveAttribute = passive.GetCurrentPassiveAttribute();
        passive.ApplyPassiveEffects(currentPassiveAttribute);
        Debug.Log("Current passive attribute: " + currentPassiveAttribute.GetType().Name);
        //initializing chosen player weapons
        for (int i = 0; i < playerData.playerWeaponData.Count; i++)
        {
            WeaponData clonedWeapon = Instantiate(playerData.playerWeaponData[i]);
            shoot.SetUpWeaponAmmo(clonedWeapon);
            shoot.SetUpWeaponDamage(clonedWeapon);
            clonedWeapon.totalAmmo = clonedWeapon.maxAmmo; 
            shoot.weapons.Add(clonedWeapon);
        }
        shoot.ChangeWeapon(0);
        //initialize chosen Ability Base onto behavior
        abilityBehaviour.SetAbility(playerData.playerAbilityReference);
        
    } 

    private void CheckRecursiveBashInput()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            recursiveBashBehavior.UseRecursiveBash();
        }
    }
    private void PassiveInteract()
    {
        {
            Ray ray = new Ray(myCamera.transform.position, myCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2f, interactableFilter))
            {
                PassiveHolder passiveHolder = hit.collider.gameObject.GetComponent<PassiveHolder>();
                if (passiveHolder != null)
                {
                    IInteractable interactable = passiveHolder.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        selectedInteraction = interactable;
                        selectedInteraction.OnHoverEnter();
                    }


                    if (Input.GetKeyDown(KeyCode.F))
                    {

                        {
                            if (selectedInteraction != null)
                            {
                                selectedInteraction.Interact(this, passiveHolder.mypassiveData);
                                Destroy(passiveHolder.gameObject);
                            }

                        }

                    }

                }

            }
            else if (selectedInteraction != null)
            {
                selectedInteraction.OnHoverExit();
                selectedInteraction = null;
            }
        }
    }
    /*
    private void EquipmentInteract()

    {
        Ray ray = new Ray(myCamera.transform.position, myCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f, interactableFilter))
        {
            EquipmentHolder equipmentHolder = hit.collider.gameObject.GetComponent<EquipmentHolder>();
            if (equipmentHolder != null)
            {
                IInteractable interactable = equipmentHolder.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    selectedInteraction = interactable;
                    selectedInteraction.OnHoverEnter();
                }


                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                     
                    {
                        if (selectedInteraction != null)
                        {
                            selectedInteraction.Interact(this, equipmentHolder.myequipmentData);
                            Destroy(equipmentHolder.gameObject);
                        }

                    }

                }
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {

                        if (selectedInteraction != null)
                        {
                            selectedInteraction.Interact(this, equipmentHolder.myequipmentData);
                            Destroy(equipmentHolder.gameObject);
                        }
                }

            }

        }
        else if (selectedInteraction != null)
        {
            selectedInteraction.OnHoverExit();
            selectedInteraction = null;
        }
    }
    */
    private void WeaponInteract()
    {
        Ray ray = new Ray(myCamera.transform.position, myCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f, interactableFilter))
        {
            WeaponHolder weaponHolder = hit.collider.gameObject.GetComponent<WeaponHolder>();
            if (weaponHolder != null)
            {
                WeaponData weaponData = weaponHolder.myweaponData;
                if (weaponData != null)
                {
                    IInteractable interactable = weaponData as IInteractable;
                    if(interactable != null)
                    {
                        selectedInteraction = interactable;
                        selectedInteraction.OnHoverEnter();
                    }
                } 
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (hit.collider.GetComponent<WeaponHolder>() is WeaponHolder interactedWeaponHolder)
                {
                    if (selectedInteraction != null)
                    {
                        selectedInteraction.Interact(this, interactedWeaponHolder);
                        Destroy(weaponHolder.currentWorldWeapon);
                    }
                        
                }
                
            }

        }
        else if (selectedInteraction != null)
        {
            selectedInteraction.OnHoverExit();
            selectedInteraction = null;
        }

    }

    private void CheckShopInput()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            shop.OpenShopMenu();
            isShopOpen = true;
            //Debug.Log("shop opened");
        }
        else 
        {
            shop.CloseShopMenu();
            isShopOpen = false;
            //Debug.Log("shop closed");
        }


    }

    private void ChangeWeaponInput()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            shoot.NextWeapon();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            shoot.PreviousWeapon();
        }
    }





    private void CheckMoveInput()
    {
        move.MovePlayer();
    }

    private void CheckSprintInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            move.SetSprinting(true);
        }
        else
        {
            move.SetSprinting(false);
        }
    }


    private void CheckJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var rc = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, 1.1f);
            var canJump = false;
            foreach (var item in rc)
            {
                if(!item.collider.GetComponentInParent<PlayerController>())
                {
                    canJump = true;
                    break;
                }
            }
            if(canJump)
                gravitational.Jump(jumpForce);
        }
    }

    private void CheckLookInput()
    {
        look.RotatePlayer();
    }
    private bool isShooting = false;

    private void CheckShootInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shoot.StartShooting();
            isShooting = true;
        }
        if (Input.GetMouseButton(0) && isShooting)
        {
            if (!Input.GetMouseButtonDown(0)) // add this check
            {
                shoot.Shooting();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            shoot.StopShooting();
            isShooting = false;
        }
    }

    private void CheckAimDownSightInput()
    {
        if (Input.GetMouseButton(1) && !look.IsAimingDownSight())
        {
            look.AimDownSightStart();
        }
        if (Input.GetMouseButtonUp(1) && look.IsAimingDownSight())
        {
            look.AimDownSightEnd();
        }
    }
    
    private void CheckGrenadeInput()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("G key pressed!");
            grenadeManager.ThrowGrenade();
        }

    }

    private void CurrentGrenadeSelectInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            grenadeManager.currentIndex = 0;
            grenadeManager.CurrentGrenadeSelect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            grenadeManager.currentIndex = 1;
            grenadeManager.CurrentGrenadeSelect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            grenadeManager.currentIndex = 2;
            grenadeManager.CurrentGrenadeSelect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            grenadeManager.currentIndex = 3;
            grenadeManager.CurrentGrenadeSelect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            grenadeManager.currentIndex = 4;
            grenadeManager.CurrentGrenadeSelect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            grenadeManager.currentIndex = 5;
            grenadeManager.CurrentGrenadeSelect();
        }
    }
    private void CheckReloadInput()
    {
        if (Input.GetKeyDown(KeyCode.R ))
        {
            if (!shoot.isReloading)
            {
                Debug.Log("Reload key pressed!");
                shoot.StopShooting();
                Debug.Log("Stopping shooting and starting reload...");
                shoot.Reloading();
                Debug.Log("Reload coroutine started.");
            }
            
        }
    }

    
}
