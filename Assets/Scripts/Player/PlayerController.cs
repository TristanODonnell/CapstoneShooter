using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Behavior")]
    [SerializeField] private LookBehavior look;
    [SerializeField] private JumpBehavior jump;
    [SerializeField] private MovementBehavior move;
    public ShootBehavior shoot;
    [SerializeField] private GrenadeBehavior grenade;
    [SerializeField] private EquipmentBehavior equipment;
    [SerializeField] private PassiveBehavior passive;

    [SerializeField] private Camera myCamera;
    [SerializeField] private LayerMask interactableFilter;
    public IInteractable selectedInteraction;

    [SerializeField] private ShopBehavior shop;
    private bool isShopOpen = false;

    public int totalPlayerCurrency; //TESTING

    // [SerializeField] WeaponInventory weaponInventory;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMoveInput();
        CheckShootInput();
        CheckSprintInput();
        CheckGrenadeInput();
        CheckEquipmentInput(0);
        CheckEquipmentInput(1);
        CheckGravity();
        CheckJumpInput();
        CheckLookInput();
        CheckAimDownSightInput();
        CheckReloadInput();
        CheckGravity();
        ChangeWeaponInput();
        CheckShopInput();


        WeaponInteract();

        PickUpWeaponInputTest();
        DropWeaponInputTest();


        totalPlayerCurrency = CurrencyManager.singleton.totalCurrency;
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
                //else if (hit.collider.GetComponent<object>() is EquipmentData equipmentData)
               // { 

              //  }
                /*
                if (selectedInteraction != null)
                {
                    WeaponData weaponData = hit.collider.GetComponent<WeaponData>();

                    if (weaponData != null)
                    {
                        selectedInteraction.Interact(this, weaponData);
                    }
                    selectedInteraction.Interact(this, weaponData);
                }
                */
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
            grenade.ThrowGrenade();
        }
    }

    private void CheckReloadInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //shoot.PlayerReload();
        }
    }

    private void CheckEquipmentInput(int slotnumber)
    {
        if(slotnumber == 0 && Input.GetKeyDown(KeyCode.E))
        {
            equipment.UseEquipment(0);
            Debug.Log("using equipment 1");
        }
        else if (slotnumber == 1 && Input.GetKeyDown(KeyCode.Q))
        {
            
            equipment.UseEquipment(1);
            Debug.Log("using equipment 2");
        }
    }
}
