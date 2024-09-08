using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static WeaponData;



public class ShootBehavior : MonoBehaviour
{
    //  [SerializeField] ObjectPool bulletPool;
    public WeaponCameraSystem weaponCameraSystem;
    public List<WeaponData> weapons = new List<WeaponData>();
    public int currentWeaponIndex = 0;

    public WeaponData currentWeapon;
    public GameObject currentWeaponModel;
    public WeaponLogic currentWeaponLogic;
    public bool isReloading = false;
    public Transform weaponTip;
    public Transform weaponPosition;

    public ObjectPool currentObjectPool;
    public LookBehavior lookBehavior;
    void Start()
    {
        currentWeaponIndex = 0;
        ChangeWeapon(currentWeaponIndex);
        for (int i = 0; i < weapons.Count; i++)
        {
            SetUpWeaponAmmo(weapons[i]); // Set up ammo for each weapon
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void StartShooting()
    {
        
        currentWeaponLogic.StartShooting(weaponTip);
    } 
    public void Shooting()
    {
        currentWeaponLogic.Shooting(weaponTip);

    }

    public void StopShooting()
    {
        currentWeaponLogic.StopShooting(weaponTip);
    }

    public void Reloading()
    {
        currentWeaponLogic.ReloadLogic();
    }    
    public IEnumerator Reload(Action onReloadComplete)
    {
        
        int ammoNeeded = currentWeapon.magazineSize - currentWeapon.currentMagazineAmmo;
        ammoNeeded = Mathf.Max(ammoNeeded, 0);
        if (currentWeapon.totalAmmo >= ammoNeeded)
        {

            yield return new WaitForSeconds(currentWeapon.reloadTime);
            currentWeapon.totalAmmo -= ammoNeeded; // subtract ammo needed from total ammo
            currentWeapon.currentMagazineAmmo = currentWeapon.magazineSize;// fill magazine to capacity
            Debug.Log("Reloaded successfully. Total ammo: " + currentWeapon.totalAmmo + ", Magazine ammo: " + currentWeapon.currentMagazineAmmo);
        }
        else
        {
            yield return new WaitForSeconds(currentWeapon.reloadTime);
            int ammoToAdd = Mathf.Min(ammoNeeded, currentWeapon.totalAmmo);
            currentWeapon.currentMagazineAmmo += ammoToAdd;
            currentWeapon.totalAmmo -= ammoToAdd;
            if (currentWeapon.totalAmmo <= 0)
            {
                currentWeapon.totalAmmo = 0; // set total ammo to 0
            }
        }
        onReloadComplete?.Invoke();
        yield break;
    }

    public void ChangeWeapon(int index)
    {
        Debug.Log("Changing to weapon at index: " + index);
        index = Mathf.Clamp(index, 0, weapons.Count - 1);
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
        currentWeapon = weapons[index];
        
        currentWeaponModel = Instantiate(currentWeapon.GetWeaponModel(), weaponPosition.position, weaponPosition.rotation, weaponPosition.transform);
        currentWeaponModel.layer = LayerMask.NameToLayer("AttachedToPlayer");
        
        Debug.Log("my weapon is now" + currentWeapon.WeaponName);

        if (currentWeapon.weaponType == WeaponType.Projectile)
        {
            currentObjectPool = currentWeaponModel.GetComponent<ObjectPool>();
            PooledObject pooledObject = currentWeapon.GetProjectilePrefab().GetComponent<PooledObject>();
            currentObjectPool.InitializePool(pooledObject, currentWeapon.PoolSize);
        }
        currentWeaponLogic = GetWeaponLogic(currentWeapon.WeaponName);
    } 
    

    public void DropWeapon(int index)
    {
        if (index >= 0 && index < weapons.Count)
        {
            GameObject weaponToDrop = Instantiate(currentWeapon.GetWorldWeapon());
            weaponToDrop.transform.position = transform.position + transform.forward * 2f;
            weaponToDrop.SetActive(true);

            WeaponHolder weaponHolder = weaponToDrop.GetComponent<WeaponHolder>();
            weaponHolder.currentWorldWeapon = weaponToDrop;
            if (weaponHolder != null)
            {
                weaponHolder.groundTotalAmmo = currentWeapon.totalAmmo; // Set the ground ammo to the current weapon's ammo
            }
            if (currentWeaponModel != null)
            { 
                Destroy(currentWeaponModel);
            }
            currentWeapon = null;
            weapons.RemoveAt(index);

        }
        else
            {
                Debug.LogWarning("Weapon not found or invalid index.");
            }
    }
    public void PickUpWeapon(WeaponHolder weaponHolder)
    {
        
        bool isDifferentWeapon = true;
        foreach (WeaponData weapon in weapons)
        {
            if (weaponHolder.myweaponData == weapon)
            {
                isDifferentWeapon = false; // Found a matching weapon
                Debug.Log("You already have this weapon.");
                return; // Exit the method since the weapon is the same
            }
        }
        if (isDifferentWeapon)
        {
            int groundTotalAmmo = weaponHolder.groundTotalAmmo;
            weaponHolder.myweaponData.totalAmmo += groundTotalAmmo;
            weaponHolder.myweaponData.currentMagazineAmmo = weaponHolder.myweaponData.magazineSize;
            weapons.Add(weaponHolder.myweaponData);

            currentWeaponIndex = weapons.Count - 1;
            currentWeapon = weaponHolder.myweaponData;
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
            currentWeaponModel = Instantiate(currentWeapon.GetWeaponModel(), weaponPosition.position, weaponPosition.rotation, weaponPosition.transform);
            currentWeaponLogic = GetWeaponLogic(currentWeapon.WeaponName);
        }
       
    }
    public void NextWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        ChangeWeapon(currentWeaponIndex);
    }
    public void PreviousWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex - 1 + weapons.Count) % weapons.Count;
        ChangeWeapon(currentWeaponIndex);
    }

    public void SetUpWeaponAmmo(WeaponData weapon)
    {
        weapon.totalAmmo = weapon.maxAmmo;
        weapon.currentMagazineAmmo = weapon.magazineSize;
    }
    private WeaponLogic GetWeaponLogic(string weaponName)
    {
        switch (weaponName)
        {
            case "Marksman Rifle":
                return new SemiAutoWeapon(this, currentWeapon, currentObjectPool, DamageType.Shrapnel);
            case "Hitscan Sniper Test":
                return new SemiAutoWeapon(this, currentWeapon, currentObjectPool, DamageType.Energy);
            case "SMG":
                return new AutomaticWeapon(this, currentWeapon, currentObjectPool, DamageType.Stun);
            // ...
            default:
                throw new ArgumentException("Unknown weapon name", nameof(weaponName));
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
        WeaponHolder weaponHolder = other.GetComponent<WeaponHolder>();
        if (weaponHolder != null )
        {
            foreach(WeaponData weapon in weapons) 
            {
                if (weaponHolder.myweaponData == weapon)
                {
                    Debug.Log("same weapon triggered");
                    int groundTotalAmmo = weaponHolder.groundTotalAmmo;

                    while (groundTotalAmmo > 0 && weapon.totalAmmo < weapon.maxAmmo)
                    {
                        int ammoToTransfer = Mathf.Min(weapon.maxAmmo - weapon.totalAmmo, groundTotalAmmo);
                        weapon.totalAmmo += ammoToTransfer;
                        groundTotalAmmo -= ammoToTransfer;

                        weaponHolder.groundTotalAmmo = groundTotalAmmo;
                    }

                    if (groundTotalAmmo <= 0)
                    {
                        Destroy(other.gameObject);
                    }
                    break;
                }
            }
        }
    }
}
