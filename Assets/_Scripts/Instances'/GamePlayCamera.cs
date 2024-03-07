using BrewedInk.CRT;
using Kino;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayCamera : MonoBehaviour
{
    public static GamePlayCamera instance;
    public Camera cam;
    public AnalogGlitch analog;
    public DigitalGlitch digital;
    public GameObject postFXX;
    public CRTCameraBehaviour crtc;
    private void Start()
    {
        instance = this;
    }
}
