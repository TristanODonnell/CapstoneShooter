using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[CustomEditor(typeof(gricel.HealthSystem))]
public class Editor_HealthImportHitbox : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GUILayout.BeginVertical();
		var t = (gricel.HealthSystem)target;
		var hitboxes = t.GetComponentsInChildren<Hitbox>(true);

		if (hitboxes.Length == 0)
		{
			if (GUILayout.Button("Add Hitboxes"))
			{
				SetHitBoxesInRenderers(t);
				EditorUtility.SetDirty(t);
			}
		}
		else
		{
			for (int i = 0; i < 5; i++)
				GUILayout.Label(" ");
			if (GUILayout.Button("Delete Hitboxes"))
			{
				DeleteHitboxes(hitboxes);
				EditorUtility.SetDirty(t);
			}
		}

		GUILayout.EndVertical();
	}

	private void DeleteHitboxes(Hitbox[] hitboxes)
	{
		foreach (var i in hitboxes)
		{
			DestroyImmediate(i.GetComponent<BoxCollider>(), true);
			DestroyImmediate(i.GetComponent<Hitbox>(), true);
		}
	}

	private void SetHitBoxesInRenderers(gricel.HealthSystem t)
	{
		var renderers = t.gameObject.GetComponentsInChildren<Renderer>();
		foreach (var i in renderers)
		{
			if (i.gameObject == t.gameObject)
				continue;
			i.gameObject.AddComponent<Hitbox>();
		}
	}

}
