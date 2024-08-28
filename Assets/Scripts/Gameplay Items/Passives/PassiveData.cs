using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Passive Data")]
public class PassiveData : ScriptableObject
{
    [SerializeField] protected string passiveName;
    [SerializeField] protected int itemCost;
    [SerializeField] protected GameObject worldPassiveModel;

    public string PassiveName => passiveName;
    public int ItemCost => itemCost;
    public GameObject GetWorldPassive() => worldPassiveModel;

}
