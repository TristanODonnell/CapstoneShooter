using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using throwables;
using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    public DataManager Singleton { get { return DataManager.Singleton; } }
    public int[] grenadeCounts;
    
   private GrenadeBehavior grenadeBehavior;

    public ThrowableItem currentGrenade;
    public int currentIndex = 0;
    public static GrenadeManager instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }
    private void Start() 
    {
       // grenadeCounts = new int[DataManager.Singleton.grenades.Count];
        //for (int i = 0; i < grenadeCounts.Length; i++)
      //  {
      //      grenadeCounts[i] = 10;
       //     Debug.Log($"Initializing grenade count for type {DataManager.Singleton.grenades[i].name} to 0");
      //  }
      //  currentGrenade = Singleton.grenades.ElementAt(0);
    }

    public void AddGrenade(ThrowableItem grenadeType, int count = 1)
    {
        int index = Singleton.grenades.IndexOf(grenadeType);
        if (index != -1)
        {
            if (grenadeCounts[index] + count <= 2)
            {
                grenadeCounts[index] += count;
            }
            else
            {
                grenadeCounts[index] = 2;
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
    
    public void RemoveGrenade(ThrowableItem grenadeName, int count = 1)
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
            currentGrenade = Singleton.grenades[currentIndex];
        }
        //SWAPPING IN BETWEEN THE LIST 
    }
     
    public void ThrowGrenade()
    {
        Debug.Log("ThrowGrenade called"); 
        int index = Singleton.grenades.IndexOf(currentGrenade);
        if (index != -1)
        {
            Debug.Log("Current grenade found in Singleton.grenades at index " + index); 
            if (grenadeCounts[index] > 0)
            {
                Debug.Log("Grenade count at index " + index + " is greater than 0"); 
                if (grenadeBehavior != null)
                {
                    Debug.Log("grenadeBehavior is not null, throwing grenade"); 
                    grenadeBehavior = ClassSelectManager.Singleton.player.gameObject.GetComponent<GrenadeBehavior>();
                    grenadeBehavior.ThrowGrenade(currentGrenade);
                    RemoveGrenade(currentGrenade);
                    //ADD IF GREATER OR EQUAL TO !, CAN THROW 
                }
                else
                {
                    Debug.LogError("grenadeBehavior is null");
                }
            }
            else
            {
                Debug.Log("Grenade count at index " + index + " is 0 or less");
            }
        }
        else
        {
            Debug.LogError("Current grenade not found in Singleton.grenades");
        }
    }


}
