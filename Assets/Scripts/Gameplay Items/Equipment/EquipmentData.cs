using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Equipment Data")]
[System.Serializable]
public class EquipmentData : ScriptableObject
{
    public string equipmentName;
    [SerializeField] private int itemCost;
    [SerializeField] private GameObject equipmentVisual;
    [SerializeField] private GameObject worldEquipmentModel;
    public float equipmentCooldown;

    public string EquipmentName { get; private set; }
    public int ItemCost => itemCost;
    public GameObject GetPlayerEquipmentVisual() => equipmentVisual;
    public GameObject GetWorldEquipment() => worldEquipmentModel;

}
