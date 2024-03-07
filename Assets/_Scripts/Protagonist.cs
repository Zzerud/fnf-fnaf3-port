using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Protagonist",menuName = "Create new Protagonist")]
public class Protagonist : Character
{
    [Header("Protagonist Properties")] public bool noMissAnimations;
    public AnimatorOverrideController deathAnimator;
    public bool isVideoDeath;
    public VideoClip videoDeath;
    public AudioClip audioDeath;
}
