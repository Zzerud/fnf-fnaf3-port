using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadBearInstance : MonoBehaviour
{
    public static MadBearInstance instance;
    public Animator anim;
    private void Start()
    {
        instance= this;
    }
}
