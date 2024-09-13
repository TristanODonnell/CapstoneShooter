using System.Collections;
using System.Collections.Generic;
using Abilities;
using throwables;
using UnityEngine;

[CreateAssetMenu(menuName = "New Player Data")]
public class PlayerData : ScriptableObject
{
    public List<WeaponData> playerWeaponData = new List<WeaponData>();
    public PassiveData playerPassiveData;
    public AbilityBase playerAbilityReference;
    



}
