using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconPlayer2 : MonoBehaviour
{
    public static IconPlayer2 Instance { get; private set; }
    public Animator anim;

    private void Start()
    {
        Instance = this;
    }
    public void End()
    {
        anim.enabled = false;
    }

}
