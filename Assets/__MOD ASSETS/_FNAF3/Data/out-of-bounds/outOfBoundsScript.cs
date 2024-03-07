using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;
using System.Net.Http;
using System;
using UnityEngine.Windows;
using System.Threading.Tasks;
using BrewedInk.CRT;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif

public class outOfBoundsScript : MonoBehaviour
{
    public GameObject blackAndWhite, scene1, scene2, scene3, scene4, scene5, scene6, scene7, scene8, scene9, scene10;
    [Space]
    public GameObject[] obbPuppetChildren;
    public VideoClip endClip;

    [Space]
    public SpriteRenderer outOfBounds;

    [Space]
    public TMP_FontAsset fontAsset;

    [Header("Flashes")]
    public SpriteRenderer blackFlash;
    public SpriteRenderer whiteFlash;

    private bool isSongStarted = false;
    private bool showName = false;
    private int children = 0;

    private void Start()
    {
        blackAndWhite.SetActive(true);

        scene1.SetActive(true);
        scene2.SetActive(false);
        scene3.SetActive(false);
        scene4.SetActive(false);
        scene5.SetActive(false);
        scene6.SetActive(false);
        scene7.SetActive(false);
        scene8.SetActive(false);
        scene9.SetActive(false);
        scene10.SetActive(false);

        whiteFlash.DOFade(0, 0);
        blackFlash.DOFade(1, 0);
        outOfBounds.DOFade(0, 0);

        MainEventSystem.instance.subtitleText.font = fontAsset;

        DadBackup.instance.dad.enabled = false;
        BoyfriendBackup.instance.bf.enabled = false;
        CameraMovement.instance.enableMovement = false;
        CameraMovement.instance.enabled = false;
        Song.instance.mainCamera.transform.position = new Vector3(-5.57999992f, 3.55999994f, -13.1999998f);
        Song.instance.defaultGameZoom = 5;
        GamePlayCamera.instance.gameObject.GetComponent<CRTCameraBehaviour>().enabled = true;
        CameraMovement.instance.gameObject.GetComponent<CRTCameraBehaviour>().enabled = true;

        MainEventSystem.instance.statics.SetActive(true);
        MainEventSystem.instance.staticss.CrossFadeAlpha(0, 0, false);
        MainEventSystem.instance.redStatic.CrossFadeAlpha(0, 0, false);
        MainEventSystem.instance.whiteStatic.CrossFadeAlpha(0, 0, false);
    }
    private void Update()
    {
        if (!isSongStarted && Song.instance.songStarted)
        {
            GamePlayCamera.instance.cam.enabled = false;
            CameraMovement.instance.enableMovement = false;
            Song.instance.defaultGameZoom = Mathf.Lerp(Song.instance.defaultGameZoom, 2, Time.deltaTime / 4f);
        }
    }
    public void ShowAndCloseOutOfBounds()
    {
        if (!showName)
        {
            outOfBounds.DOFade(1, 1);
            showName = true;
        }
        else
        {
            outOfBounds.DOFade(0, 2);
            showName = false;
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
    public void ChangeStatic(string fadeType, string time)
    {
        float s = float.Parse(time, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);

        if (fadeType == "fadein")
            MainEventSystem.instance.staticss.CrossFadeAlpha(1, s, false);
        else
            MainEventSystem.instance.staticss.CrossFadeAlpha(0, s, false);
    }
    public void StartGameLol()
    {
        ChangeWhiteFade("out", "1.001");
        isSongStarted = true;
        GamePlayCamera.instance.cam.enabled = true;
        CameraMovement.instance.enableMovement = true;
        CameraMovement.instance.enabled = true;
        GamePlayCamera.instance.gameObject.GetComponent<CRTCameraBehaviour>().enabled = true;
        CameraMovement.instance.gameObject.GetComponent<CRTCameraBehaviour>().enabled = false;
        scene1.SetActive(false);
        scene2.SetActive(true);
        DadBackup.instance.dad.enabled = true;
        BoyfriendBackup.instance.bf.enabled = true;
        AddCameraZoom("0.5");
    }
    public void EvilBB()
    {
        scene2.SetActive(false);
        scene3.SetActive(true);
        MainEventSystem.instance.ChangeScrollSpeed("2.86");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("oobbb2");
        MainEventSystem.instance.ChangeCharacterPlayer("oobsbonnie2");
        ChangeStatic("fadeout", "7.1");
        ChangeWhiteFade("in", "0.001");
        ChangeWhiteFade("out", "5.001");
        AddCameraZoom("0.5");
    }
    public void OobMangle()
    {
        scene3.SetActive(false);
        scene4.SetActive(true);
        ChangeWhiteFade("in", "0.001");
        ChangeWhiteFade("out", "5.001");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("oobmangle");
        MainEventSystem.instance.ChangeCharacterPlayer("oobsbonnie");
        AddCameraZoom("0.5");
        ChangeStatic("fadeout", "0.001");
    }
    public void EvilMangle()
    {
        scene4.SetActive(false);
        scene5.SetActive(true);
        ChangeWhiteFade("out", "5.1");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("oobmangle6");
        MainEventSystem.instance.ChangeCharacterPlayer("oobsbonnie2");
        AddCameraZoom("0.5");
    }
    public void OobChica()
    {
        scene5.SetActive(false);
        scene6.SetActive(true);
        ChangeWhiteFade("in", "0.001");
        ChangeStatic("fadeout", "0.001");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("oobchica");
        MainEventSystem.instance.ChangeCharacterPlayer("oobsbonnie");
        ChangeWhiteFade("out", "5.1");
        AddCameraZoom("0.5");
    }
    public void EvilChica()
    {
        scene6.SetActive(false);
        scene7.SetActive(true);
        ChangeWhiteFade("in", "0.001");
        ChangeStatic("fadeout", "0.1");
        ChangeWhiteFade("out", "5.001");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("oobchica2");
        MainEventSystem.instance.ChangeCharacterPlayer("oobsbonnie2");
        AddCameraZoom("0.5");
    }
    public void Shake()
    {
        CameraShake.instance.StartShake(0.3f, 0.3f);
        AddCameraZoom("0.8");
    }
    public void Oobfredbear()
    {
        scene7.SetActive(false);
        scene8.SetActive(true);
        ChangeWhiteFade("inBlack", "0.001");
        ChangeStatic("fadeout", "1.001");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("oobfredbear");
        MainEventSystem.instance.ChangeCharacterPlayer("oobsbonnie");
        AddCameraZoom("0.5");
    }
    public void EvilFredbear()
    {
        scene8.SetActive(false);
        scene9.SetActive(true);
        ChangeWhiteFade("in", "0.01");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("oobfredbear2");
        MainEventSystem.instance.ChangeCharacterPlayer("oobsbonnie2");
        ChangeWhiteFade("out", "5.1");
        ToggleSubtitles();
        AddCameraZoom("0.7");
        ChangeStatic("fadeout", "1.1");
    }
    public void Obbpuppet()
    {
        scene9.SetActive(false);
        scene10.SetActive(true);
        MainEventSystem.instance.ChangeScrollSpeed("2.6");
        ChangeStatic("fadein", "0.001");
        ChangeWhiteFade("in", "0.01");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("oobNothing");
        MainEventSystem.instance.ChangeCharacterPlayer("charlie");
        ChangeStatic("fadeout", "10.01");
        ChangeWhiteFade("out", "5.001");
    }
    public void AddChildren()
    {
        CameraShake.instance.Flash("1.1");
        obbPuppetChildren[children].SetActive(true);
        children++;
    }
    public void PrepareForEnd()
    {
        VideoPlayedInstance.instance.clip = endClip;
        VideoPlayedInstance.instance.player.clip = endClip;
        VideoPlayedInstance.instance.player.Prepare();
        ChangeWhiteFade("out", "5.1");
    }
    public void VideoPlay()
    {
        VideoPlayedInstance.instance.raw.SetActive(true);
        VideoPlayedInstance.instance.player.Play();
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

        public Subtitle(string textEng, string textRus, string color)
        {
            this.textEng = textEng;
            this.textRus = textRus;
            this.color = color;
        }
    }

    [Space(20)]
    [Header("Subtitles")]
    public Subtitle[] sub;
    private int indexText = 0;

#if UNITY_EDITOR
    [SerializeField] private TextAsset _textAsset;

    [ContextMenu("Read File")]
    public void ReadFile()
    {
        EditorCoroutineUtility.StartCoroutineOwnerless(ReadFileAsync());
    }

    IEnumerator ReadFileAsync()
    {
        yield return null;
        string text = _textAsset.text;
        List<Subtitle> subs = new();
        const string subText = "\"ToggleSubtitles\",\n";
        int currentPos = 0;
        while (currentPos < text.Length)
        {
            int index = text.IndexOf(subText, currentPos);
            if (index == -1)
                break;
            currentPos = index + subText.Length;

            int indexOfStartLine = text.IndexOf("\"", currentPos) + 1;
            int indexOfEndLine = text.IndexOf("\",", indexOfStartLine);
            string textEng = text[indexOfStartLine..indexOfEndLine];

            int indexOfStartColor = text.IndexOf("\"", indexOfEndLine + 2) + 1;
            int indexOfEndColor = text.IndexOf("\"", indexOfStartColor);
            string colorStr = text[indexOfStartColor..indexOfEndColor];

            string textRu = "";
            if (textEng.Length > 0)
            {
                // Set the language from/to in the url (or pass it into this function)
                string url = String.Format
                ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                 "en", "ru", Uri.EscapeUriString(textEng));
                HttpClient httpClient = new HttpClient();
                Task<string> task = httpClient.GetStringAsync(url);
                yield return task;
                string result = task.Result;
                int startIndex = result.IndexOf("\"") + 1;
                int endIndex = result.IndexOf("\"", startIndex);
                textRu =  result[startIndex..endIndex];
            }
            Subtitle s;
            if (colorStr.Length > 0)
            {
                s = new(textEng, textRu, "#" + colorStr);
            }
            else
            {
                s = new(textEng, textRu, colorStr);
            }
            subs.Add(s);
        }
        sub = subs.ToArray();
    }
#endif

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
