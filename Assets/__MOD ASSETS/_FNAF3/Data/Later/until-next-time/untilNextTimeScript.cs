using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using SimpleSpriteAnimator;
using UnityEngine.Rendering.PostProcessing;


#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif

public class untilNextTimeScript : MonoBehaviour
{
    public GameObject blackAndWhite, scene1, scene2, scene3;

    [Space]
    public SpriteRenderer helloAgain;
    public SpriteRenderer gf;
    public SpriteAnimation[] animation2;

    [Space]
    public TMP_FontAsset fontAsset;
    public VideoClip unt, until;
    public PostProcessProfile a, brainBloom;

    [Header("Flashes")]
    public SpriteRenderer blackFlash;
    public SpriteRenderer whiteFlash;

    private bool isSongStarted = false;
    private bool showName = false;

    private void Start()
    {
        blackAndWhite.SetActive(true);

        scene1.SetActive(true);
        scene2.SetActive(false);
        scene3.SetActive(false);

        whiteFlash.DOFade(0, 0);
        blackFlash.DOFade(1, 0);
        helloAgain.DOFade(0, 0);

        MainEventSystem.instance.subtitleText.font = fontAsset;

        CameraMovement.instance.enableMovement = false;
        CameraMovement.instance.volume.profile = brainBloom;
        CameraMovement.instance.enabled = false;
        Song.instance.mainCamera.transform.position = new Vector3(-1.87f, 1.86000001f, -13.1999998f);
        Song.instance.defaultGameZoom = 1.5f;
        
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
            Song.instance.defaultGameZoom = Mathf.Lerp(Song.instance.defaultGameZoom, 5, Time.deltaTime / 27f);
            if (DadBG.instance)
            {
                DadBackup.instance.dad.enabled = false;
                DadBG.instance.dad.enabled = true;
            }
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

    public void HelloAgainChange(string fade, string time)
    {
        float s = float.Parse(time, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
        if(fade == "fadein")
            helloAgain.DOFade(1, s);
        else
            helloAgain.DOFade(0, s);
    }
    public void GameStart()
    {
        isSongStarted = true;
        GamePlayCamera.instance.cam.enabled = true;
        CameraMovement.instance.enableMovement = true;
        CameraMovement.instance.enabled = true;
        AddCameraZoom("0.5");
        MainEventSystem.instance.PlayCutSceneEnemy("Sit");
    }
    public void Shake()
    {
        CameraShake.instance.StartShake(0.2f, 0.1f);
    }

    public void Eyes()
    {
        CameraShake.instance.Flash("1.1");
        scene1.SetActive(false);
        scene2.SetActive(true);
        VideoPlayedInstance.instance.clip = unt;
        VideoPlayedInstance.instance.player.clip = unt;
        VideoPlayedInstance.instance.player.Prepare();
    }
    public void PlayVid()
    {
        CameraMovement.instance.volume.profile = null;
        GamePlayCamera.instance.digital.enabled = false;
        GamePlayCamera.instance.analog.enabled = false;
        VideoPlayedInstance.instance.player.Play();
        VideoPlayedInstance.instance.raw.SetActive(true);
    }
    public void NewPerson()
    {
        VideoPlayedInstance.instance.player.Stop();
        VideoPlayedInstance.instance.raw.SetActive(false);
        ChangeWhiteFade("in", "0.01");
        ChangeWhiteFade("out", "2.01");
        gf.enabled = true;
        MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("pshadowfreddyBonnie");
        AddCameraZoom("0.5");
    }
    public void ChangeCharacters2()
    {
        CameraShake.instance.StartShake(0.3f, 1.3f);
        LeanTween.delayedCall(1.3f, () =>
        {
            scene2.SetActive(false);
            scene3.SetActive(true);
            ChangeWhiteFade("out", "2.001");
            MainEventSystem.instance.ChangeCharacterEnemyWithoutFlash("shadowfreddy2");
            MainEventSystem.instance.ChangeCharacterPlayer("bfNothingPhantoms");
            BoyfriendBackup.instance.bf.enabled = false;
            for (int i = 0; i < gf.gameObject.GetComponent<SpriteAnimator>().spriteAnimations.Count; i++)
            {
                gf.gameObject.GetComponent<SpriteAnimator>().spriteAnimations.RemoveAt(i);
            }
            //gf.gameObject.GetComponent<SpriteAnimator>().spriteAnimations.RemoveAt(animation2.Length);
            foreach (SpriteAnimation a in animation2)
            {
                gf.gameObject.GetComponent<SpriteAnimator>().spriteAnimations.Add(a);
            }
            CameraMovement.instance.volume.profile = a;
            GamePlayCamera.instance.digital.enabled = true;
            GamePlayCamera.instance.analog.enabled = true;
            //rgb shader lol
        });
    }
    public void Purple1()
    {
        CameraShake.instance.StartShake(0.2f, 10.4f);
        CameraShake.instance.purple.CrossFadeAlpha(1, 0, false);
        CameraShake.instance.purple.CrossFadeAlpha(0, 1, false);
        AddCameraZoom("0.7");
    }
    public void Purple2()
    {
        CameraShake.instance.purple.CrossFadeAlpha(1, 0, false);
        CameraShake.instance.purple.CrossFadeAlpha(0, 1, false);
        AddCameraZoom("0.7");
        VideoPlayedInstance.instance.clip = until;
        VideoPlayedInstance.instance.player.clip = until;
        VideoPlayedInstance.instance.player.Prepare();
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
