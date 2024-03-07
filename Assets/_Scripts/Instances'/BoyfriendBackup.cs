using DG.Tweening;
using SimpleSpriteAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoyfriendBackup : MonoBehaviour
{
    public static BoyfriendBackup instance { get; private set; }
    public SpriteRenderer bf;
    public SpriteAnimator bfAnim;
    public GameObject[] effects;
    public Transform bfTransform;

    private void Start()
    {
        instance = this;
        bf.DOFade(1, 0);
        effects[0].GetComponent<SpriteRenderer>().color = new Color(ColorPicker.down.r, ColorPicker.down.g, ColorPicker.down.b);
        effects[1].GetComponent<SpriteRenderer>().color = new Color(ColorPicker.left.r, ColorPicker.left.g, ColorPicker.left.b);
        effects[2].GetComponent<SpriteRenderer>().color = new Color(ColorPicker.right.r, ColorPicker.right.g, ColorPicker.right.b);
        effects[3].GetComponent<SpriteRenderer>().color = new Color(ColorPicker.up.r, ColorPicker.up.g, ColorPicker.up.b);
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].SetActive(false);
        }
        if (Song.currentSong.name == "Rethink")
        {
            fadeBF(0, 0);
        }
    }
    public void Change(Protagonist bf)
    {
        bfAnim.spriteAnimations = bf.animations;
        bfAnim.Start();
    }

    public void fadeBF(float to, float time)
    {
        bf.DOFade(to, time);
    }
}
