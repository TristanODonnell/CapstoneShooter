using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Detection : MonoBehaviour
{
	public static Player_Detection singleton { get; private set; }
	public static Vector3 position => singleton.transform.position;
	private void Awake()
	{
		singleton = this;
	}
}
