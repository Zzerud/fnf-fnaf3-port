using SimpleSpriteAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorOnButtonInTitle : MonoBehaviour
{
    public int id;
    private Image anim;
    public static ColorOnButtonInTitle instance;

    public void Start()
    {
        if (instance == null) instance = this;
        anim.color = Song.instance.player1NoteColors[id];
    }
}
