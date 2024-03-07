using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTransition : MonoBehaviour
{
    public static LoadingTransition instance;

    public UITransitionEffect transitionEffect;

    public TMP_Text loadingText;

    public Image img;
    public Sprite[] imgSprites;

    public bool toggled;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } 
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Time.timeScale = 1;
            try
            {
                Hide();
                if (!Pause.instance.pauseScreen.activeSelf)
                {
                    if (Song.instance.stopwatch != null)
                        Song.instance.stopwatch.Start();
                    if (Song.instance.beatStopwatch != null)
                        Song.instance.beatStopwatch.Start();
                    Time.timeScale = 1;
                }
                Time.timeScale = 1;
            }
                
            catch (Exception)
            {

                
            }
            
        }
        else
        {
            Time.timeScale = 0;
            try
            {
                if (Song.instance.stopwatch != null)
                    Song.instance.stopwatch.Stop();
                if (Song.instance.beatStopwatch != null)
                    Song.instance.beatStopwatch.Stop();
                Time.timeScale = 0f;
            }
            catch (Exception)
            {

               
            }
            

        }

    }


    public void Show(Action action)
    {
        img.sprite = imgSprites[UnityEngine.Random.Range(0, imgSprites.Length)];
        if (!toggled) toggled = true;
        else return;
        transitionEffect.Show();
        LeanTween.value(gameObject, Color.clear, Color.white, .5f).setDelay(transitionEffect.duration).setOnUpdate(val =>
        {
            loadingText.color = val;
        }).setOnComplete(() =>
        {
            LeanTween.delayedCall(.5f, action);
        });
    }

    public void Hide()
    {
        if (toggled) toggled = false;
        else return;
        LeanTween.value(gameObject, Color.white, Color.clear, .5f).setDelay(transitionEffect.duration).setOnUpdate(val =>
        {
            loadingText.color = val;
        }).setOnComplete(() =>
        {
            transitionEffect.Hide();
        });
        
    }
}
