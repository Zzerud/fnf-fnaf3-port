using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayedInstance : MonoBehaviour
{
    public static VideoPlayedInstance instance;
    public VideoClip clip;
    public GameObject raw;
    public GameObject raw2;

    public VideoPlayer player;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void PlayVid()
    {
        player.clip = clip;
        player.Play();
    }
}
