using BrewedInk.CRT;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;

public class leantrapScript : MonoBehaviour
{
    public GameObject blackAndWhite, scene1, scene2, dont;

    

    [Header("Flashes")]
    public SpriteRenderer blackFlash;
    public SpriteRenderer whiteFlash;


    private void Start()
    {
        blackAndWhite.SetActive(true);

        dont.SetActive(false);
        scene1.SetActive(true);
        scene2.SetActive(false);

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
    public void Tablet()
    {
        MainEventSystem.instance.OnTablet();
    }

    public void Lean()
    {
        CameraShake.instance.purple.CrossFadeAlpha(1, 0, false);
        CameraShake.instance.purple.CrossFadeAlpha(0, 1, false);
        GamePlayCamera.instance.gameObject.AddComponent<DistortionEffect>();
        MainEventSystem.instance.ChangeCharacterPlayer("bflean2");
        AddCameraZoom("0.7");
    }
    public void Lol()
    {
        MainEventSystem.instance.ChangeCharacterPlayer("nobfbb");
        BoyfriendBackup.instance.bf.enabled = false;
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("leantrap2");
        scene1.SetActive(false);
        scene2.SetActive(true);
        AddCameraZoom("0.7");
    }
    public void End()
    {
        dont.SetActive(true);
        GamePlayCamera.instance.cam.enabled = false;
        Destroy(GamePlayCamera.instance.gameObject.GetComponent<DistortionEffect>());
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
