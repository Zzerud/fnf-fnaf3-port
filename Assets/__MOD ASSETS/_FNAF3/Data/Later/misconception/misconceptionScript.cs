using BrewedInk.CRT;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;

public class misconceptionScript : MonoBehaviour
{
    public GameObject blackAndWhite, scene1, scene2;

    [Header("Flashes")]
    public SpriteRenderer blackFlash;
    public SpriteRenderer whiteFlash;


    private void Start()
    {
        blackAndWhite.SetActive(true);
        whiteFlash.DOFade(0, 0);
        blackFlash.DOFade(0, 0);

        scene1.SetActive(true);
        scene2.SetActive(false);
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

    public void Wha()
    {
        AddCameraZoom("1.5");
        CameraShake.instance.Flash("1.1");
        scene1.SetActive(false);
        scene2.SetActive(true);
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("fred");
        MainEventSystem.instance.ChangeCharacterPlayer("frank");
    }
    public void Normall()
    {
        AddCameraZoom("1.5");
        CameraShake.instance.Flash("1.1");
        ChangeWhiteFade("outBlack", "0.5");
        scene2.SetActive(false);
        scene1.SetActive(true);
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("purple freddy");
        MainEventSystem.instance.ChangeCharacterPlayer("purple dad");
    }
    public void End()
    {
        ChangeWhiteFade("inBlack", "0.001");
        GamePlayCamera.instance.cam.enabled = false;

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
