using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPSystem : MonoBehaviour
{
    public int totalXP;
    public int currentXP;
    public int playerLevel;
    public int xpRequiredForNextLevel;
    public int xpGainPerWave;
    [SerializeField] private float xpMultiplier;
    [SerializeField] private float xpPassiveMultiplier;
    public static XPSystem Singleton
    {
        get; private set;
    }
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // Set the initial XP required for the next level
      //  xpRequiredForNextLevel = 100; // Example value

        // Set the initial XP gain per level
      //  xpGainPerWave = 50; // Example value

       // SetXPModifier();
       // playerLevel = 1;
       // currentXP = 0;

      //  AddXP(4000);
    }

    public void AddXP(int xp)
    {
        currentXP += (int)(xp * xpMultiplier * xpPassiveMultiplier) ;

        // Check if the player has leveled up
        while (currentXP >= xpRequiredForNextLevel)
        {
            LevelUp();
        }
    }

    // Level up the player
    private void LevelUp()
    {
        int remainingXP = currentXP - xpRequiredForNextLevel;
        currentXP = remainingXP;
        playerLevel++;

        xpRequiredForNextLevel = (int)(100 + (playerLevel * 100 / (playerLevel * 0.1f + 1)));

    }

    public void SetXPModifier()
    {
        int currentXPGainedLevel = ModifierManager.Singleton.currentXPGainedLevel;

         xpMultiplier = ModifierManager.Singleton.xpGainedModifiers[currentXPGainedLevel - 1];

    }


    public void SetPassiveXPMultiplier(float multiplier)
    {
        xpPassiveMultiplier = multiplier;
    }
}


