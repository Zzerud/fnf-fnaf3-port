using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class redImage : MonoBehaviour
{
    public static redImage instance { get; private set; }

    public Image red;
    public Sprite reeed;
    private void Start()
    {
        if(instance == null) instance = this;
        red.sprite = reeed;
        red.CrossFadeAlpha(0, 0, false);
    }

    public void startRed()
    {
        red.CrossFadeAlpha(1, 15f, false);
    }
    public void endRed()
    {
        red.CrossFadeAlpha(0, 3f, false);
    }
}
