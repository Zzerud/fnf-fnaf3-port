using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;

public class FearForeverScript : MonoBehaviour
{
    public GameObject guide, blackAndWhite, scene1, scene2, foxy1, foxy2, foxy3, chica, bb1, bb2, mangle, puppet, phantoms;
    [Space(15)]
    public SpriteRenderer chicaLayout;
    public SpriteRenderer statics;
    public GameObject dad;
    public PostProcessProfile a;
    public VideoClip clipBb, clipPuppet, clipPhantomes;
    public GameObject bbObj;

    [Header("Flashes")]
    public SpriteRenderer blackFlash;
    public SpriteRenderer whiteFlash;

    [Header("Movement Persons")]
    public float duration;
    public Vector3 targetPosition;
    public Transform l;
    private bool isStart = true;

    private void Start()
    {
        blackAndWhite.SetActive(true);

        CameraMovement.instance.volume.profile = a;

        scene1.SetActive(true);
        scene2.SetActive(false);
        foxy1.SetActive(false);
        foxy2.SetActive(false);
        foxy3.SetActive(false);
        chica.SetActive(false);
        bb1.SetActive(false);
        bb2.SetActive(false);
        mangle.SetActive(false);
        puppet.SetActive(false);
        phantoms.SetActive(false);

        whiteFlash.DOFade(0, 0);
        blackFlash.DOFade(1, 0);
    }
    private void Update()
    {
        dad.transform.position = DadBackup.instance.dadTransform.position;
        float s = DadBackup.instance.dadTransform.transform.localScale.x;
        dad.transform.DOScale(s, 0);
    }
    IEnumerator MoveToTarget()
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            DadBackup.instance.dadTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            l.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }
    }
    public void ChangeWhiteFade(string fadeType, string time)
    {
        if (isStart)
        {
            guide.SetActive(false);
            StartCoroutine(MoveToTarget());
            isStart = false;
        }
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
    public void Mangle()
    {
        MainEventSystem.instance.Mangle();
    }
    public void Tablet()
    {
        MainEventSystem.instance.OnTablet();
    }
    public void TableEvent()
    {
        MainEventSystem.instance.ChangeCharacterPlayer("bfscared");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("phfreddy2");
        MainEventSystem.instance.OnTablet();
        scene1.SetActive(false);
        scene2.SetActive(true);
        AddCameraZoom("0.6");

    }
    public void toSceneFoxy1()
    {
        scene2.SetActive(false);
        foxy1.SetActive(true);
        MainEventSystem.instance.ChangeCharacterPlayer("bfscared2");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("phfoxy");
        MainEventSystem.instance.ChangeScrollSpeed("2.4");
    }
    public void toSceneFoxy2()
    {
        foxy1.SetActive(false);
        foxy2.SetActive(true);
        MainEventSystem.instance.ChangeCharacterPlayer("bfscared");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("phfoxy2");
        CameraShake.instance.Flash("0.98");
        MainEventSystem.instance.Mangle();
        MainEventSystem.instance.ChangeScrollSpeed("2.6");
    }
    public void toSceneFoxy3()
    {
        foxy2.SetActive(false);
        foxy3.SetActive(true);
        MainEventSystem.instance.ChangeCharacterPlayer("bfrun");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("phfoxy3");
        CameraShake.instance.Flash("0.98");
        ChangeWhiteFade("outBlack", "0.01");
        MainEventSystem.instance.OnTablet();
        MainEventSystem.instance.ChangeScrollSpeed("2.7");

    }
    public void HiChica()
    {
        foxy3.SetActive(false);
        chica.SetActive(true);
        MainEventSystem.instance.ChangeCharacterPlayer("bfscared");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("phchica");
        statics.DOFade(0, 6);
        ChangeWhiteFade("outBlack", "3.1");
    }
    public void Puppet()
    {
        MainEventSystem.instance.Puppet();
    }
    public void OnChicaHere()
    {
        statics.DOFade(1, 0);
        CameraShake.instance.Flash("0.98");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("phchica2");
        chicaLayout.sortingOrder = 0;

    }
    public void ChicaScare()
    {
        MainEventSystem.instance.JumpScare("chicajump");
        Tablet();
        VideoPlayedInstance.instance.clip = clipBb;
        VideoPlayedInstance.instance.player.clip = clipBb;
        VideoPlayedInstance.instance.player.Prepare();
    }
    public void ByeChica()
    {
        MainEventSystem.instance.JumpScare("chicajumpFull");
        ChangeWhiteFade("inBlack", "0.001");
        MainEventSystem.instance.ChangeScrollSpeed("2.6");
    }
    public void ShowVid()
    {
        VideoPlayedInstance.instance.player.Play();
        VideoPlayedInstance.instance.raw.SetActive(true);
    }
    public void ShowBb()
    {
        VideoPlayedInstance.instance.player.Stop();
        VideoPlayedInstance.instance.raw.SetActive(false);
        chica.SetActive(false);
        bb1.SetActive(true);
        MainEventSystem.instance.ChangeCharacterPlayer("none");
        BoyfriendBackup.instance.bf.enabled = false;
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("phbb");
    }
    public void Bb2()
    {
        CameraShake.instance.Flash("1.1");
        bb1.SetActive(false);
        bb2.SetActive(true);
        BoyfriendBackup.instance.bf.enabled = true;
        MainEventSystem.instance.ChangeCharacterPlayer("bfsit");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("phbb2");
    }
    public void JumpScareBB()
    {
        MainEventSystem.instance.PlayCutSceneEnemy("bbScare");
        MainEventSystem.instance.JumpScare("bbScare");
        ChangeWhiteFade("inBlack", "1.2");
        Tablet();
    }
    public void MangleStage()
    {
        bb2.SetActive(false);
        mangle.SetActive(true);
        MainEventSystem.instance.mangle.SetTrigger("out");
        MainEventSystem.instance.mangleSounds.Stop();
        MainEventSystem.instance.PlayCutSceneEnemy("bbHere");
        MainEventSystem.instance.ChangeCharacterPlayer("bfmangle");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("mangle");
        MainEventSystem.instance.ChangeScrollSpeed("2.7");
        DadBackup.instance.dad.enabled = false;
    }
    
    public void bbWithMangle()
    {
        bbObj.SetActive(true);
        CameraShake.instance.FlashBlack("1.6");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("phbb3");
        MainEventSystem.instance.ChangeScrollSpeed("2.8");
        VideoPlayedInstance.instance.clip = clipPuppet;
        VideoPlayedInstance.instance.player.clip = clipPuppet;
        VideoPlayedInstance.instance.player.Prepare();
    }
    public void MangleScream()
    {
        MainEventSystem.instance.JumpScare("manglegg");
    }
    public void ShowPuppet()
    {
        ChangeWhiteFade("outBlack", "2.1");
        VideoPlayedInstance.instance.player.Stop();
        VideoPlayedInstance.instance.raw.SetActive(false);
        mangle.SetActive(false);
        bbObj.SetActive(false);
        puppet.SetActive(true);
        MainEventSystem.instance.ChangeCharacterPlayer("none");
        BoyfriendBackup.instance.bf.enabled = false;
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("puppet");
        MainEventSystem.instance.ChangeScrollSpeed("2.6");

        VideoPlayedInstance.instance.clip = clipPhantomes;
        VideoPlayedInstance.instance.player.clip = clipPhantomes;
        VideoPlayedInstance.instance.player.Prepare();

    }
    public void ShowPhantoms()
    {
        VideoPlayedInstance.instance.player.Stop();
        VideoPlayedInstance.instance.raw.SetActive(false);
        puppet.SetActive(false);
        phantoms.SetActive(true);
        BoyfriendBackup.instance.bf.enabled = false;
        MainEventSystem.instance.ChangeCharacterPlayer("bffront");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("phantoms");
        MainEventSystem.instance.ChangeScrollSpeed("2.8");


    }

    public void BfTurn()
    {
        BoyfriendBackup.instance.bf.enabled = true;
        DadBackup.instance.dad.enabled = false;
        CameraShake.instance.FlashBlack("0.3");
    }
    public void DadTurn()
    {
        BoyfriendBackup.instance.bf.enabled = false;
        DadBackup.instance.dad.enabled = true;
        CameraShake.instance.FlashBlack("0.3");
    }
    public void TurnOffCam()
    {
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
