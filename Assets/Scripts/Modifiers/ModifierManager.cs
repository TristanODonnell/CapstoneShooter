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
    public enum SkillTreeType
    {
        Ability,
        Speed,
        JumpHeight,
        Reload,
        MaxAmmo,
        WeaponMagazineSize,
        RecursiveBash,
        ArmorHealth,
        FleshHealth,
        EnergyShield,
        ShrapnelDamage,
        EnergyDamage,
        HeavyDamage,
        MeleeDamage,
        GrenadeDamage,
        EnemyNumbers,
        XPGained,
    }
    public void UpdateSkillTreeType(SkillTreeType skillTreeType)
    {
        Debug.Log("Updating skill tree type: " + skillTreeType);
        switch (skillTreeType)
        {
            case SkillTreeType.Ability:
                currentAbilityCooldownLevel++;
                break;
            case SkillTreeType.Speed:
                currentSpeedLevel++;
                break;
            case SkillTreeType.JumpHeight:
                currentJumpHeightLevel++; break;
            case SkillTreeType.Reload:
                currentReloadLevel++; break;
            case SkillTreeType.MaxAmmo:
                currentmaxAmmoLevel++; break;
            case SkillTreeType.WeaponMagazineSize:
                currentWeaponMagazineLevel++; break;
            case SkillTreeType.RecursiveBash:
                currentRecursiveBashLevel++; break;
            case SkillTreeType.ArmorHealth:
                currentArmorLevel++; break;
            case SkillTreeType.FleshHealth:
                currentFleshLevel++; break;
            case SkillTreeType.EnergyShield:
                currentEnergyShieldLevel++; break;
            case SkillTreeType.ShrapnelDamage:
                currentShrapnelWeaponDamageLevel++; break;
            case SkillTreeType.EnergyDamage:
                currentEnergyWeaponDamageLevel++; break;
            case SkillTreeType.HeavyDamage:
                currentHeavyWeaponDamageLevel++; break;
            case SkillTreeType.MeleeDamage:
                currentMeleeWeaponDamageLevel++; break;
            case SkillTreeType.GrenadeDamage:
                currentGrenadeDamageLevel++; break;
            case SkillTreeType.EnemyNumbers:
                currentTotalEnemyCreditsLevel++; break;
            case SkillTreeType.XPGained:
                currentXPGainedLevel++; break;
            default:
                break;
        }
    }
    public int GetCurrentLevel(SkillTreeType type)
    {
        switch (type)
        {
            case SkillTreeType.Ability:
                return currentAbilityCooldownLevel;
            case SkillTreeType.Speed:
                return currentSpeedLevel;

            case SkillTreeType.JumpHeight:
                return currentJumpHeightLevel; 
            case SkillTreeType.Reload:
                return currentReloadLevel;
            case SkillTreeType.MaxAmmo:
                return currentmaxAmmoLevel; 
            case SkillTreeType.WeaponMagazineSize:
                return currentWeaponMagazineLevel;
            case SkillTreeType.RecursiveBash:
                return currentRecursiveBashLevel;
            case SkillTreeType.ArmorHealth:
                return currentArmorLevel; 
            case SkillTreeType.FleshHealth:
                return currentFleshLevel;
            case SkillTreeType.EnergyShield:
                return currentEnergyShieldLevel;
            case SkillTreeType.ShrapnelDamage:
                return currentShrapnelWeaponDamageLevel;
            case SkillTreeType.EnergyDamage:
                return currentEnergyWeaponDamageLevel;
            case SkillTreeType.HeavyDamage:
                return currentHeavyWeaponDamageLevel;
            case SkillTreeType.MeleeDamage:
                return currentMeleeWeaponDamageLevel;
            case SkillTreeType.GrenadeDamage:
                return currentGrenadeDamageLevel;
            case SkillTreeType.EnemyNumbers:
                return currentTotalEnemyCreditsLevel;
            case SkillTreeType.XPGained:
                return currentXPGainedLevel;
            default:
                return 0;
        }
    }
    public bool IsMaxLevel(SkillTreeType type)
    {
        switch (type)
        {
            case SkillTreeType.Ability:
                return currentAbilityCooldownLevel >= abilityCooldownModifiers.Count;
            case SkillTreeType.Speed:
                return currentSpeedLevel >= speedModifiers.Count;
            case SkillTreeType.JumpHeight:
                return currentJumpHeightLevel >= jumpingHeightModifiers.Count;
            case SkillTreeType.Reload:
                return currentReloadLevel >= reloadTimeModifiers.Count;
            case SkillTreeType.MaxAmmo:
                return currentmaxAmmoLevel >= maxAmmunitionModifiers.Count;
            case SkillTreeType.WeaponMagazineSize:
                return currentWeaponMagazineLevel >= weaponMagazineModifiers.Count;
            case SkillTreeType.RecursiveBash:
                return currentRecursiveBashLevel >= recursiveBashModifiers.Count;
            case SkillTreeType.ArmorHealth:
                return currentArmorLevel >= armorModifiers.Count;
            case SkillTreeType.FleshHealth:
                return currentFleshLevel >= fleshModifiers.Count;
            case SkillTreeType.EnergyShield:
                return currentEnergyShieldLevel >= energyShieldModifiers.Count;
            case SkillTreeType.ShrapnelDamage:
                return currentShrapnelWeaponDamageLevel >= shrapnelWeaponDamageModifiers.Count;
            case SkillTreeType.EnergyDamage:
                return currentEnergyWeaponDamageLevel >= energyWeaponDamageModifiers.Count;
            case SkillTreeType.HeavyDamage:
                return currentHeavyWeaponDamageLevel >= heavyWeaponDamageModifiers.Count;
            case SkillTreeType.MeleeDamage:
                return currentMeleeWeaponDamageLevel >= meleeWeaponDamageModifiers.Count;
            case SkillTreeType.GrenadeDamage:
                return currentGrenadeDamageLevel >= grenadeDamageModifiers.Count;
            case SkillTreeType.EnemyNumbers:
                return currentTotalEnemyCreditsLevel >= totalEnemyCreditsModifiers.Count;
            case SkillTreeType.XPGained:
                return currentXPGainedLevel >= xpGainedModifiers.Count;
            default:
                return false;
        }
    }
    public int currentAbilityCooldownLevel =1; //next
    public int currentSpeedLevel = 1; //done
    public int currentJumpHeightLevel = 1; //done
    public int currentReloadLevel = 1; //DONE
    public int currentmaxAmmoLevel = 1; //DONE
    public int currentWeaponMagazineLevel = 1; //DONE
    public int currentRecursiveBashLevel = 1;//done----------------
    public int currentArmorLevel = 1;//Done
    public int currentFleshLevel = 1;//Done
    public int currentEnergyShieldLevel = 1;//Done
    public int currentShrapnelWeaponDamageLevel = 1; //done
    public int currentEnergyWeaponDamageLevel = 1; //done
    public int currentHeavyWeaponDamageLevel = 1; //done
    public int currentMeleeWeaponDamageLevel = 1; //done
    public int currentGrenadeDamageLevel = 1;//done
    public int currentTotalEnemyCreditsLevel = 1;//done
    public int currentXPGainedLevel = 1;//done

    public List<float> abilityCooldownModifiers = new List<float> { 1.0f, 0.95f, 0.90f, 0.75f, 0.60f, 0.40f }; //equipmentcooldown
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
