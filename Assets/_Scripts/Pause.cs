using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    public GameObject pauseScreen;
    public bool editingVolume;
    public bool canPause = true;
    
    public static Pause instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(Player.pauseKey) || Input.GetKeyDown(KeyCode.Escape)) & !Player.demoMode & !Song.instance.isDead & Song.instance.songStarted & canPause)
        {
            if(!pauseScreen.activeSelf)
                PauseSong();
        }
                
        if (Player.demoMode && Input.GetKeyDown(Player.pauseKey))
        {
            QuitSong();
        }
    }

    public void EditVolume()
    {
        pauseScreen.SetActive(false);
        Menu.instance.menuCanvas.enabled = true;
        Song.instance.battleCanvas.enabled = false;
        //OptionsV2.instance.volumeScreen.SetActive(true);
        Menu.instance.mainMenu.SetActive(false);
        editingVolume = true;
    }

    public void SaveVolume()
    {
        pauseScreen.SetActive(true);
        Song.instance.battleCanvas.enabled = true;
        Menu.instance.menuCanvas.enabled = false;
        //OptionsV2.instance.volumeScreen.SetActive(false);

        LeanTween.delayedCall(1.25f, () =>
        {
            //OptionsV2.instance.mainOptionsScreen.SetActive(false);
            Menu.instance.mainMenu.SetActive(true);
        });
        editingVolume = false;
    }
    
    public void PauseSong()
    {
        /*if (!AudioAutoPause._instance._isMenu)
        {
            AudioAutoPause._instance._isFocused = false;
            AudioAutoPause._instance._isMenu = true;
            Song.instance.stopwatch.Stop();
            Song.instance.beatStopwatch.Stop();
        }*/

        //Time.timeScale = 0;

        // Song.instance.modInstance?.Invoke("OnPause");

        Song.instance.subtitleDisplayer.paused = true;

        try
        {
            if (Song.instance.stopwatch != null)
                Song.instance.stopwatch.Stop();
            if (Song.instance.beatStopwatch != null)
                Song.instance.beatStopwatch.Stop();
        }
        catch (Exception)
        {


        }

        Time.timeScale = 0f;

        foreach (AudioSource source in Song.instance.musicSources)
         {
             source.Pause();
         }
        Song.instance.vocalSource.Pause();
       

        pauseScreen.SetActive(true);
        AudioListener.pause = true;
    }

    public void ContinueSong()
    {
        //Song.instance.modInstance?.Invoke("OnUnpause");
        Time.timeScale = 1;
        Song.instance.stopwatch.Start();
        Song.instance.beatStopwatch.Start();

        Song.instance.subtitleDisplayer.paused = false;

        foreach (AudioSource source in Song.instance.musicSources)
        {
            source.UnPause();
        }
        AudioListener.pause = false;
        Song.instance.vocalSource.UnPause();

        pauseScreen.SetActive(false);
    }

    public void RestartSong()
    {
        LoadingTransition.instance.Show(() => SceneManager.LoadScene("Game_Backup3"));
    }

    public void QuitSong()
    {
        Time.timeScale = 1;
        SpawnPointManager.timeLastSpawnPoint = 0;
        SpawnPointManager.timeLastSpawnPointMilliseconds = 0;

        /*ContinueSong();
        Song.instance.subtitleDisplayer.StopSubtitles();
        /*foreach (AudioSource source in Song.instance.musicSources)
        {
            source.Stop();
        }

        Song.instance.vocalSource.Stop();
        Song.instance.musicSourceAndr[0].Stop();
        Song.instance.vocalSourceAndr.Stop();*/
        SceneManager.LoadScene("Title");
    }
}
