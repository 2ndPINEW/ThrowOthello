﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    float StartAttitude = 0;

    void Start()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Quaternion attitude = Input.gyro.attitude;
            attitude.x *= -1;
            attitude.y *= -1;
            transform.localRotation = Quaternion.Euler(90, StartAttitude, 0) * attitude;
        }
    }

    private void OnGUI()
    {
        var rect = new Rect(30, 100, 500, 50);
        GUI.skin.label.fontSize = 30;
        GUI.Label(rect, string.Format("X={0:F2}, Y={1:F2}, Z={2:F2}",
            transform.forward.x, transform.forward.y, transform.forward.z));
    }

    public float ResetCamera()
    {
        StartAttitude = Input.gyro.attitude.eulerAngles.x;
        //60~280
        //0~60, 280~360
        return StartAttitude;
    }
}