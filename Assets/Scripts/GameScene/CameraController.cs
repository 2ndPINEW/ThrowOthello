using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
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
            transform.localRotation = Quaternion.Euler(90, 0, 0) * attitude;
        }
    }
}