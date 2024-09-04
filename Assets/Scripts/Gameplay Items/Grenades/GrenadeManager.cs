using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    public DataManager Singleton { get { return DataManager.Singleton; } }
    public int[] grenadeCounts;
    
    [SerializeField] private GrenadeBehavior grenadeBehavior;

    public GrenadeData currentGrenade;
    public int currentIndex = 0;

    private void Start() 
    {
        grenadeCounts = new int[DataManager.Singleton.grenades.Count];
        for (int i = 0; i < grenadeCounts.Length; i++)
        {
            grenadeCounts[i] = 0;
        }

        
    }

    public void AddGrenade(GrenadeData grenadeName, int count = 1)
    {
        int index = Singleton.grenades.IndexOf(grenadeName);
        if (index != -1)
        {
            if (grenadeCounts[index] + count <= 2)
            {
                grenadeCounts[index] += count;
            }
            else
            {
                grenadeCounts[index] = 2;
                Debug.Log("Cannot carry more than 2 " + grenadeName.GrenadeName + "s");
            }
            
        }
        
        

    }
    public void GrenadeRefill()
    {
        for (int i = 0; i < grenadeCounts.Length; i++)
        {
            grenadeCounts[i] = 2;
        }
    }

    public void RemoveGrenade(GrenadeData grenadeName, int count = 1)
    {
        int index = Singleton.grenades.IndexOf(grenadeName);
        if(index != -1)
        {
            grenadeCounts[index] -= count;
            if (grenadeCounts[index] <= 0)
            {
                grenadeCounts[index] = 0;
            }
        }
        
    }


    public void CurrentGrenadeSelect() //HAVE HERE BC INPUT BASED 
    {
        if (currentIndex >= 0 && currentIndex < Singleton.grenades.Count)
        {
            currentGrenade = Singleton.grenades.ElementAt(currentIndex);
            Debug.Log("Current grenade: " + currentGrenade.GrenadeName);
        }
        //SWAPPING IN BETWEEN THE LIST 
    }
     
    public void ThrowGrenade()
    {
        int index = Singleton.grenades.IndexOf(currentGrenade);
        if(index != -1  && grenadeCounts[index] > 0)
        {
            grenadeBehavior.ThrowGrenade(currentGrenade);  
            RemoveGrenade(currentGrenade);
            //ADD IF GREATER OR EQUAL TO !, CAN THROW 
            Debug.Log("Threw Grenade" + currentGrenade.GrenadeName);
        }
        else
        {
            Debug.Log("Not enough " + currentGrenade.GrenadeName + "s in inventory");
        } 
    }
}
