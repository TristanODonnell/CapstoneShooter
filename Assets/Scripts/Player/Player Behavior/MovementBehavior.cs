using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementBehavior : MonoBehaviour
{
    public SpeedModifier speedModifier;
    public SpeedModifier.Speed speed = new SpeedModifier.Speed(5.0f);
    public float SpeedValue
    {
        get { return speed.baseSpeed * speed.multiplier ; }
        set { speed.baseSpeed = value; }
    }

    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float walkMultiplier;
    [SerializeField] private CharacterController controller;
    private Vector3 moveDirection;
    private bool isSprinting = false;

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


}
