using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ShopManager))]
public class ShopInspectorToolEditor : Editor
{

    private ShopManager shopManager;

    void OnEnable()
    {
        
        shopManager = (ShopManager)target;
        ShopManager.singleton = shopManager;
    }

    /*
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Items:");

        if (ShopManager.singleton != null)
        {
            foreach (var pair in shopManager.items)
            {
                EditorGUILayout.LabelField(pair.Key.ToString(), EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical();
                foreach (var item in pair.Value)
                {
                    EditorGUILayout.LabelField($"Name: {item.name}, Price: {item.price}, Prefab Name: {item.prefabName}, Description: {item.description}, Item Type: {item.itemType}");
                }
                EditorGUILayout.EndVertical();
            }
        }
        else
        {
            Debug.LogError("SingletonShopManager singleton instance is null!");
        }

    }
    */
} 
