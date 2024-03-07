using BrewedInk.CRT;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;

public class partyRoomScript : MonoBehaviour
{
    public GameObject blackAndWhite, scene1;
    public SpriteRenderer dad2, dad3;
    public GameObject bg;

    public VideoClip clip;

    [Header("Flashes")]
    public SpriteRenderer blackFlash;
    public SpriteRenderer whiteFlash;


    private bool isHere = false;
    private void Start()
    {
        blackAndWhite.SetActive(true);

        scene1.SetActive(true);

        whiteFlash.DOFade(0, 0);
        blackFlash.DOFade(0, 0);
    }

    private void Update()
    {
        if (!isHere) DadBackup.instance.dad.enabled = false; 
    }
    public void ChangeWhiteFade(string fadeType, string time)
    {
        float s = float.Parse(time, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
        switch (fadeType)
        {
            case "in":
                whiteFlash.DOFade(1, s);
                break;
            case "out":
                whiteFlash.DOFade(0, s);
                break;
            case "flash":
                whiteFlash.DOFade(1, 0);
                AddCameraZoom("0.3");
                whiteFlash.DOFade(0, s);
                break;
            case "inWithoutBlack":
                blackFlash.DOFade(0, 0);
                whiteFlash.DOFade(1, 0);
                break;
            case "inBlack":
                blackFlash.DOFade(1, s);
                break;
            case "outBlack":
                blackFlash.DOFade(0, s);
                break;
            case "inUI":
                CameraShake.instance.whiteUi.CrossFadeAlpha(1, s, false);
                break;
            case "outUI":
                CameraShake.instance.whiteUi.CrossFadeAlpha(0, s, false);
                break;


        }
    }
    public void Tablet()
    {
        MainEventSystem.instance.OnTablet();
    }

    public void HelloThere()
    {
        AddCameraZoom("2.5");
        CameraShake.instance.Flash("1.1");
        ToggleSubtitles();
        isHere = true;
        DadBackup.instance.dad.enabled = true;
        MainEventSystem.instance.PlayCutSceneEnemy("Enter");
    }
    public void PaperBonnie()
    {
        CameraShake.instance.FlashBlack("1.1");
        ToggleSubtitles();
        dad2.enabled = true;
        AddCameraZoom("2.5");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("paperfreddy2");
    }
    public void PaperBB()
    {
        CameraShake.instance.Flash("1.1");
        ToggleSubtitles();
        dad3.enabled = true;
        AddCameraZoom("2.5");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("paperfreddy3");
        bg.SetActive(true);

        VideoPlayedInstance.instance.player.clip = clip;
        VideoPlayedInstance.instance.clip = clip;
        VideoPlayedInstance.instance.player.Prepare();
    }
    public void PlayVid()
    {
        VideoPlayedInstance.instance.raw.SetActive(true);
        VideoPlayedInstance.instance.player.Play();
    }
    public void StopVid()
    {
        VideoPlayedInstance.instance.player.Stop();
        VideoPlayedInstance.instance.raw.SetActive(false);
        MainEventSystem.instance.par.SetActive(true);
    }
    public void End()
    {
        MainEventSystem.instance.par.SetActive(false);
        ChangeWhiteFade("inBlack", "13.5");
    }





    #region NeedToAllOfScripts
    public void AddCameraZoom(string to)
    {
        float s = float.Parse(to, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
        Song.instance.mainCamera.orthographicSize -= s;
    }
    [System.Serializable]
    public class Subtitle
    {
        public string textEng;
        public string textRus;
        public string color;
    }
    [Space(20)]
    [Header("Subtitles")]
    public Subtitle[] sub;
    private int indexText = 0;
    public void ToggleSubtitles()
    {
        if (Application.systemLanguage == SystemLanguage.Russian) MainEventSystem.instance.ToggleSubtitles(sub[indexText].textRus, sub[indexText].color);
        else MainEventSystem.instance.ToggleSubtitles(sub[indexText].textEng, sub[indexText].color);
        indexText++;
    }
    public void LoadBGSave(int numberSave)
    {
        switch (numberSave)
        {
            case 1:
                MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("Scarlet");
                break;
            case 2:
                MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("Sweet");
                break;
            case 3:
                MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("LoFi");
                break;
        }
        Song.instance.isLoadedEvents = true;
    }
    #endregion

}
