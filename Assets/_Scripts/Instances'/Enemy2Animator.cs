using SimpleSpriteAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Animator : MonoBehaviour
{
    public static Enemy2Animator instance { get; private set; }
    public SpriteAnimator animator;
    public SpriteAnimator[] additionalAnimators;
    public float[] enemyAdditionalIdleTimer;
    public float[] _currentEnemyAdditionalIdleTimer;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
}
