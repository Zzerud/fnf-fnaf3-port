using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;

public class RetributionScript : MonoBehaviour
{
    public GameObject blackAndWhite, miniGameBg, stageBg, stage2Bg;
    public VideoClip endClip;
    public PostProcessProfile a;

    [Header("Flashes")]
    public SpriteRenderer blackFlash;
    public SpriteRenderer whiteFlash;

    [Header("stage1")]
    public GameObject childrens;
    public GameObject location;
    public GameObject locationWithoutSpring;

    private void Start()
    {
        blackAndWhite.SetActive(true);
        miniGameBg.SetActive(true);
        stageBg.SetActive(false);
        stage2Bg.SetActive(false);

        CameraMovement.instance.volume.profile = a;

        whiteFlash.DOFade(0, 0);
        blackFlash.DOFade(1, 0);
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
    public void ChangeSceneToThirst()
    {
        ChangeWhiteFade("out", "0.5");
        AddCameraZoom("0.5");
        miniGameBg.SetActive(false);
        stageBg.SetActive(true);
        CameraShake.instance.StartShake(0.3f, 0.2f);
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("afton2");
        MainEventSystem.instance.ChangeCharacterPlayer("cassidy");
    }
    public void ChangeSceneToPerspective()
    {
        ChangeWhiteFade("outBlack", "0.8");
        AddCameraZoom("0.5");
        stageBg.SetActive(false);
        stage2Bg.SetActive(true);
        CameraShake.instance.StartShake(0.3f, 0.2f);
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("afton3");
        MainEventSystem.instance.ChangeCharacterPlayer("cassidy2");
    }
    public void BackToThirst()
    {
        ChangeWhiteFade("outBlack", "0.1");
        ChangeWhiteFade("out", "0.97");
        childrens.SetActive(true);
        stageBg.SetActive(true);
        stage2Bg.SetActive(false);
        AddCameraZoom("0.5");

        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("afton2");
        MainEventSystem.instance.ChangeCharacterPlayer("cassidy");
    }
    public void Spring()
    {
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("springbonnie");
        ChangeWhiteFade("outBlack", "0.3");
        location.SetActive(false);
        locationWithoutSpring.SetActive(true);

    }
    public void ChangeHallucinations(string Name, string isOn)
    {
        CameraShake.instance.ChangeHallucinations(Name, isOn);
    }

    public void PrepareForTheClip()
    {
        ChangeWhiteFade("inBlack", "0.97");
        VideoPlayedInstance.instance.clip = endClip;
        VideoPlayedInstance.instance.player.clip = endClip;
        VideoPlayedInstance.instance.player.Prepare();
    }
    public void StartVid()
    {
        VideoPlayedInstance.instance.player.Play();
        VideoPlayedInstance.instance.raw.SetActive(true);
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
