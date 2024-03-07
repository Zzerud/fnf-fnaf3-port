using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

public class endoRevengoScript : MonoBehaviour
{
    public GameObject blackAndWhite, scene1;
    public SpriteRenderer gf, gfAnim;
    public GameObject dad;
    public PlayableAsset anim2, anim3;

    [Header("Flashes")]
    public SpriteRenderer blackFlash;
    public SpriteRenderer whiteFlash;

    public PlayableDirector director;

    private bool isHere = false;
    private void Start()
    {
        blackAndWhite.SetActive(true);

        scene1.SetActive(true);

        whiteFlash.DOFade(0, 0);
        blackFlash.DOFade(1, 0);
    }
    private void Update()
    {
        if (!isHere)
        {
            DadBackup.instance.dad.enabled = false;
            CameraMovement.instance.focusOnPlayerOne = true;

        }
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

    public void Hi()
    {
        isHere = true;
        CameraShake.instance.FlashBlack("1.2");

        DadBackup.instance.dad.enabled = true;
        MainEventSystem.instance.PlayCutSceneEnemy("Hello_Endo");
    }
    public void Shake()
    {
        CameraShake.instance.StartShake(.2f, 2.2f);
        //Endo2Here();
    }
    public void Endo2Here()
    {
        dad.GetComponent<Animator>().enabled = true;
        dad.GetComponent<SpriteRenderer>().enabled = true;

        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("endo02");
        CameraShake.instance.FlashBlack("1.2");
        gf.enabled = true;
        DadBackup.instance.dad.enabled = false;
        director.Play();
    }
    public void Endo01AnimationPlay()
    {
        gf.enabled = false;
        gfAnim.enabled = true;
    }
    public void PlayEndo2()
    {
        dad.GetComponent<Animator>().enabled = false;
        dad.GetComponent<SpriteRenderer>().enabled = false;
        director.Stop();
        DadBackup.instance.dad.enabled = true;
        CameraShake.instance.Flash("1.1");
        AddCameraZoom("0.2");
        ToggleSubtitles();
        gf.enabled = true;
        gfAnim.gameObject.GetComponent<Animator>().enabled = false;
        gfAnim.enabled = false;
    }
    public void playChar(bool onOff)
    {
        DadBackup.instance.dad.enabled = onOff;
        DadBG.instance.dad.enabled = !onOff;
        dad.GetComponent<Animator>().enabled = !onOff;
        dad.GetComponent<SpriteRenderer>().enabled = !onOff;
    }
    public void playGf(bool onOff)
    {
        gf.enabled = onOff;
        gfAnim.gameObject.GetComponent<Animator>().enabled = !onOff;
        gfAnim.enabled = !onOff;

    }
    public void Anim2()
    {
        director.playableAsset = anim2;
        director.Play();

        dad.GetComponent<Animator>().enabled = true;
        dad.GetComponent<SpriteRenderer>().enabled = true;

        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("endosb");
        CameraShake.instance.FlashBlack("1.2");
        DadBackup.instance.dad.enabled = false;
    }
    public void PlayEndoSb()
    {
        dad.GetComponent<Animator>().enabled = false;
        dad.GetComponent<SpriteRenderer>().enabled = false;
        director.Stop();
        DadBackup.instance.dad.enabled = true;
        CameraShake.instance.Flash("1.1");
        AddCameraZoom("0.2");
        ToggleSubtitles();
        gf.enabled = true;
        gfAnim.gameObject.GetComponent<Animator>().enabled = false;
        gfAnim.enabled = false;
    }
    public void Anim3()
    {
        director.playableAsset = anim3;
        director.Play();
        dad.GetComponent<Animator>().enabled = true;
        dad.GetComponent<SpriteRenderer>().enabled = true;
        CameraShake.instance.FlashBlack("1.2");
        DadBackup.instance.dad.enabled = false;
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
