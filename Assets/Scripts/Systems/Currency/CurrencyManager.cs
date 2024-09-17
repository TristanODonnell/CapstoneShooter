using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager singleton;

    public UnityEvent<int> OnCurrencyChanged;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
    }
    public int totalCurrency; 
    public int currencyAdded { get; private set; }

    
    public void AddCurrency(int amount)
    {
        totalCurrency += amount;
        currencyAdded = amount;
        OnCurrencyChanged.Invoke(totalCurrency);
    }

    public void SubtractCurrency(int amount)
    {
        totalCurrency -= amount;
        OnCurrencyChanged.Invoke(totalCurrency);
    }
}
