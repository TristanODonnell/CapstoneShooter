using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using gricel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static WeaponData;



public class ShootBehavior : MonoBehaviour
{
    private GameObject riotShieldColliderInstance;


    public Transform weaponTip;
    public Transform weaponPosition;
    public bool isReloading = false;
    public List<WeaponData> weapons = new List<WeaponData>();
    public LookBehavior lookBehavior;
    [Header("Current Weapon Information")]
    public int currentWeaponIndex = 0;
    public WeaponData currentWeapon;
    public GameObject currentWeaponModel;
    public WeaponLogic currentWeaponLogic;

    private float weaponDamagePassive = 1f;
    private float weaponReloadPassive = 1f;
    private float weaponMagazinePassive = 1f;
    private float weaponMaxAmmoPassive = 1f;
    void Start()
    {


    }

    private WeaponLogic GetWeaponLogic(string weaponName)
    {
        switch (weaponName)
        {
            case "Marksman Rifle":
            case "Sniper":
            case "Blazing Falcon":
            case "Grenade Launcher":
            case "PumpShotgun":
            case "RocketLauncher":
                return new SemiAutoWeapon(this, currentWeapon, currentWeapon.ObjectPool, gameObject);
            case "SMG":
            case "Assault Rifle":
            case "BarrelShotgunx2":
            case "Laser":
            case "Machine Revolver":
            case "Minigun":
                return new AutomaticWeapon(this, currentWeapon, currentWeapon.ObjectPool, gameObject);
            case "Light Dagger":
            case "Machete":
            case "ShovelAxe":
                return new MeleeWeapon(this, currentWeapon, currentWeapon.ObjectPool, gameObject);
            case "Chainsaw":
                return new AutomaticMeleeWeapon(this, currentWeapon, currentWeapon.ObjectPool, gameObject);
            case "Burst Cannon":
                return new BurstWeapon(this, currentWeapon, currentWeapon.ObjectPool, gameObject);
            case "Railgun":
                return new RailGunWeapon(this, currentWeapon, currentWeapon.ObjectPool, gameObject);
            case "Player Riot Shield":
                return new RiotShieldWeapon(this, currentWeapon, currentWeapon.ObjectPool, gameObject);


            default:
                throw new ArgumentException("Unknown weapon name", nameof(weaponName));
        }
    }


    public void SetUpWeaponAmmo(WeaponData weapon)
    {
        Debug.Log("Setting up ammo for: " + weapon.WeaponName);

        int currentReloadLevel = ModifierManager.Singleton.currentReloadLevel;
        int currentMaxAmmoLevel = ModifierManager.Singleton.currentmaxAmmoLevel;
        int currentWeaponMagazineLevel = ModifierManager.Singleton.currentWeaponMagazineLevel;
        // Ensure that currentMaxAmmoLevel is within valid bounds
        if (currentMaxAmmoLevel <= 0 || currentMaxAmmoLevel > ModifierManager.Singleton.maxAmmunitionModifiers.Count)
        {
            Debug.LogError($"Invalid currentMaxAmmoLevel: {currentMaxAmmoLevel}. Must be between 1 and {ModifierManager.Singleton.maxAmmunitionModifiers.Count}");
            return;  // Exit the method to prevent further errors
        }

        // Ensure that currentReloadLevel and currentWeaponMagazineLevel are also within valid bounds
        if (currentReloadLevel <= 0 || currentReloadLevel > ModifierManager.Singleton.reloadTimeModifiers.Count)
        {
            Debug.LogError($"Invalid currentReloadLevel: {currentReloadLevel}. Must be between 1 and {ModifierManager.Singleton.reloadTimeModifiers.Count}");
            return;
        }

        if (currentWeaponMagazineLevel <= 0 || currentWeaponMagazineLevel > ModifierManager.Singleton.weaponMagazineModifiers.Count)
        {
            Debug.LogError($"Invalid currentWeaponMagazineLevel: {currentWeaponMagazineLevel}. Must be between 1 and {ModifierManager.Singleton.weaponMagazineModifiers.Count}");
            return;
        }

        float reloadTimeModifier = ModifierManager.Singleton.reloadTimeModifiers[currentReloadLevel - 1];
        float maxAmmoModifier = ModifierManager.Singleton.maxAmmunitionModifiers[currentMaxAmmoLevel - 1];
        float magazineSizeModifier = ModifierManager.Singleton.weaponMagazineModifiers[currentWeaponMagazineLevel - 1];


        weapon.reloadTime = weapon.originalReloadTime;
        weapon.maxAmmo = weapon.originalMaxAmmo;
        weapon.magazineSize = weapon.originalMagazineSize;
        //set up modified values
        weapon.reloadTime *= reloadTimeModifier * weaponReloadPassive;
        weapon.maxAmmo = (int)(weapon.maxAmmo * maxAmmoModifier * weaponMaxAmmoPassive);
        weapon.magazineSize = (int)(weapon.magazineSize * magazineSizeModifier * weaponMagazinePassive);

        weapon.currentMagazineAmmo = weapon.magazineSize;


    }


