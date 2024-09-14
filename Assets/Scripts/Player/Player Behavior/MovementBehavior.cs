using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementBehavior : MonoBehaviour
{
    public float originalSpeed;
    public float speed;

    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float walkMultiplier;
    [SerializeField] private CharacterController controller;
    private Vector3 moveDirection;
    private bool isSprinting = false;

    private void Start()
    {
        speed = originalSpeed;
        SetPlayerSpeedModifier(1);
    }
    public void MovePlayer()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.z = Input.GetAxisRaw("Vertical");

        Vector3 moveForward = transform.forward * moveDirection.z;
        Vector3 moveRight = transform.right * moveDirection.x;


        float currentSpeedMultiplier = isSprinting? sprintMultiplier : walkMultiplier;
        controller.Move((moveForward + moveRight) * Time.deltaTime * speed * currentSpeedMultiplier);
    }

    public void SetSprinting(bool isSprinting)
    {
        this.isSprinting = isSprinting;
    }

    public void SetPlayerSpeedModifier(float speedModifier)
    {
        speed = originalSpeed;
        int currentSpeedLevel = ModifierManager.Singleton.currentSpeedLevel;

        float speedSkillTree = ModifierManager.Singleton.speedModifiers[currentSpeedLevel - 1];

        float modifiedSpeed = speedSkillTree * speedModifier * speed;
        speed = modifiedSpeed;
    }    

    

}
