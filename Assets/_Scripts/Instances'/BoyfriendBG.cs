using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyfriendBG : MonoBehaviour
{
    public static BoyfriendBG instance { get; private set; }
    public SpriteRenderer bf;
    public Animator bfAnim;

    public Vector3 offset;

    private void Start()
    {
        instance = this;
    }
    public void OnEndAnim()
    {
        MainEventSystem.instance.isAnimationOverBF = true;
        bfAnim.SetTrigger("EndAnim");
    }
    public void OnCameraMovement()
    {
        CameraMovement.instance.playerTwoOffset = offset;
    }
}
