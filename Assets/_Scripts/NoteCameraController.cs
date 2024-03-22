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
        if (OptionsV2.Downscroll)
        {
            noteCamera.transform.position = new Vector3(0, 7, -10);
        }
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
