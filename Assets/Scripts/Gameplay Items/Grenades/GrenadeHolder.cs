using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeHolder : MonoBehaviour
{
    public GrenadeData myGrenadeData;


    public void OnTriggerEnter(Collider other)
    {
        int index = DataManager.Singleton.grenades.IndexOf(myGrenadeData);
        if(index != -1 && GameManager.Singleton.grenadeManager.grenadeCounts[index] < 2)
        {
            GameManager.Singleton.grenadeManager.AddGrenade(myGrenadeData, 1);
            Destroy(gameObject);
        }
    }
}
 