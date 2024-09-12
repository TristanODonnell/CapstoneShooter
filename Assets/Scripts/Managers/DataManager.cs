using System.Collections;
using System.Collections.Generic;
using throwables;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Singleton
    {
        get; private set;
    }
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    /*
    public void ApplyMagSizeModifiers(float ammoMultiplier)
    {
        // access all ScriptableObjects containing weapon data
        foreach (WeaponData weaponData in weapons)
        {
            weaponData.magazineSize = (int)(weaponData.originalMagazineSize * ammoMultiplier);
        }
    }

    public void ResetMagSizeModifiers()
    {
        foreach (WeaponData weaponData in weapons)
        {
            weaponData.magazineSize = weaponData.originalMagazineSize;
        }
    }

    public void ApplyReloadTimeModifiers(float reloadMultiplier)
    {
        foreach (WeaponData weaponData in weapons)
        {
            weaponData.reloadTime = (weaponData.originalReloadTime * reloadMultiplier);
        }
    }
    public void ResetReloadTimeModifiers()
    {
        foreach (WeaponData weaponData in weapons)
        {
            weaponData.reloadTime = weaponData.originalReloadTime;
        }
    }

    public void ApplyTotalAmmoModifiers(float maxAmmoMultiplier)
    {
        foreach (WeaponData weaponData in weapons)
        {
            weaponData.maxAmmo = (int)(weaponData.originalMaxAmmo * maxAmmoMultiplier);
        }
    }
    public void ResetMaxAmmoModifiers()
    {
        foreach (WeaponData weaponData in weapons)
        {
            weaponData.maxAmmo = weaponData.originalMaxAmmo;
        }
    }
    */
    public List<WeaponData> weapons = new List<WeaponData>();
    public List<EquipmentData> equipment = new List<EquipmentData>();
    public List<PassiveData> passive = new List<PassiveData>();


    public List<ThrowableItem> grenades = new List<ThrowableItem>();
    
}
