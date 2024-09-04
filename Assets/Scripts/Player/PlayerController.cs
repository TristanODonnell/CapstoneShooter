using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerData attachedPlayerData;







    [Header("Player Behavior")]
    [SerializeField] private LookBehavior look;
    [SerializeField] private JumpBehavior jump;
    [SerializeField] private MovementBehavior move;
    public ShootBehavior shoot;


    public GrenadeBehavior grenade;
    public GrenadeManager grenadeManager;

    public EquipmentBehavior equipment;
    
    public PassiveBehavior passive;
    
    [SerializeField] private Camera myCamera;
    [SerializeField] private LayerMask interactableFilter;
    public IInteractable selectedInteraction;

    [SerializeField] private ShopBehavior shop;
    private bool isShopOpen = false;

    public int totalPlayerCurrency; //TESTING

    // [SerializeField] WeaponInventory weaponInventory;
    

    void Start()
    {
        grenadeManager = GameManager.Singleton.GetComponent<GrenadeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMoveInput();
        CheckShootInput();
        CheckSprintInput();
        CheckGrenadeInput();
        
        CheckGravity();
        CheckJumpInput();
        CheckLookInput();
        CheckAimDownSightInput();
        CheckReloadInput();
        CheckGravity();
        ChangeWeaponInput();
        CheckShopInput();

        EquipmentInteract();
        WeaponInteract();
        PassiveInteract();
        CurrentGrenadeSelectInput();








        PickUpWeaponInputTest();
        DropWeaponInputTest();


        totalPlayerCurrency = CurrencyManager.singleton.totalCurrency;
    }

    public void AssignPlayerData(PlayerData playerData)
    {
        attachedPlayerData = playerData;
        for (int i = 0; i < 3; i++)
        {
            shoot.weapons.Add(playerData.playerWeaponData[i]);
        }
        equipment.playerEquipmentData1 = playerData.playerEquipmentData[0];
        equipment.playerEquipmentData2 = playerData.playerEquipmentData[1];
        passive.attachedPassive = playerData.playerPassiveData;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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


    private void PickUpWeaponInputTest()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Ray ray = new Ray(myCamera.transform.position, myCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2f)) // Adjust the distance (2f) as needed
            {
                GameObject hitObject = hit.collider.gameObject;

                // Check if the hit object has the "Weapon" tag or is a weapon prefab
                if (hitObject.CompareTag("Weapon"))
                {
                  //  shoot.PickUpWeapon(hitObject);

                    // Optionally, destroy the object in the world after picking it up
                    Destroy(hitObject);
                }
            }
        }
    }
    private void DropWeaponInputTest()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
          //  shoot.DropWeapon(shoot.currentWeaponIndex);

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
            Debug.Log("Player jumped");
            jump.JumpPlayer();
        }
    }

    private void CheckGravity()
    {
        jump.GravityCalculation();
        
    }

    private void CheckLookInput()
    {
        look.RotatePlayer();
    }

    private void CheckShootInput()
    {
        if(Input.GetMouseButtonDown(0)) //CLICKING TO SHOOT 
        {
          //  shoot.Shoot();

        }

        //MAY NEED ADDITIONAL INPUT FOR HOLDING DOWN TO SHOOT burst, or melee
    }    

    private void CheckAimDownSightInput()
    {
        if (Input.GetMouseButton(1) && shoot.isAimingDownSight == false)
        {
          //  shoot.AimDownSightStart();
        }
        if (Input.GetMouseButtonUp(1) && shoot.isAimingDownSight )
        {
          //  shoot.AimDownSightEnd();
        }
        

    }

    private void CheckGrenadeInput()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            //shoot.PlayerReload();
        }
    }

    
}
