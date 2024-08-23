using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGravity : MonoBehaviour
{
	[SerializeField]
	private float gravity = -9.81f;


	private static SceneGravity singleton;
	private static SceneGravity Singleton()
	{
		if (!singleton)
		{
			singleton = FindObjectOfType<SceneGravity>(true);

			if (!singleton)
				singleton = new GameObject("Scene Gravity").AddComponent<SceneGravity>();

		}
		return singleton;
	}
	private void Awake()
	{
		if (singleton && singleton != this)
			Destroy(gameObject);
		else
			singleton = this;
			
	}

	public static float GlobalGravity => Singleton().gravity;

}
