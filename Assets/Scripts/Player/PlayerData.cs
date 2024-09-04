using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Player Data")]
public class PlayerData : ScriptableObject
{
    public PassiveData playerPassiveData;
    public List<EquipmentData> playerEquipmentData = new List<EquipmentData>();
    public List<WeaponData> playerWeaponData = new List<WeaponData>();
    public GrenadeData playerGrenadeSelect;

    



}
