using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(gricel.HealthSystem))]
public class Editor_HealthImportHitbox : Editor
{
	private bool displayTools;
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GUILayout.BeginVertical();
		var t = (gricel.HealthSystem)target;
		var hitboxes = t.GetComponentsInChildren<Hitbox>(true);

		if (!displayTools)
		{
			if (GUILayout.Button("Display tools v"))
				displayTools = true;
			GUILayout.EndVertical();
			return;
		}
		if (displayTools)
		{
			if (GUILayout.Button("Hide tools ^"))
				displayTools = false;
		}

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
			var hitBox = t.gameObject.GetComponentsInChildren<Hitbox>(true);
			foreach (var i in hitBox)
			{
				EditorGUI.BeginChangeCheck();


				GUILayout.BeginHorizontal();
				GUILayout.Label("   ");
				EditorGUILayout.Space();
				GUILayout.Label($"Normal Damage Multiplier {i.damagePercentage * 100f}%");
				if (GUILayout.Button("Select"))
					Selection.activeGameObject = i.gameObject;
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				i.name = EditorGUILayout.TextField(i.name);
				EditorGUILayout.Space();
				i.damagePercentage = EditorGUILayout.Slider(i.damagePercentage, 0f, 10f);
				GUILayout.EndHorizontal();


				if (EditorGUI.EndChangeCheck())
					EditorUtility.SetDirty(t);
			}


			for (int i = 0; i < 5; i++)
				GUILayout.Label(" ");
			GUILayout.EndVertical();



			GUILayout.BeginHorizontal();
			for (int i = 0; i < 5; i++)
				GUILayout.Label(" ");
			if (GUILayout.Button("Delete Hitboxes"))
			{
				DeleteHitboxes(hitboxes);
				EditorUtility.SetDirty(t);
			}
			GUILayout.EndHorizontal();
		}

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
