using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;
using static gricel.HealthSystem;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(menuName = "New Passive Data")]
public class PassiveData : ScriptableObject
{
    [SerializeField] protected string passiveName;
    [SerializeField] protected int itemCost;
    [SerializeField] protected GameObject worldPassiveModel;
    public string PassiveName => passiveName;
    public int ItemCost => itemCost;
    public GameObject GetWorldPassive() => worldPassiveModel;
    public PassiveType passiveType;
    public enum PassiveType { TankKit, SpeedKit, CooldownKit, XPKit, RamboKit, BigPocketKit, SlowJumperKit, NoPassive}
}
