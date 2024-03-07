using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;

public class TakenApartScript : MonoBehaviour
{
    public GameObject miniGameBg, takenBg, runBg;
    public VideoClip clip;
    [Space(20)]
    #region MiniGame
    public Sprite[] startLocations;
    public SpriteRenderer startBg;
    #endregion
    [Space(20)]
    #region Taken
    public SpriteRenderer TakenBg;
    public Sprite[] takenBgSprites;
    public SpriteRenderer podsos;
    #endregion
    [Space(20)]
    #region Run
    public Animator anim;
    #endregion

    private int TakenScene = 0;
    private int RunScene = 0;
    public PostProcessProfile a;

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

    private void Start()
    {
        CameraMovement.instance.volume.profile = a;


        miniGameBg.SetActive(true);
        takenBg.SetActive(false);
        runBg.SetActive(false);
        podsos.DOFade(0f, 0f);

        podsos.enabled = false;
        StartCoroutine(FirstBG());
        DadBackup.instance.dad.enabled = false;
        BoyfriendBackup.instance.bf.enabled = false;

        Song.instance.mainCamera.orthographicSize = 2.04f;
        CameraMovement.instance.enableMovement = false;
        Song.instance.mainCamera.transform.position = new Vector3(1.01999998f, 4.11999989f, -1f);

    }
    private void Update()
    {
        if (miniGameBg.activeSelf)
        {
            Song.instance.mainCamera.orthographicSize = 2.04f;
        }
        if (runBg.activeSelf)
        {
            Song.instance.mainCamera.orthographicSize = 3;

        }
    }
    IEnumerator FirstBG()
    {
        int i = 0;
        while (true)
        {
            startBg.sprite = startLocations[i];
            i++;
            if (i == startLocations.Length) i = 0;
            yield return new WaitForSeconds(0.7f);
        }
    }

    public void ChangeSceneToTaken()
    {
        CameraMovement.instance.enableMovement = true;

        StopAllCoroutines();
        miniGameBg.SetActive(false);
        takenBg.SetActive(true);
        runBg.SetActive(false);

        Song.instance.mainCamera.orthographicSize = 5.5f;
        CameraMovement.instance.enableMovement = true;

        podsos.enabled = true;
        podsos.DOFade(100f, 0f);
        podsos.DOFade(0f, 9f);
        Song.instance.mainCamera.orthographicSize -= 0.5f;
        DadBackup.instance.dad.enabled = true;
        BoyfriendBackup.instance.bf.enabled = true;
        CameraShake.instance.Flash("1.6");
        CameraShake.instance.StartShake(0.5f, 1.2f);
        TakenBg.sprite = takenBgSprites[TakenScene];
        TakenScene++;
    }
    public void Run()
    {
        takenBg.SetActive(false);
        runBg.SetActive(true);
        switch (RunScene)
        {
            case 0:
                anim.SetTrigger("Bonnie");
                break;
            case 1:
                anim.SetTrigger("Chica");
                break;
            case 2:
                anim.SetTrigger("Foxy");
                break;
        }
        MainEventSystem.instance.Static("3");
        DadBackup.instance.dad.enabled = false;
        BoyfriendBackup.instance.bf.enabled = false;
        //camera pos
        CameraMovement.instance.enableMovement = false;
        Song.instance.mainCamera.transform.position = new Vector3(-1f, 1f, -55);

        GamePlayCamera.instance.cam.enabled = false;
    }
    // Do zoom later plz
    public void CameraToShow()
    {
        //zoom
        GamePlayCamera.instance.cam.enabled = true;
        switch (RunScene)
        {
            case 0:
                MainEventSystem.instance.ChangeCharacterPlayer("Bonnie");
                break;
            case 1:
                MainEventSystem.instance.ChangeCharacterPlayer("Chica");
                break;
        }
    }
    public void ShowPerson()
    {
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("Afton");
        switch (RunScene)
        {
            case 2:
                MainEventSystem.instance.ChangeCharacterPlayer("Foxy");
                MainEventSystem.instance.jumpScare.gameObject.SetActive(false);
                CameraShake.instance.Flash("0.8");
                break;
        }
        RunScene++;
        ChangeSceneToTaken();
    }
    public void FadeStatic()
    {
        MainEventSystem.instance.whiteStatic.CrossFadeAlpha(1, 0, false);
        MainEventSystem.instance.whiteStatic.CrossFadeAlpha(0, 1, false);
        MainEventSystem.instance.staticss.CrossFadeAlpha(0, 0, false);
        MainEventSystem.instance.ToggleSubtitles();
    }
    public void BlackFoxy()
    {
        CameraShake.instance.flashBlack.CrossFadeAlpha(1, 1, false);
        GamePlayCamera.instance.cam.enabled = true;
    }
    public void JumpScare()
    {
        MainEventSystem.instance.jumpScare.gameObject.SetActive(true);
        CameraShake.instance.flashBlack.CrossFadeAlpha(0, 0, false);
        MainEventSystem.instance.jumpScare.SetTrigger("FoxyJump");
        VideoPlayedInstance.instance.clip = clip;
        VideoPlayedInstance.instance.player.clip = clip;
        VideoPlayedInstance.instance.player.Prepare();
    }
    public void Black()
    {
        CameraShake.instance.flashBlack.CrossFadeAlpha(1, 0, false);
        MainEventSystem.instance.staticss.CrossFadeAlpha(0, 4, false);
    }
    public void StartVid()
    {
        VideoPlayedInstance.instance.player.Play();
        VideoPlayedInstance.instance.raw.SetActive(true);
    }
}
