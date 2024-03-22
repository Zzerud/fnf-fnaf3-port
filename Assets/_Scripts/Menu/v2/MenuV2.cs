using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FridayNightFunkin.Json;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class MenuV2 : MonoBehaviour
{
    public GameObject debug;
    public GameObject weekContinue;
    public TMP_Text weekContinueText;
    public Week weekName;

    public TMP_Text txt;

    public RectTransform mainScreen;

    public RectTransform playScreen;
    public RectTransform optionsScreen;
    public RectTransform menuButtonsRect;
    public Image inputBlocker;

    [Header("Audio")]
    public AudioSource musicSource;
    //public AudioClip menuClip;
    private Stopwatch _menuStopwatch;
    public Animator heckerAnimator;
    private int _beatCounter;

    [Header("Background")] public Camera backgroundCamera;

    public SpriteRenderer backgroundSprite;

    public UIGradient backgroundGradient;

    [Header("Song List")] public RectTransform songListRect;

    public GameObject bundleButtonPrefab;

    public GameObject songButtonPrefab;

    public Sprite defaultCoverSprite;

    public bool canChangeSongs = true;
    public GameObject migrateBundlesButton;

    private Dictionary<BundleButtonV2, List<SongButtonV2>> bundles =
        new Dictionary<BundleButtonV2, List<SongButtonV2>>();

    [Header("Mode of Play")] public GameObject playModeScreen;
    public GameObject autoPlayButton;

    [Header("Song Info")] public Image songCoverImage;
    public TMP_Text songNameText;
    public TMP_Text highScoreText;
    private int _lastScore = 0;
    [FormerlySerializedAs("songCharterText")] public TMP_Text songCreditsText;
    public TMP_Text songDescriptionText;
    public TMP_Dropdown songDifficultiesDropdown;
    public TMP_Dropdown songModeDropdown;
    public GameObject selectSongScreen;
    public GameObject songInfoScreen;
    public GameObject loadingSongScreen;

    [Header("Notifications")] public GameObject notificationObject;
    public RectTransform notificationLists;

    [Space] public Button[] menuButtons;
    public GameObject[] menuScreens;
    
    private SongMetaV2 _currentMeta;
    private string _songsFolder;
    
    public static MenuV2 Instance;
    public static int lastSelectedBundle;
    public static int lastSelectedSong;

    public static StartPhase startPhase;
    


    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR
        Debug.unityLogger.logEnabled = false;
#endif
        Time.timeScale = 1;
        try
        {
            
            LoadingTransition.instance.Hide();

        }
        catch
        {
            Debug.LogError("Doesn't run");
        }
        Time.timeScale = 1;
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        InitializeMenu();
    }

    public enum StartPhase
    {
        Nothing,
        SongList,
        Offset
    }


    public void UpdateScoreText()
    {
        
        
        string highScoreSave = _currentMeta.songName + _currentMeta.bundleMeta.bundleName +
                               songDifficultiesDropdown.options[songDifficultiesDropdown.value].text.ToLower() +
                               (songModeDropdown.value + 1);
        int highScore = PlayerPrefs.GetInt(highScoreSave, 0);
        print("High Score for " + highScoreSave + " is " + highScore);
        if (songModeDropdown.value != 3)
        {
            LeanTween.value(_lastScore, highScore, .35f).setOnUpdate(value =>
            {
                highScoreText.text = $"High Score: <color=white>{(int)value}</color>";
            }).setOnComplete(() =>
            {
                _lastScore = highScore;
            });
        }
        else
        {
            highScoreText.text = "High Score not available for AutoPlay.";
        }

        
    }

    public int GetBundleIndex(BundleButtonV2 item)
    {

        for (int i = 0; i < bundles.Keys.Count; i++)
        {
            if (bundles.Keys.ElementAt(i) == item)
            {
                return i;
            }
        }

        return 0;
    }
    
    public void ChangeSong(SongMetaV2 meta)
    {
        print("Checking if we can change songs. It is " + canChangeSongs);
        if (!canChangeSongs) return;
        print("Updating info");
        songNameText.text = meta.songName;
        songDescriptionText.text = "<color=yellow>Description:</color> " + meta.songDescription;
        songCoverImage.sprite = meta.songCover;

        songCreditsText.text = string.Empty;
        
        foreach (string role in meta.credits.Keys.ToList())
        {
            string memberName = meta.credits[role];

            songCreditsText.text += $"<color=yellow>{role}:</color> {memberName}\n";
        }
        
        songDifficultiesDropdown.ClearOptions();

        songDifficultiesDropdown.AddOptions(meta.difficulties.Keys.ToList());
        
        loadingSongScreen.SetActive(true);

        selectSongScreen.SetActive(false);
        songInfoScreen.SetActive(false);

        LeanTween.value(musicSource.gameObject, musicSource.volume, 0, 1f).setOnComplete(() =>
        {
            StartCoroutine(nameof(LoadSongAudio), meta.songPath+"/Inst.ogg");
        }).setOnUpdate(value =>
        {
            musicSource.volume = value;
        });
        

        _currentMeta = meta;
    }

    public void PlaySong()
    {
        var difficultiesList = _currentMeta.difficulties.Keys.ToList();
        Song.difficulty = difficultiesList[songDifficultiesDropdown.value];
        Song.modeOfPlay = songModeDropdown.value + 1;
        Song.currentSongMeta = _currentMeta;

        LoadingTransition.instance.Show(() => SceneManager.LoadScene("Game_Backup3"));
    }
    

    IEnumerator LoadSongAudio(string path)
    {
        WWW www = new WWW(path);
        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            canChangeSongs = false;
            musicSource.clip = www.GetAudioClip();
            while (musicSource.clip.loadState != AudioDataLoadState.Loaded)
                yield return new WaitForSeconds(0.1f);
            musicSource.Play();
            LeanTween.value(musicSource.gameObject, musicSource.volume, OptionsV2.instVolume, 1f).setOnUpdate(value =>
            {
                musicSource.volume = value;
            });
            canChangeSongs = true;
            loadingSongScreen.SetActive(false);
            songInfoScreen.SetActive(true);
            UpdateScoreText();
        }
    }


    //FREEPLAY
    public WeekSong currentSong;
    public bool isWeek;
    public GameObject enemyObj;
    public void LaunchSongScreen(WeekSong song)
    {
        isWeek = false;
        playModeScreen.SetActive(true);
        autoPlayButton.SetActive(OptionsV2.AutoPlay);
        currentSong = song;


        Song.weekMode = false;
        Song.currentSong = song;

        
        GetData();
    }
    public void LaunchSongScreen(Week week)
    {
        weekName = week;
        isWeek = true;
        if (PlayerPrefs.HasKey(week.weekName) && PlayerPrefs.GetInt(week.weekName) > 0)
        {
            Song.weekMode = true;
            Song.currentWeek = week;
            Song.currentSong = week.songs[PlayerPrefs.GetInt(week.weekName)];
            Song.currentWeekIndex = PlayerPrefs.GetInt(week.weekName);

            weekContinue.SetActive(true);
            weekContinueText.text = Application.systemLanguage == SystemLanguage.Russian ? $"Продолжить неделю на песне '{weekName.songs[Song.currentWeekIndex].songName}'?":
                    $"Continue the week with song '{weekName.songs[Song.currentWeekIndex].songName}'?";
        }
        else
        {
            playModeScreen.SetActive(true);
            autoPlayButton.SetActive(OptionsV2.AutoPlay);
            PlayerPrefs.SetInt(week.weekName, 0);
            Song.weekMode = true;
            Song.currentWeek = week;
            Song.currentSong = week.songs[0];
            Song.currentWeekIndex = 0;
        }

        GetData();
    }

    public void ContinueWeek(int continueWeek)
    {
        if (continueWeek == 0)
        {
            playModeScreen.SetActive(true);
            autoPlayButton.SetActive(OptionsV2.AutoPlay);
            PlayerPrefs.SetInt(weekName.weekName, 0);
            Song.weekMode = true;
            Song.currentWeek = weekName;
            Song.currentSong = weekName.songs[0];
            Song.currentWeekIndex = 0;
        }
        else
        {
            BeginSong(PlayerPrefs.GetInt($"{weekName.weekName} Opponent"));
        }
    }

    public void BeginSong(int modeOfPlay)
    {
        StartCoroutine(SongBegin(modeOfPlay));
    }

    private IEnumerator SongBegin(int modeOfPlay)
    {
        Song.modeOfPlay = modeOfPlay;



        if (Song.weekMode)
        {
            PlayerPrefs.SetInt($"{weekName.weekName} Opponent", modeOfPlay);


            LoadingTransition.instance.Show(() =>
            {
                
            });

            VideoPlayerScene.nextScene = "Game_Backup3";
            VideoPlayerScene.videoToPlay = Application.systemLanguage == SystemLanguage.Russian ? Song.currentWeek.songs[Song.currentWeekIndex].videoRus : Song.currentWeek.songs[Song.currentWeekIndex].videoEng;
            yield return new WaitForSeconds(0.5f);
            
            SceneManager.LoadScene("Video", LoadSceneMode.Single);

        }
        else
        {
            LoadingTransition.instance.Show(() =>
            {
                
            });
            yield return new WaitForSeconds(0.5f);
            
            SceneManager.LoadScene("Game_Backup3");
        }
    }

    public GameObject storyMenu, freePlay, credits;
    bool isStoryMenu = false;
    bool isFreePlayMenu = false;
    bool isCredits = false;
    public void OnClickStory()
    {
        isStoryMenu = !isStoryMenu;
        storyMenu.SetActive(isStoryMenu);

    }
    public void OnClickFreePlay()
    {
        isFreePlayMenu = !isFreePlayMenu;
        freePlay.SetActive(isFreePlayMenu);
    }
    public void OnClickCreditMenu()
    {
        isCredits = !isCredits;
        credits.SetActive(isCredits);
    }

    public void InitializeMenu()
    {
        if (Instance==null) Instance = this;
        musicSource.volume = OptionsV2.menuVolume;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void GetData()
    {
        try
        {
            if (!isWeek)
                txt.text = Application.systemLanguage == SystemLanguage.Russian ? $"Лучший счёт за главного героя - {PlayerPrefs.GetFloat("statsBf" + currentSong.songIndex)}, аккуратность - {PlayerPrefs.GetFloat("accuracyBf" + currentSong.songIndex):0.00}%\r\n" +
                    $"Лучший счёт за оппонента - {PlayerPrefs.GetFloat("statsDad" + currentSong.songIndex)}, аккуратность - {PlayerPrefs.GetFloat("accuracyDad" + currentSong.songIndex):0.00}%\r\n":
                    $"Best score for a main character - {PlayerPrefs.GetFloat("statsBf" + currentSong.songIndex)}, accuracy - {PlayerPrefs.GetFloat("accuracyBf" + currentSong.songIndex):0.00}%\r\n" +
                    $"Best score for opponent - {PlayerPrefs.GetFloat("statsDad" + currentSong.songIndex)}, accuracy - {PlayerPrefs.GetFloat("accuracyDad" + currentSong.songIndex):0.00}%\r\n";
            else
            {
                float totalAccuracyBf = 0;
                float totalAccuracyDad = 0;


                if (PlayerPrefs.GetFloat("statsBf" + 0) > 0 && PlayerPrefs.GetFloat("statsBf" + 1) > 0 && PlayerPrefs.GetFloat("statsBf" + 2) > 0)
                    totalAccuracyBf = (PlayerPrefs.GetFloat("statsBf" + 0) + PlayerPrefs.GetFloat("statsBf" + 1) + PlayerPrefs.GetFloat("statsBf" + 2) / 3);

                if (PlayerPrefs.GetFloat("statsDad" + 0) > 0 && PlayerPrefs.GetFloat("statsDad" + 1) > 0 && PlayerPrefs.GetFloat("statsDad" + 2) > 0)
                    totalAccuracyDad = (PlayerPrefs.GetFloat("statsDad" + 0) + PlayerPrefs.GetFloat("statsDad" + 1) + PlayerPrefs.GetFloat("statsDad" + 2) / 3);


                txt.text = Application.systemLanguage == SystemLanguage.Russian ? $"Средняя аккуратность недели за главного героя - {totalAccuracyBf:0.00}%\r\n" +
                    $"Средняя аккуратность недели за оппонента - {totalAccuracyDad:0.00}%\r\n":
                    $"Average accuracy of the week per main character - {totalAccuracyBf:0.00}%\r\n" +
                    $"Average accuracy of the week for the opponent - {totalAccuracyDad:0.00}%\r\n";
            }
        }
        catch(Exception e) 
        {

        }
       
        
    }

    public void OptionsScreenTransition(bool toOptions)
    {
        if (toOptions)
        {
           // DiscordController.instance.SetMenuState("Editing Options");
            mainScreen.gameObject.SetActive(false);
            optionsScreen.gameObject.SetActive(true);
            menuButtonsRect.gameObject.SetActive(false);
        }
        else
        {
          //  DiscordController.instance.SetMenuState("Idle");
            mainScreen.gameObject.SetActive(true);
            optionsScreen.gameObject.SetActive(false);
            menuButtonsRect.gameObject.SetActive(true);
            
            foreach(Button btn in menuButtons)
            {
                btn.interactable = true;
            }
        }
    }
    
    public void OpenPlayScreenFromMenu()
    {
      //  TransitionScreen(mainScreen, playScreen, () => DiscordController.instance.SetMenuState("Selecting a Song"));
        canChangeSongs = true;
    }

    public void OpenMenuFromPlayScreen()
    {
        if (!canChangeSongs) return;
     //   TransitionScreen(playScreen, mainScreen, () => DiscordController.instance.SetMenuState("Idle"));
        /*if (musicSource.clip != menuClip)
        {
            musicSource.Stop();

            musicSource.volume = OptionsV2.menuVolume;
            //s.play("menu");
            musicSource.Play();
        }
        */
    }

    public void ChangeSelectedButton(Button newSelectedButton)
    {
        foreach(Button btn in menuButtons)
        {
            btn.interactable = true;
        }

        newSelectedButton.interactable = false;
    }

    public void ChangeSelectedScreen(GameObject newScreen)
    {
        foreach(GameObject menuScreen in menuScreens)
        {
            menuScreen.SetActive(false);
        }

        newScreen.SetActive(true);
    }

    public void OpenURL(string link)
    {
        Application.OpenURL(link);
    }

    public void TransitionScreen(RectTransform oldScreen, RectTransform newScreen, Action onComplete = null)
    {
        inputBlocker.enabled = true;
        oldScreen.LeanMoveY(-720,1f).setEaseOutExpo().setOnComplete(() =>
        {
            oldScreen.gameObject.SetActive(false);
            newScreen.gameObject.SetActive(true);
            newScreen.LeanMoveY(-720, 0);
            newScreen.LeanMoveY(0, 1f).setEaseOutExpo().setOnComplete(() =>
            {
                inputBlocker.enabled = false;
                onComplete?.Invoke();
            });

        });
    }
    
    

    public void DisplayNotification(Color color, string text)
    {
        GameObject notification = Instantiate(notificationObject, notificationLists);
        NotificationObject notificationScript = notification.GetComponent<NotificationObject>();

        notificationScript.notificationText.text = text;
        notificationScript.BackgroundColor = color;

    }
    
    // Update is called once per frame
    void Update()
    {
        debug.SetActive(OptionsV2.DebugMenu);
    }
}
