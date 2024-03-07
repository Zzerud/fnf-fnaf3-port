using BrewedInk.CRT;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;

public class everlastingScript : MonoBehaviour
{
    public GameObject guide, blackAndWhite, scene1, scene2, sceneRun, scene3, scene4, scene5;
    [Space(15)]
    public VideoClip clipStart, clipFight;
    public VideoClip deathBf2;
    public CRTDataObject conf;
    public VideoPlayer vid, vid2;

    [Space(15)]
    public SpriteRenderer firstBgLayout;
    public SpriteRenderer camstatic, cam5, cam2;

    [Header("Flashes")]
    public SpriteRenderer blackFlash;
    public SpriteRenderer whiteFlash;


    private void Start()
    {
        blackAndWhite.SetActive(true);
        CameraMovement.instance.GetComponent<CRTCameraBehaviour>().startConfig = conf;

        VideoPlayedInstance.instance.clip = clipStart;
        VideoPlayedInstance.instance.player.clip = clipStart;
        VideoPlayedInstance.instance.player.Prepare();

        scene1.SetActive(true);
        scene2.SetActive(false);
        sceneRun.SetActive(false);
        scene3.SetActive(false);
        scene4.SetActive(false);
        scene5.SetActive(false);


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
        Red();
    }
    public void ShowVideo()
    {
        guide.SetActive(false);
        VideoPlayedInstance.instance.raw.SetActive(true);
        VideoPlayedInstance.instance.player.Play();
    }
    public void VidEnd()
    {
        VideoPlayedInstance.instance.raw.SetActive(false);
        VideoPlayedInstance.instance.player.Stop();
        ChangeWhiteFade("outBlack", "0.5");
    }
    public void Scream()
    {
        AddCameraZoom("0.1");
        Tablet();
        CameraShake.instance.Flash("1.3");
    }
    public void Shake()
    {
        AddCameraZoom("0.6");

        CameraShake.instance.Flash("1.2");
        CameraShake.instance.StartShake(0.1f, 0.2f);

    }
    
