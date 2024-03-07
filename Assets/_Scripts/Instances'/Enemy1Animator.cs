using SimpleSpriteAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Animator : MonoBehaviour
{
    public static Enemy1Animator instance { get; private set; }
    public SpriteAnimator animator, additionalAnimator;

    private void Start()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }
}
