using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EquipmentBehavior : MonoBehaviour
{
    //we changed format to be player_abilitybehavior
    /*
    public GameObject attachedEquipmentModel1;
    public EquipmentData playerEquipmentData1;

    public GameObject attachedEquipmentModel2;
    public EquipmentData playerEquipmentData2;

    [SerializeField] private Transform equipmentPosition;



    private void Start()
    {
        SetUpEquipment();
    }
    public void SetUpEquipment()
    {

        attachedEquipmentModel1 = Instantiate(playerEquipmentData1.GetPlayerEquipmentVisual(), equipmentPosition.position, equipmentPosition.rotation, equipmentPosition.transform);
        attachedEquipmentModel2 = Instantiate(playerEquipmentData2.GetPlayerEquipmentVisual(), equipmentPosition.position, equipmentPosition.rotation, equipmentPosition.transform);

        attachedEquipmentModel1.layer = LayerMask.NameToLayer("AttachedToPlayer");
        attachedEquipmentModel2.layer = LayerMask.NameToLayer("AttachedToPlayer");
    }

    public void ReplaceEquipment(int equipmentNumber, EquipmentData newEquipment)
    {

        Vector3 dropPosition = transform.position + transform.forward * 2f;
        Quaternion dropRotation = transform.rotation;

        if (equipmentNumber == 0)
        {

            
            GameObject droppedEquipment1 = Instantiate(playerEquipmentData1.GetWorldEquipment(), dropPosition, dropRotation);
            

            Destroy(attachedEquipmentModel1);
            playerEquipmentData1 = newEquipment;
            attachedEquipmentModel1 = Instantiate(playerEquipmentData1.GetPlayerEquipmentVisual(), equipmentPosition.position, equipmentPosition.rotation, equipmentPosition.transform);
            attachedEquipmentModel1.layer = LayerMask.NameToLayer("AttachedToPlayer");
        }
        else if (equipmentNumber == 1)
        {


            GameObject droppedEquipment1 = Instantiate(playerEquipmentData2.GetWorldEquipment(), dropPosition, dropRotation);


            Destroy(attachedEquipmentModel2);
            playerEquipmentData2 = newEquipment;
            attachedEquipmentModel2 = Instantiate(playerEquipmentData2.GetPlayerEquipmentVisual(), equipmentPosition.position, equipmentPosition.rotation, equipmentPosition.transform);
            attachedEquipmentModel2.layer = LayerMask.NameToLayer("AttachedToPlayer");
        }

    }
    
    public void UseEquipment()
    {

    }
    */
}