    public void Red()
    {
        StartCoroutine(r());
    }
    public void StopRed()
    {
        StopAllCoroutines();
        MainEventSystem.instance.redAlways.CrossFadeAlpha(0f, 0f, false);

    }
    IEnumerator r()
    {
        while (true)
        {
            MainEventSystem.instance.redAlways.CrossFadeAlpha(0.2f, 0f, false);
            MainEventSystem.instance.redAlways.CrossFadeAlpha(0f, 0.3f, false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    public void OnSpringtrapOut()
    {
        StopRed();
        AddCameraZoom("0.5");
        MainEventSystem.instance.PlayCutSceneEnemy("walk");
        CameraShake.instance.Flash("0.5");
        LeanTween.delayedCall(2.45f, () =>
        {
            CameraShake.instance.FlashBlack("0.5");
        });
    }
    public void Camstatic(string fade, string timeFade)
    {
        float s = float.Parse(timeFade, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);

        switch (fade)
        {
            case "fadein":
                camstatic.DOFade(1, s);
                break;
            case "fadeout":
                camstatic.DOFade(0, s);
                break;
        }
    }
    public void Spring2()
    {
        DadBG.instance.OnEndAnim();
        DadBackup.instance.dad.enabled = true;

        firstBgLayout.sortingOrder = 4;
        camstatic.DOFade(1, 0);
        camstatic.DOFade(0, 1);
        cam5.DOFade(1, 0);
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("springtrap2");
        MainEventSystem.instance.ChangeCharacterPlayer("bfnonever2");
        LeanTween.delayedCall(0.2f, () =>
        {
            Vector3 sc = cam5.transform.localScale;
            DadBackup.instance.dadTransform.DOScale( new Vector3(0.300000012f, 0.243279949f, 0.397630423f), 0);
        });
    }
    public void Spring3()
    {
        CameraShake.instance.Flash("0.5");
        camstatic.DOFade(1, 0);
        camstatic.DOFade(0, 1);
        cam5.DOFade(0, 0);
        cam2.DOFade(1, 0);
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("springtrap3");
        LeanTween.delayedCall(0.2f, () =>
        {
            Vector3 sc = cam2.transform.localScale;
            DadBackup.instance.dadTransform.DOScale(new Vector3(0.300000012f, 0.243279949f, 0.397630423f), 0);

        });
    }
    public void EndSpringCamera()
    {
        StopRed();
        Camstatic("fadein", "0.2");
        cam2.DOFade(0, 0);
        ChangeWhiteFade("inBlack", "2.1");
        AddCameraZoom("1.2");
    }
    public void SpringHere()
    {
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("springtrap4");
        MainEventSystem.instance.ChangeCharacterPlayer("bfnonever");
        Song.instance.deadPlayer.clip = deathBf2;
        Song.instance.deadPlayer.Prepare();
        scene1.SetActive(false);
        scene2.SetActive(true);
        ChangeWhiteFade("outBlack", "3.2");
    }
    public void SpringHere2()
    {
        ChangeWhiteFade("outBlack", "0.1");
        CameraShake.instance.Flash("1.01");
        Tablet();
        AddCameraZoom("1.5");
    }
    public void GoofyMoment()
    {
        AddCameraZoom("1.5");
        MainEventSystem.instance.ChangeScrollSpeed("0.725");
        ChangeWhiteFade("inBlack", "1.2");
    }
    public void EndGoofyMoment()
    {
        AddCameraZoom("1.5");
        Red();
        CameraShake.instance.Flash("1.01");
        CameraShake.instance.StartShake(0.2f, 0.2f);
        VideoPlayedInstance.instance.clip = clipFight;
        VideoPlayedInstance.instance.player.clip = clipFight;
        VideoPlayedInstance.instance.player.Prepare();

    }
    public void StartRapSpring()
    {
        ChangeWhiteFade("inBlack", "0.5");
        VideoPlayedInstance.instance.raw2.SetActive(true);
        VideoPlayedInstance.instance.player.Play();
        MainEventSystem.instance.ChangeScrollSpeed("2.32");
        NoteCameraController.instance.OnCameraOpen();
    }
    public void Run()
    {
        VideoPlayedInstance.instance.raw2.SetActive(false);
        VideoPlayedInstance.instance.player.Stop();
        NoteCameraController.instance.OnDisabledCamera();
        scene2.SetActive(false);
        sceneRun.SetActive(true);
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("springtrap5");
        MainEventSystem.instance.ChangeCharacterPlayer("bfrun");
        MainEventSystem.instance.ChangeScrollSpeed("3.01");
        vid.Prepare();
    }
    public void VidOnGame()
    {
        CameraShake.instance.FlashBlack("0.5");
        vid.Play();
        sceneRun.SetActive(false);
        scene3.SetActive(true);
        CameraMovement.instance.GetComponent<CRTCameraBehaviour>().enabled = true;
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("springtrap6");
        MainEventSystem.instance.ChangeCharacterPlayer("bfnonever3");
        MainEventSystem.instance.ChangeScrollSpeed("2.9");
        StopRed();
    }
    public void Spring7()
    {
        scene3.SetActive(false);
        scene4.SetActive(true);
        Tablet();
        CameraMovement.instance.GetComponent<CRTCameraBehaviour>().enabled = false;

        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("springtrap7");
        MainEventSystem.instance.ChangeCharacterPlayer("bfback");
        MainEventSystem.instance.ChangeScrollSpeed("3.01");
        CameraShake.instance.Flash("1.01");
    }
    public void FireStage()
    {
        scene4.SetActive(false);
        scene5.SetActive(true);
        vid2.Play();
        CameraShake.instance.Flash("1.01");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("springtrap8");
        MainEventSystem.instance.ChangeCharacterPlayer("bfbackfire");
        MainEventSystem.instance.ChangeScrollSpeed("2.5");
        MainEventSystem.instance.InGameAnimation("bfLight");
        MainEventSystem.instance.fireMaterial.SetActive(true);
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
