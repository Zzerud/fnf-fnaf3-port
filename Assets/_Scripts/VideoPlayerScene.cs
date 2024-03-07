using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.IO;

public class VideoPlayerScene : MonoBehaviour
{
    public static VideoClip videoToPlay;

    public static string nextScene = "Title";

    public VideoPlayer videoPlayer;
    public VideoClip videoFileName;

    public TMP_Text skipText;
    public GameObject skipObj;
    public bool isSkip = false;
    [Space] public string defaultVideo;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Scene Loaded");
        videoPlayer.prepareCompleted += PrepareCompleted;

        skipText.text = Application.systemLanguage == SystemLanguage.Russian ?
            "Нажмите в любое место чтобы пропустить видео" :
            "Click anywhere to skip the video";


        if (videoToPlay != null)
        {
            videoPlayer.clip = videoToPlay;
            videoPlayer.EnableAudioTrack(0, true);
            videoPlayer.Prepare();
            Debug.Log("Video is running!");

        }
        else
        {
            Debug.Log("Scene Scip");
            StartCoroutine(EndVideo());
        }
    }

    private void PrepareCompleted(VideoPlayer source)
    {
        
        StartCoroutine(nameof(EndVideo));

        LoadingTransition.instance.Hide();
    }       
    public void IsSkip()
    {
        isSkip = true;
    }

    IEnumerator EndVideo()
    {
        yield return new WaitForSecondsRealtime(2);
        videoPlayer.Play();
        yield return new WaitForSecondsRealtime(0);
        skipObj.SetActive(true);
        if(videoToPlay == null) SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        yield return new WaitUntil(() => !videoPlayer.isPlaying || isSkip);
        if (videoPlayer.isPlaying) videoPlayer.Pause();
        skipText.gameObject.SetActive(false);
        LoadingTransition.instance.Show(() => {
            SceneManager.LoadScene(nextScene);
            //InterstitialAdShows.RequestInterstitial();
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
