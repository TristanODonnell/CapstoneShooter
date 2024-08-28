using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager singleton;
   
    UnityEvent<int> OnCurrencyChanged; //CAN USE FOR UI
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
    }
    public int totalCurrency {  get; private set; }
    public int currencyAdded { get; private set; }

    public void SetTotalCurrency(int value)
    {
        totalCurrency = value;
    }

    public void AddCurrency(int amount)
    {
        totalCurrency += amount;
        currencyAdded = amount;
       
    }

    public void SubtractCurrency(int amount)
    {
        totalCurrency -= amount;
       
    }
}
