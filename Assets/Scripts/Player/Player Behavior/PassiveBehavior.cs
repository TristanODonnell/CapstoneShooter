using System;
using System.Collections;
using System.Collections.Generic;
using gricel;
using Unity.VisualScripting;
using UnityEngine;
using static gricel.HealthSystem;
using static PassiveData;
using static UnityEngine.Rendering.DebugUI;

public class PassiveBehavior : MonoBehaviour
{
     private PassiveAttribute _currentPassiveAttribute;
    public PassiveData attachedPassive;

    public PlayerController player;
    public Hitbox hitBox;
    public HealthSystem health;
    public MovementBehavior movement;
    public ShootBehavior shoot;
    public GravitationalBehaviour gravitational;
    
    private void Start()
    {
        PassiveAttribute oldPassiveAttribute = CreatePassiveAttribute(attachedPassive);
        ApplyPassiveEffects(oldPassiveAttribute);

    }
    public void Initialize()
    {
        
        
    }
    public void SwapPassive(PassiveData newPassive)
    {
        Debug.Log("Swapping passive from " + attachedPassive + " to " + newPassive);
        PassiveAttribute oldPassiveAttribute = CreatePassiveAttribute(attachedPassive);
        RemovePassiveEffects(oldPassiveAttribute);
        Vector3 dropPosition = transform.position + transform.forward * 2f;
        Quaternion dropRotation = transform.rotation;
        
        GameObject droppedPassive = Instantiate(attachedPassive.GetWorldPassive(), dropPosition, dropRotation);

        attachedPassive = null;
        attachedPassive = newPassive;
        Debug.Log("Attached passive set to " + attachedPassive);

        PassiveAttribute newPassiveAttribute = CreatePassiveAttribute(attachedPassive);

        ApplyPassiveEffects(newPassiveAttribute);
    }
    
    public PassiveAttribute GetCurrentPassiveAttribute()
    {
        if (_currentPassiveAttribute == null)
        {
            _currentPassiveAttribute = CreatePassiveAttribute(attachedPassive);
        }
        return _currentPassiveAttribute;
    }
    
    private PassiveAttribute CreatePassiveAttribute(PassiveData passiveData)
    {
        try
        {
            switch (passiveData.passiveType)
            {
                case PassiveType.TankKit:
                    return new TankKit(passiveData);
                case PassiveType.RamboKit:
                    return new RamboKit(passiveData);
                case PassiveType.SpeedKit:
                    return new SpeedKit(passiveData);
                case PassiveType.CooldownKit:
                    return new CooldownKit(passiveData);
                case PassiveType.XPKit:
                    return new XPKit(passiveData);
                case PassiveType.BigPocketKit:
                    return new BigPocketKit(passiveData);
                case PassiveType.SlowJumperKit:
                    return new SlowJumperKit(passiveData);
                default:
                    return new NoPassive(passiveData);
            }
        }
        catch
        {
            return null;
        }
    } 

    public void ApplyPassiveEffects(PassiveAttribute passiveAttribute)
    {
        try
        {
            passiveAttribute.ApplyEffects(this);
        }
        catch { }
    }

    public void RemovePassiveEffects(PassiveAttribute passiveAttribute)
    {
        Debug.Log("Removing passive effects from " + passiveAttribute);
        passiveAttribute.RemoveEffects(this);
    }


}
