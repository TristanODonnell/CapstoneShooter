using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillNode;

public class SkillTree : MonoBehaviour
{/*
    public List<SkillNode> speedNodes = new List<SkillNode>();
    public List<SkillNode> jumpHeightNodes = new List<SkillNode>();
    public List<SkillNode> reloadTimeNodes = new List<SkillNode>();
    public List<SkillNode> maxAmmunitionNodes = new List<SkillNode>();
    public List<SkillNode> maxMagazineNodes = new List<SkillNode>();
    public List<SkillNode> recursiveBashStrengthNodes = new List<SkillNode>();
    public List<SkillNode> healthUpgradeNodes = new List<SkillNode>();
    public List<SkillNode> weaponBoostNodes = new List<SkillNode>();
    public List<SkillNode> damageWheelIncreaseNodes = new List<SkillNode>();
    public List<SkillNode> squadDecreaseNodes = new List<SkillNode>();
    public List<SkillNode> totalDangerDecreaseNodes = new List<SkillNode>();
    public List<SkillNode> xpGainedNodes = new List<SkillNode>();
    public PlayerController playerController;

    private void Start()
    {
        
        InitializeSkillNodes();
    }

    public void UnlockSkillNode(SkillNode node)
    {
        node.isUnlocked = true;
        ApplySkillNode(node);
    }

    private void InitializeSkillNodes()
    {
        speedNodes.Add(new SkillNode("Speed 1", "Increases speed by 5%", 100, SkillType.Speed, 1.05f));
        speedNodes.Add(new SkillNode("Speed 2", "Increases speed by 10%", 200, SkillType.Speed, 1.10f));
        speedNodes.Add(new SkillNode("Speed 3", "Increases speed by 15%", 300, SkillType.Speed, 1.15f));
        speedNodes.Add(new SkillNode("Speed 4", "Increases speed by 20%", 400, SkillType.Speed, 1.20f));
    }

    public void ApplySkillNode(SkillNode node)
    {
        switch (node.type)
        {
            case SkillType.Speed:
                playerController.move.modifiedSpeed *= node.value; // Multiply the modified speed by the node value
                break;
            case SkillType.JumpHeight:
                playerController.modifiedJumpForce *= node.value;
                break;




                /*
                switch (node.type)
                {
                    case SkillType.ReloadTime:
                        playerController.shoot.reloadTime *= node.value;
                        break;
                    case SkillType.MaxAmmunition:
                        playerController.shoot.maxAmmunition += node.value;
                        break;
                    case SkillType.MaxMagazine:
                        playerController.shoot.maxMagazine *= node.value;
                        break;
                    case SkillType.RecursiveBashStrength:
                        playerController.passive.recursiveBashStrength *= node.value;
                        break;
                    case SkillType.HealthUpgrade:
                        playerController.healthSystem.armourHealth *= node.value;
                        playerController.healthSystem.fleshHealth *= node.value;
                        playerController.healthSystem.energyShield *= node.value;
                        break;
                    case SkillType.WeaponBoost:
                        playerController.shoot.weaponDamage *= node.value;
                        break;
                    case SkillType.DamageWheelIncrease:
                        playerController.shoot.damageWheelIncrease *= node.value;
                        break;
                    case SkillType.SquadDecrease:
                        playerController.squadDecrease *= node.value;
                        break;
                    case SkillType.TotalDangerDecrease:
                        playerController.totalDangerDecrease *= node.value;
                        break;
                    case SkillType.XPGained:
                        playerController.xpGained *= node.value;
                        break;
                    ADD FOR COOLDOWN EQUIPMENT AS WELL 
               
        }

    }

    */

}
