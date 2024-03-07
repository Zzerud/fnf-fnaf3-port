using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;

public class lastHourScript : MonoBehaviour
{
    public GameObject blackAndWhite, scene1, scene2;
    public GameObject fred;

    public float duration = 5;
    public Vector3 targetPosition, targetPosition2, targetPosition3;
    public float durationFred = 8;
    public Vector3 targetPositionFred, targetPosition2Fred;


    [Header("Flashes")]
    public SpriteRenderer blackFlash;
    public SpriteRenderer whiteFlash;


    private void Start()
    {
        blackAndWhite.SetActive(true);

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


    public void SpringOut()
    {
        StartCoroutine(MoveToTarget());
    }
    public void SpringIn()
    {
        CameraShake.instance.StartShake(0.4f, 0.2f);
        AddCameraZoom("0.8");
        duration = .2f;
        targetPosition = targetPosition2;
        StartCoroutine(MoveToTarget());
    }
    public void SpringInOffice()
    {
        AddCameraZoom("0.8");
        duration = 3;
        targetPosition = targetPosition3;
        StartCoroutine(MoveToTarget());
    }
    public void ShowFred()
    {
        ToggleSubtitles();
        StartCoroutine(MoveToTargetFred());
    }
    public void OutFred()
    {
        targetPositionFred = targetPosition2Fred;
        StartCoroutine(MoveToTargetFred());
    }

    public void HiFred()
    {
        ToggleSubtitles();
        AddCameraZoom("0.8");
        ChangeWhiteFade("inBlack", "0.4");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("5amspringtrapfreddy");
    }

    public void Wow()
    {
        StopAllCoroutines();
        scene1.SetActive(false);
        scene2.SetActive(true);
        ToggleSubtitles();
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("5amspringtrap2");
        MainEventSystem.instance.ChangeCharacterPlayer("securityguy2");
        ChangeWhiteFade("outBlack", "0.5");
        AddCameraZoom("0.5");
    }

    IEnumerator MoveToTarget()
    {
        Vector3 startPosition = DadBackup.instance.dadTransform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            DadBackup.instance.dadTransform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }
    }
    IEnumerator MoveToTargetFred()
    {
        Vector3 startPositionFred = fred.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < durationFred)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / durationFred;

            fred.transform.localPosition = Vector3.Lerp(startPositionFred, targetPositionFred, t);

            yield return null;
        }
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
