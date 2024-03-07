using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonsense : MonoBehaviour
{
    public static nonsense instance { get; private set; }

    public Character nonsenses;
    public SpriteRenderer nonsensesRenderer;
    public Animator anim;
    private void Start()
    {
        instance = this;
    }

    public void ChangePerson()
    {
        nonsensesRenderer.enabled = false;
        anim.enabled = false;
        DadBackup.instance.dadTransform.position = this.transform.position;
        DadBackup.instance.dadSpriteAnimator.spriteAnimations = nonsenses.animations;
        DadBackup.instance.dadSpriteAnimator.Start();

        Song.instance.enemy = nonsenses;

        IconPlayer2.Instance.anim.Play("changeIconToNonsense");
        nonsensica.instance.nonsensicasRenderer.enabled = true;
        nonsensica.instance.anim.enabled = true;
    }
}
