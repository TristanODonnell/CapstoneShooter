using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCameraSystem : MonoBehaviour
{
    public LookBehavior lookBehavior;
    public Vector2 lookDirection;
    private Vector3 cameraRecoilOffset = Vector3.zero;
    public bool isAimingDownSight = false;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera adsCamera;
    public void AimDownSightStart()
    {
        isAimingDownSight = true;
        mainCamera.enabled = false;
        adsCamera.enabled = true;
        //mainUICanvas.enabled = false;
        // adsUICanvas.enabled = true;
        // Play ADS animation

    }

    public void AimDownSightEnd()
    {
        isAimingDownSight = false;
        mainCamera.enabled = true;
        adsCamera.enabled = false;
        //mainUICanvas.enabled = true;
        //adsUICanvas.enabled = false;
        // Play ADS animation (reverse)

    }

    
}