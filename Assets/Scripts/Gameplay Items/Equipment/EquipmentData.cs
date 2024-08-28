using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Equipment Data")]
[System.Serializable]
public class EquipmentData : ScriptableObject
{
    [SerializeField] protected string equipmentName;
    [SerializeField] protected int itemCost;
    [SerializeField] protected GameObject equipmentVisual;
    [SerializeField] protected GameObject worldEquipmentModel;


    public string EquipmentName => equipmentName;
    public int ItemCost => itemCost;
    public GameObject GetPlayerEquipmentVisual() => equipmentVisual;
    public GameObject GetWorldEquipment() => worldEquipmentModel;

    

    public virtual void UseEquipment()
    {

    }
}
