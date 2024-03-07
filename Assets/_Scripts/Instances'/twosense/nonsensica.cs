using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonsensica : MonoBehaviour
{
    public static nonsensica instance { get; private set; }

    public Character nonsensicas;
    public SpriteRenderer nonsensicasRenderer;
    public Animator anim;
    private void Start()
    {
        instance = this;
    }

    public void ChangePerson()
    {
        nonsensicasRenderer.enabled = false;
        anim.enabled = false;
        DadBackup.instance.dadTransform.position = transform.position;
        DadBackup.instance.dadSpriteAnimator.spriteAnimations = nonsensicas.animations;
        DadBackup.instance.dadSpriteAnimator.Start();

        Song.instance.enemy = nonsensicas;

        IconPlayer2.Instance.anim.enabled = true;
        IconPlayer2.Instance.anim.Play("changeIcon");
        nonsense.instance.nonsensesRenderer.enabled = true;
        nonsense.instance.anim.enabled = true;
    }
}
