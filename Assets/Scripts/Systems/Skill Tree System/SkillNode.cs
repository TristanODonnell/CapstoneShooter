using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Speed,
    JumpHeight,
    ReloadTime,
    MaxAmmunition,
    MaxMagazine,
    RecursiveBashStrength,
    HealthUpgrade,
    WeaponBoost,
    DamageWheelIncrease,
    SquadDecrease,
    TotalDangerDecrease,
    XPGained
}
[System.Serializable]
public class SkillNode : MonoBehaviour
{
    public string name;
    public string description;
    public int cost;
    public SkillType type;
    public float value;
    public bool isUnlocked;

    public SkillNode(string name, string description, int cost, SkillType type, float value)
    {
        this.name = name;
        this.description = description;
        this.cost = cost;
        this.type = type;
        this.value = value;
        this.isUnlocked = false; // default to false
    }
}
