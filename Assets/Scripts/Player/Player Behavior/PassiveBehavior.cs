using System;
using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;
using static gricel.HealthSystem;
using static PassiveData;
using static UnityEngine.Rendering.DebugUI;

public class PassiveBehavior : MonoBehaviour
{
     private PassiveAttribute _currentPassiveAttribute;
    public PassiveData attachedPassive;
     
    public PlayerController player { get; private set; }
    public Hitbox hitBox { get; private set; }
    public HealthSystem health { get; private set; }
    public MovementBehavior movement { get; private set; }
    public ShootBehavior shoot { get; private set; }
    public EquipmentBehavior equipment { get; private set; }
    public GravitationalBehaviour gravitational { get; private set; }
    public DataManager dataManager { get; private set; }
    public XPSystem xpSystem { get; private set; }
    private void Start()
    {
        player = GetComponent<PlayerController>();
        hitBox = GetComponent<Hitbox>();
        health = GetComponent<HealthSystem>();
        movement = GetComponent<MovementBehavior>();
        shoot = GetComponent<ShootBehavior>();
        equipment = GetComponent<EquipmentBehavior>();
        gravitational = GetComponent<GravitationalBehaviour>();
        dataManager = DataManager.Singleton;
        xpSystem = XPSystem.Singleton;

        PassiveAttribute currentPassiveAttribute = GetCurrentPassiveAttribute();
        Debug.Log("Current passive attribute: " + currentPassiveAttribute.GetType().Name);
    }

    public void SwapPassive(PassiveData newPassive)
    {
        Vector3 dropPosition = transform.position + transform.forward * 2f;
        Quaternion dropRotation = transform.rotation;

        GameObject droppedPassive = Instantiate(attachedPassive.GetWorldPassive(), dropPosition, dropRotation);

        attachedPassive = newPassive;
        RemovePassiveEffects(GetCurrentPassiveAttribute());

        PassiveAttribute newPassiveAttribute = CreatePassiveAttribute(attachedPassive);

        ApplyPassiveEffects(newPassiveAttribute);
    }
    
    private PassiveAttribute GetCurrentPassiveAttribute()
    {
        if (_currentPassiveAttribute == null)
        {
            _currentPassiveAttribute = CreatePassiveAttribute(attachedPassive);
        }
        return _currentPassiveAttribute;
    }
    
    private PassiveAttribute CreatePassiveAttribute(PassiveData passiveData)
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
            case PassiveType.NoPassive:
                return new NoPassive(passiveData);
                
            default:
                throw new ArgumentException("Unsupported passive type", nameof(passiveData));
        }
    }

    public void ApplyPassiveEffects(PassiveAttribute passiveAttribute)
    {
        passiveAttribute.ApplyEffects(this);
    }

    public void RemovePassiveEffects(PassiveAttribute passiveAttribute)
    {
        passiveAttribute.RemoveEffects(this);
    }


}
