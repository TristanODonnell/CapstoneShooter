using System.Collections;
using System.Collections.Generic;
using gricel;
using UnityEngine;
using static gricel.HealthSystem;
using static UnityEngine.Rendering.DebugUI;

public class PassiveBehavior : MonoBehaviour
{
    public PassiveData attachedPassive;
    private RamboKit ramboKit;
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
        ramboKit = new RamboKit(attachedPassive);

        // Apply the passive effects
        ApplyPassiveEffects(ramboKit);
    }

    public void SwapPassive(PassiveData newPassive)
    {
        Vector3 dropPosition = transform.position + transform.forward * 2f;
        Quaternion dropRotation = transform.rotation;

        GameObject droppedPassive = Instantiate(attachedPassive.GetWorldPassive(), dropPosition, dropRotation);

        attachedPassive = newPassive; 
        

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
