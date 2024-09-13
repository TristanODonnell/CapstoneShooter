using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierManager : MonoBehaviour
{
    public static ModifierManager Singleton
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
    public int currentEquipmentCooldownLevel =1; //next
    public int currentSpeedLevel = 1; //done
    public int currentJumpHeightLevel = 1; //done
    public int currentReloadLevel = 1; //DONE
    public int currentmaxAmmoLevel = 1; //DONE
    public int currentWeaponMagazineLevel = 1; //DONE
    public int currentRecursiveBashLevel = 1;//done----------------
    public int currentArmorLevel = 1;//Done
    public int currentFleshLevel = 1;//Done
    public int currentEnergyShieldLevel = 1;//Done
    public int currentShrapnelWeaponDamageLevel = 3; //done
    public int currentEnergyWeaponDamageLevel = 1; //done
    public int currentHeavyWeaponDamageLevel = 1; //done
    public int currentMeleeWeaponDamageLevel = 1; //done
    public int currentGrenadeDamageLevel = 1;//done
    public int currentTotalEnemyCreditsLevel = 1;//done
    public int currentXPGainedLevel = 1;//done

    public List<float> equipmentCooldownModifiers = new List<float> { 1.0f, 0.95f, 0.90f, 0.75f, 0.60f, 0.40f }; //equipmentcooldown
    public List<float> speedModifiers = new List<float> { 1.0f, 1.05f, 1.15f, 1.25f, 1.50f, 2.00f }; //speed
    public List<float> jumpingHeightModifiers = new List<float> { 1.0f, 1.25f, 1.5f, 2.0f, 2.5f, 3.0f }; //jumpheight
    public List<float> reloadTimeModifiers = new List<float> { 1.0f, 0.95f, 0.90f, 0.80f, 0.70f, 0.50f }; //reloadtime
    public List<float> maxAmmunitionModifiers = new List<float> { 1.0f, 1.25f, 1.50f, 1.75f, 2.0f, 2.25f, }; //max ammo
    public List<float> weaponMagazineModifiers = new List<float> { 1.0f, 1.05f, 1.10f, 1.25f, 1.50f, 1.75f, 2.0f }; //magazine size
    public List<float> recursiveBashModifiers = new List<float> { 1.0f, 1.10f, 1.20f, 1.40f, 1.70f, 2.0f }; //melee
    public List<float> armorModifiers = new List<float> { 1.0f, 1.20f, 1.40f, 1.60f, 1.80f, 2.0f, 2.50f, 3.00f }; //health
    public List<float> fleshModifiers = new List<float> { 1.0f, 1.05f, 1.10f, 1.20f, 1.35f, 1.50f, 2.0f, 2.50f }; //health
    public List<float> energyShieldModifiers = new List<float> { 1.0f, 1.05f, 1.10f, 1.25f, 1.5f, 2.00f, 2.50f, 3.0f }; //health
    public List<float> shrapnelWeaponDamageModifiers = new List<float> { 1.0f, 1.10f, 1.30f, 1.50f, 1.75f, 2.0f }; //shrapnel weapon damage
    public List<float> energyWeaponDamageModifiers = new List<float> { 1.0f, 1.10f, 1.30f, 1.50f, 1.75f, 2.0f }; //energy weapon damage
    public List<float> heavyWeaponDamageModifiers = new List<float> { 1.0f, 1.10f, 1.30f, 1.50f, 1.75f, 2.0f }; //heavy weapon damage
    public List<float> meleeWeaponDamageModifiers = new List<float> { 1.0f, 1.10f, 1.30f, 1.50f, 1.75f, 2.0f }; //melee weapon damage
    public List<float> grenadeDamageModifiers = new List<float> { 1.0f, 1.10f, 1.30f, 1.50f, 1.75f, 2.0f }; //grenade damage
    public List<float> totalEnemyCreditsModifiers = new List<float> { 1.0f, 0.97f, 0.94f, 0.88f, 0.80f, 0.70f, 0.60f, 0.40f }; //credits available per round 
    public List<float> xpGainedModifiers = new List<float> { 1.0f, 1.20f, 1.50f, 2.00f, 2.50f, 3.00f }; //xp gained 





}
