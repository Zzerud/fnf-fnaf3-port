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


    public string urlToVideo = "";
    public static string videoPlay = "";


    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(PlayVid());
    }
    IEnumerator PlayVid()
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
            yield return null;
        }
        else
        {
            Debug.Log("Scene Scip");
            StartCoroutine(EndVideo());
        }
        /*if(videoPlay != "")
        {
#if !UNITY_EDITOR
            urlToVideo = "jar:file://" + Application.dataPath + "!/assets/" + videoPlay + ".mp4";

            using (var www = new WWW(urlToVideo))
            {
                yield return www;
                videoPlayer.url = urlToVideo;
                videoPlayer.Prepare();
            }
#elif UNITY_EDITOR
            urlToVideo = Path.Combine(Application.streamingAssetsPath, videoPlay + ".mp4");
            videoPlayer.url = urlToVideo;
            videoPlayer.Prepare();
            yield return null;
#endif
        }
        else
        {
            StartCoroutine(EndVideo());
        }*/
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
        //yield return new WaitForSecondsRealtime(2);
        videoPlayer.Play();
        yield return new WaitForSecondsRealtime(0);
        skipObj.SetActive(true);
        if(videoToPlay == null) SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        yield return new WaitUntil(() => !videoPlayer.isPlaying || isSkip);
        if (videoPlayer.isPlaying) videoPlayer.Pause();
        skipText.gameObject.SetActive(false);
        LoadingTransition.instance.Show(() => {
            SceneManager.LoadScene(nextScene);
        });
        
    }

}
