using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Detection : MonoBehaviour
{
	public static Player_Detection singleton { get; private set; }
	private void Awake()
	{
		singleton = this;
	}
}
