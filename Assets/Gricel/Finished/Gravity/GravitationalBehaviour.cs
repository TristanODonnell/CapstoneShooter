using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalBehaviour : MonoBehaviour
{
	public float currentGravity = -9.81f;
	private void Start()
	{
		currentGravity = SceneGravity.GlobalGravity;
	}
}
