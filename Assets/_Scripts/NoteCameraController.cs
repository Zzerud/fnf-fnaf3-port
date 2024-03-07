using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCameraController : MonoBehaviour
{
    public Camera noteCamera;
    public static NoteCameraController instance { get; private set; }
    private void Start()
    {
        instance = this;        
    }
    public void OnCameraOpen()
    {
        noteCamera.enabled = true;
    }
    public void OnDisabledCamera()
    {
        noteCamera.enabled = false;
    }
}
