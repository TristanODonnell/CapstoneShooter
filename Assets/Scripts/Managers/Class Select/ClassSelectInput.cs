using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ClassSelectInput : MonoBehaviour
{
    //WILL NEED TO CHANGE TO SPEcIFIC VALUES FOR HOW WE WISH TO HANDLE INPUT

    

    public PassiveData passiveData;
    public WeaponData weaponData;
    public WeaponData weaponData2;
    public WeaponData weaponData3;
    public EquipmentData equipmentData;
    public EquipmentData equipmentData2;
    private void Start()
    {

        ClassSelectManager.Singleton.DataReset();

        OnPassiveButtonClicked(passiveData);
        OnEquipmentButtonClicked(equipmentData);
        OnEquipmentButtonClicked(equipmentData2);
        OnWeaponButtonClicked(weaponData);
        OnWeaponButtonClicked(weaponData2);
        OnWeaponButtonClicked(weaponData3);



        GameManager.Singleton.AssignPlayerToController(ClassSelectManager.Singleton.playerData);

    }
    private void Update()
    {
        
    }



    public void OnWeaponButtonClicked(WeaponData weaponData)
    {
        ClassSelectManager.Singleton.ChooseWeapon(weaponData);
    }

    


    public void OnPassiveButtonClicked(PassiveData passiveData)
    {
        ClassSelectManager.Singleton.ChoosePassive(passiveData);
    }

    public void OnEquipmentButtonClicked(EquipmentData equipmentData)
    {
        ClassSelectManager.Singleton.ChooseEquipment(equipmentData);
    }

    
} 
