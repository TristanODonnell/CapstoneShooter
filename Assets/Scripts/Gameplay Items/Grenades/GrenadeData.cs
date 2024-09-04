using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Grenade Data")]
public class GrenadeData : ScriptableObject
{
    [SerializeField] protected string grenadeName;
    [SerializeField] protected GameObject worldGrenadeModel;
    [SerializeField] protected int itemCost;
    public string GrenadeName => grenadeName; //mainly using with debugs 
    public int ItemCost => itemCost;
    public GameObject GetWorldGrenade() => worldGrenadeModel;
}
