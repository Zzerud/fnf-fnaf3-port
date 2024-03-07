using DG.Tweening;
using SimpleSpriteAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadBackup : MonoBehaviour
{
    public static DadBackup instance { get; private set; }
    public SpriteRenderer dad;
    public SpriteAnimator dadSpriteAnimator;
    public Transform dadTransform;

    public bool isFlying;

    private void Start()
    {
        dad.DOFade(1, 0);
        if(instance == null)
            instance = this;
        if (Song.currentSong.name == "nyan" || Song.currentSong.name == "nyanstep")
        {
            isFlying = true;
        }
    }
    private void Update()
    {
        if (isFlying)
        {
            //dadTransform.transform.position = new Vector2(transform.position.x, Mathf.Sin(Time.time * 4) * 0.5f + 0.5f);
            dadTransform.transform.position = new Vector3(Mathf.Cos(Time.time * 6) * 0.5f-4.33f, Mathf.Sin(Time.time * 6) * 0.5f-2.92f, transform.position.z);
        }
    }
    public void Change(Character dad)
    {
        dadSpriteAnimator.spriteAnimations = dad.animations;
        dadSpriteAnimator.Start();
    }

    public void fadeDad(float to, float time)
    {
        dad.DOFade(to, time);
    }
}
