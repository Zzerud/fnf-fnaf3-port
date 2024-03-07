using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadBG : MonoBehaviour
{
    public static DadBG instance { get; private set; }
    public SpriteRenderer dad;
    public Animator dadAnim;

    public bool isDev = false;

    private void Start()
    {
        instance = this;
    }
    public void OnEndAnim()
    {
        MainEventSystem.instance.isAnimationOver = true;
        dadAnim.SetTrigger("EndAnim");
        CameraMovement.instance.focusWhenAnimations = false;
    }

    public void OnCameraOnEnemy()
    {
        CameraMovement.instance.focusWhenAnimations = true;
    }
   

}
