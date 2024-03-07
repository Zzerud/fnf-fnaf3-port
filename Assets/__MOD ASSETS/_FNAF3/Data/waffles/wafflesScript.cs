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
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif

public class wafflesScript : MonoBehaviour
{
    public GameObject blackAndWhite, scene1Dad, scene1Bf, scene2, scene3;
    public GameObject dadBear, dadBonnie;
    public SpriteRenderer bear2;
    public GameObject bonnie2;
    public SpriteRenderer waffles;

    public TMP_FontAsset fontAsset;

    [Header("Flashes")]
    public SpriteRenderer blackFlash;
    public SpriteRenderer whiteFlash;


    private void Start()
    {
        blackAndWhite.SetActive(true);

        scene1Dad.SetActive(true);
        scene1Bf.SetActive(false);
        scene2.SetActive(false);
        scene3.SetActive(false);

        dadBear.SetActive(false);
        dadBonnie.SetActive(true);

        whiteFlash.DOFade(0, 0);
        blackFlash.DOFade(1, 0);

        MainEventSystem.instance.subtitleText.font = fontAsset;
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
    public void StartGame()
    {
        ChangeWhiteFade("outBlack", "0.001");
        ChangeWhiteFade("out", "0.001");
        CameraShake.instance.Flash("1.1");
        AddCameraZoom("0.5");
        MainEventSystem.instance.SongCredit("waffles");
        BoyfriendBackup.instance.bf.enabled = false;
        ToggleSubtitles();
    }


    public void CharBearDad()
    {
        scene1Bf.SetActive(false);
        scene1Dad.SetActive(true);

        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("wafflebear");
        DadBackup.instance.dad.enabled = true;
        BoyfriendBackup.instance.bf.enabled = false;

        dadBear.SetActive(false);
        dadBonnie.SetActive(true);
    }
    public void CharBonnieDad()
    {
        scene1Bf.SetActive(false);
        scene1Dad.SetActive(true);

        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("wafflebonnie");
        DadBackup.instance.dad.enabled = true;
        BoyfriendBackup.instance.bf.enabled = false;

        dadBear.SetActive(true);
        dadBonnie.SetActive(false);
    }
    public void CharBf()
    {
        scene1Bf.SetActive(true);
        scene1Dad.SetActive(false);

        DadBackup.instance.dad.enabled = false;
        BoyfriendBackup.instance.bf.enabled = true;

        dadBear.SetActive(false);
        dadBonnie.SetActive(true);
    }
    public void Scene2()
    {
        scene1Bf.SetActive(false);
        scene1Dad.SetActive(false);
        scene3.SetActive(false);
        scene2.SetActive(true);

        CameraShake.instance.Flash("1.1");
        AddCameraZoom("0.6");

        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("wafflebear2");
        MainEventSystem.instance.ChangeCharacterPlayer("waffletrap2");
        DadBackup.instance.dad.enabled = true;
        BoyfriendBackup.instance.bf.enabled = true;
    }
    public void Scene2Bear()
    {
        bear2.enabled = false;
        bonnie2.SetActive(true);
        CameraShake.instance.FlashBlack("0.6");
        DadBackup.instance.dad.sortingOrder = 7;
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("wafflebear2");
    }
    public void Scene2Bonnie()
    {
        bear2.enabled = true;
        bonnie2.SetActive(false);
        DadBackup.instance.dad.sortingOrder = 2;
        CameraShake.instance.FlashBlack("0.6");
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("wafflebonnie2");
    }
    public void Scene3()
    {
        scene3.SetActive(true);
        scene2.SetActive(false);

        ToggleSubtitles();
        ChangeWhiteFade("outBlack", "0.001");
        CameraShake.instance.Flash("1.1");
        AddCameraZoom("0.6");
        MainEventSystem.instance.ChangeCharacterPlayer("waffletrap3");
        DadBackup.instance.dad.enabled = false;
    }
    public void WaffleGlow(string timeGlow)
    {
        float s = float.Parse(timeGlow, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
        CameraShake.instance.waffleGlow.CrossFadeAlpha(1, 0, false);
        CameraShake.instance.waffleGlow.CrossFadeAlpha(0, s, false);
    }
    public void Scene2FromScene3()
    {
        ToggleSubtitles(); 
        scene3.SetActive(false);
        scene2.SetActive(true);
        Scene2Bear();
        CameraShake.instance.Flash("1.1");
        ChangeWhiteFade("outBlack", "0.001");
        AddCameraZoom("0.6");

        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("wafflebear2");
        MainEventSystem.instance.ChangeCharacterPlayer("waffletrap2");
        DadBackup.instance.dad.enabled = true;
    }

    public void End()
    {
        waffles.sortingOrder = 653;
        ChangeWhiteFade("inBlack", "10.1");
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