    public void SetWeaponPassive( float weaponDamagePassiveModifier, float weaponMagazinePassiveModifier, float weaponReloadPassiveModifier, float weaponMaxAmmoPassiveModifier)
    {
        weaponDamagePassive = weaponDamagePassiveModifier;
        weaponReloadPassive = weaponReloadPassiveModifier;
        weaponMagazinePassive = weaponMagazinePassiveModifier;
        weaponMaxAmmoPassive = weaponMaxAmmoPassiveModifier;

        foreach (WeaponData weapon in weapons)
        {
            SetUpWeaponAmmo(weapon);
            SetUpWeaponDamage(weapon);
        }
    }
    public void SetUpWeaponDamage(WeaponData weapon)
    {
        weapon.ProtectionValues = weapon.originalProtectionValues;
        Debug.Log("Setting up damage for: " + weapon.WeaponName);

        int currentShrapnelWeaponDamageLevel = ModifierManager.Singleton.currentShrapnelWeaponDamageLevel;
        int currentEnergyWeaponDamageLevel = ModifierManager.Singleton.currentEnergyWeaponDamageLevel;
        int currentHeavyWeaponDamageLevel = ModifierManager.Singleton.currentHeavyWeaponDamageLevel;
        int currentMeleeWeaponDamageLevel = ModifierManager.Singleton.currentMeleeWeaponDamageLevel;

        Debug.Log("Current damage levels: ");
        Debug.Log("  Shrapnel: " + currentShrapnelWeaponDamageLevel);
        Debug.Log("  Energy: " + currentEnergyWeaponDamageLevel);
        Debug.Log("  Heavy: " + currentHeavyWeaponDamageLevel);
        Debug.Log("  Melee: " + currentMeleeWeaponDamageLevel);

        float shrapnelDamageModifier = ModifierManager.Singleton.shrapnelWeaponDamageModifiers[currentShrapnelWeaponDamageLevel - 1];
        float energyDamageModifier = ModifierManager.Singleton.energyWeaponDamageModifiers[currentEnergyWeaponDamageLevel - 1];
        float heavyDamageModifier = ModifierManager.Singleton.heavyWeaponDamageModifiers[currentHeavyWeaponDamageLevel - 1];
        float meleeDamageModifier = ModifierManager.Singleton.meleeWeaponDamageModifiers[currentMeleeWeaponDamageLevel - 1];

        Debug.Log("Damage modifiers: ");
        Debug.Log("  Shrapnel: " + shrapnelDamageModifier);
        Debug.Log("  Energy: " + energyDamageModifier);
        Debug.Log("  Heavy: " + heavyDamageModifier);
        Debug.Log("  Melee: " + meleeDamageModifier);

        switch (weapon.weaponCategory)
        {
            case WeaponCategory.Shrapnel:
                Debug.Log("Applying Shrapnel damage modifier: " + shrapnelDamageModifier);
                weapon.ProtectionValues = weapon.ProtectionValues * shrapnelDamageModifier * weaponDamagePassive;
                break;
            case WeaponCategory.Energy:
                Debug.Log("Applying Energy damage modifier: " + energyDamageModifier);
                weapon.ProtectionValues = weapon.ProtectionValues * energyDamageModifier * weaponDamagePassive;
                break;
            case WeaponCategory.Heavy:
                Debug.Log("Applying Heavy damage modifier: " + heavyDamageModifier);
                weapon.ProtectionValues = weapon.ProtectionValues * heavyDamageModifier * weaponDamagePassive;
                break;
            case WeaponCategory.Melee:
                Debug.Log("Applying Melee damage modifier: " + meleeDamageModifier);
                weapon.ProtectionValues = weapon.ProtectionValues * meleeDamageModifier * weaponDamagePassive;
                break;
        }

        Debug.Log("Final ProtectionValues: " + weapon.ProtectionValues);
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
        if (riotShieldColliderInstance != null) // Add this line
        {
            Destroy(riotShieldColliderInstance);
        }
        currentWeapon = Instantiate(weapons[index]);
       
        currentWeaponModel = Instantiate(currentWeapon.GetWeaponModel(), weaponPosition.position, weaponPosition.rotation, weaponPosition.transform);
        currentWeaponModel.layer = LayerMask.NameToLayer("AttachedToPlayer");
        Debug.Log("my weapon is now" + currentWeapon.WeaponName);

        ObjectPool foundObjectPool = FindObjectOfType<ObjectPool>();
        if (currentWeapon.objectPoolGameObject != null)
        {
            currentWeapon.ObjectPool = foundObjectPool;
            Debug.Log("Weapon object pool assigned: " + currentWeapon.ObjectPool);
        }
        
        currentWeaponLogic = GetWeaponLogic(currentWeapon.WeaponName);
        if (currentWeaponLogic is RiotShieldWeapon)
        {
            riotShieldColliderInstance = Instantiate(currentWeapon.riotShieldCollider, weaponPosition.position, weaponPosition.rotation, weaponPosition.transform);
            riotShieldColliderInstance.layer = LayerMask.NameToLayer("Player Riot Shield");
        }
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
            WeaponData clone = Instantiate(weaponHolder.myweaponData);
            SetUpWeaponAmmo(clone);
            SetUpWeaponDamage(clone);     
            clone.totalAmmo += groundTotalAmmo;
           

            currentWeaponIndex = weapons.Count - 1;
            currentWeapon = clone;
            weapons.Add(clone);
           
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

    public void StartShooting(bool useAmmo = true)
    {

        currentWeaponLogic.StartShooting(weaponTip, useAmmo);
    }
    public void Shooting(bool useAmmo = true)
    {
        currentWeaponLogic.Shooting(weaponTip, useAmmo);

    }
    public void StopShooting(bool useAmmo = true)
	{
		currentWeaponLogic.StopShooting(weaponTip, useAmmo);
    }
    public void Reloading()
    {
        currentWeaponLogic.ReloadLogic();
    }

    public Transform GetShootingOrigin()
    {
        if (lookBehavior != null)
        {
            return lookBehavior.myCamera.transform;
        }
        else
        {
            return weaponTip;
        }
        
    }
   
}
