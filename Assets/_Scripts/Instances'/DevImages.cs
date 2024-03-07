using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevImages : MonoBehaviour
{
   public static DevImages Instance { get; private set; }

    public GameObject one, two;
    private void Start()
    {
        Instance = this;
    }
}
