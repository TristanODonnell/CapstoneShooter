using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalBehaviour : MonoBehaviour
{
	public float currentGravity = -9.81f;
	[SerializeField] private CharacterController characterController;
	private float countGravity;
	private float jumpPower;
	private float fallTime;

	public bool isJumping => fallTime > 0.2f;
	private void OnValidate()
	{
		characterController = GetComponent<CharacterController>();
	}

	public void Jump(float power)
	{
		jumpPower = Mathf.Abs(power);
		countGravity = 0;
	}
	private void Update()
	{

		if (!characterController.isGrounded)
		{
			countGravity -= Time.deltaTime;
			fallTime += Time.deltaTime;
		}

		var gravityFall = Vector3.down * countGravity * Mathf.Abs(countGravity) * currentGravity;
		var gravityJump = Vector3.up * jumpPower;


		characterController.Move((gravityFall + gravityJump) * Time.deltaTime);
		if (characterController.isGrounded) {
			if (countGravity < -0.5f)
			{
				jumpPower = 0;
				countGravity = -0.5f;
			}
			fallTime = 0;
		}
	}
	private void Start()
	{
		currentGravity = SceneGravity.GlobalGravity;
	}
}
