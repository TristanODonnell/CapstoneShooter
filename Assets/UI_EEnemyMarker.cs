using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EEnemyMarker : MonoBehaviour
{
    [SerializeField] private Image ui_compassCenter;
    private Camera ui_player;
    private gricel.Enemy ui_ClosestEnemy;
    private Countdown_AutoReset ui_GetNewClosestEnemy = new(2f);

    void E_GetClosestEnemy()
    {
        float distance = float.MaxValue;
        var enemies = FindObjectsOfType<gricel.Enemy>();
        foreach (var enemy in enemies)
        {
            var enPos = enemy.transform.position;
            var plPos = ui_player.transform.position;
            var bothDistance = (enPos - plPos).sqrMagnitude;

			if (bothDistance < distance)
            {
                distance = bothDistance;
                ui_ClosestEnemy = enemy;
            }
        }
    }

    void Update()
    {
        if (E_TryLocatePlayer())
            return;
        if (ui_GetNewClosestEnemy.CountdownReturn())
            E_GetClosestEnemy();
        if(ui_ClosestEnemy == null)
        {
            ui_GetNewClosestEnemy.Countdown_ForceSeconds(0);
            return;
        }
        E_UpdateCompass();
	}

	private void E_UpdateCompass()
	{
        ui_compassCenter.gameObject.SetActive(true);

        var rot = Quaternion.LookRotation(ui_ClosestEnemy.transform.position - ui_player.transform.position).eulerAngles;
        var player = ui_player.transform.rotation.eulerAngles;
        rot -= player;
        rot.x = rot.z = 0;
        ui_compassCenter.transform.rotation = Quaternion.Euler(0, 0, -rot.y);

	}
    private void Start() => ui_compassCenter.gameObject.SetActive(false);

	private bool E_TryLocatePlayer()
	{
		if (ui_player != null)
			return false;
		ui_player = FindObjectOfType<Camera>();

		return true;
	}
}
