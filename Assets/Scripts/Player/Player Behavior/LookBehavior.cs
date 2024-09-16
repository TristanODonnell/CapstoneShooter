using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponData;

public class LookBehavior : MonoBehaviour
{
    private Vector2 lookDirection;
    [SerializeField] private float lookSensitivity;
    public Camera myCamera;
    public ShootBehavior shootBehavior;
    private float originalFieldOfView;
    private bool isAimingDownSight = false;
    private float adsZoomLevel;
    private float zoomSensitivity = 0.5f;
    private void Start()
    {
    }
    public void RotatePlayer()
    {
        lookDirection.x += Input.GetAxisRaw("Mouse X") * Time.deltaTime * lookSensitivity;
        lookDirection.y += Input.GetAxisRaw("Mouse Y") * Time.deltaTime * lookSensitivity;


        lookDirection.y = Mathf.Clamp(lookDirection.y, -85f, 85f);

        myCamera.transform.localRotation = Quaternion.Euler(-lookDirection.y, 0, 0);
        transform.rotation = Quaternion.Euler(0, lookDirection.x, 0);

    }
    public void ApplyCameraRecoil(float totalVerticalRecoil, float totalHorizontalRecoil)
    {

        lookDirection.y += totalVerticalRecoil;
        lookDirection.x += totalHorizontalRecoil;
        //Debug.Log($"Test Recoil Applied: lookDirection.x = {lookDirection.x}, lookDirection.y = {lookDirection.y}");
        myCamera.transform.localRotation = Quaternion.Euler(-lookDirection.y, 0, 0);
        transform.rotation = Quaternion.Euler(0, lookDirection.x, 0);
    }
    public void AimDownSightStart()
    {
        try
        {
            if (!isAimingDownSight)
            {
                adsZoomLevel = shootBehavior.currentWeapon.adsZoomLevel / zoomSensitivity;
                originalFieldOfView = myCamera.fieldOfView; // Store the original field of view
                myCamera.fieldOfView = adsZoomLevel; // Set the camera's field of view to the ADS zoom level
                isAimingDownSight = true; // Set the flag to true
            }
        }
        catch
        {
            
        }
    }

    public void AimDownSightEnd()
    {
        if (isAimingDownSight)
        {
            myCamera.fieldOfView = originalFieldOfView; // Reset the camera's field of view
            isAimingDownSight = false; // Set the flag to false
        }
    }

    public bool IsAimingDownSight()
    {
        return isAimingDownSight; // Return the current state of the flag
    }

}
 