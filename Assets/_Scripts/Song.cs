using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using DG.Tweening;
using FridayNightFunkin;
using Newtonsoft.Json;
using QFSW.MOP2;
using SimpleSpriteAnimator;
using Slowsharp;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Song : MonoBehaviour
{
    [HideInInspector] public bool isActiveShake;
    public VideoClip clipRetry;
    public VideoPlayer deadPlayer;
    public GameObject rawObj;
    public Image pauseImage;

    public Image bfOverlay;
    public bool isAdWatched = false;

    public AudioClip[] quote;
    public GameObject blackNotes;
    private int loadCheckpointCount = 0;
    public bool isLoadedEvents = false;


    public bool isTwoParametres = false;
    public bool isGoodAccuracy = true;
    //public bool isCameraFromJSON = true;
    public GameObject restartButton;
    public GameObject restartTextPc,restartTextAndr;
    public bool restart = false;
    bool focusedGame = true;
    bool songEnded = false;
    bool songstrat = false;
    public bool isMenu = false;
    public GameObject mobileHit, mobileBut;
    bool ready = false;

    #region Variables

    public AudioSource soundSource;
    //public SourceAudio soundSourceAndr;
    public AudioClip startSound;
    [Space] public AudioSource[] musicSources;
    //[Space] public SourceAudio[] musicSourceAndr;
    public AudioSource vocalSource;
    //public SourceAudio vocalSourceAndr;
    public AudioSource oopsSource;
   // public SourceAudio oopsSourceAndr;
    public AudioClip musicClip;
    public AudioClip vocalClip;

    public string vocal;
    public string music;

    public AudioClip menuClip;
    public AudioClip[] noteMissClip;
    public bool hasVoiceLoaded;    
    public HybInstance modInstance;


    [Space] public bool songSetupDone;
        
    [Space] public GameObject[] defaultSceneObjects;

    [Space] public GameObject ratingObject;
    public GameObject liteRatingObjectP1;
    public GameObject liteRatingObjectP2;
    public Sprite[] rusSprites;
    public Sprite[] engSprites;
    public Sprite sickSprite;
    public Sprite goodSprite;
    public Sprite badSprite;
    public Sprite shitSprite;
    [Header("Player 1 Stats")]
    public GameObject playerOneScoringObject;
    public GameObject playerOneScoringCorner;
    public TMP_Text playerOneScoringText;
    public Image playerOneCornerImage;
    public TMP_Text playerOneComboText;
    public float playerOneComboTimer;
    public float playerOneScoreLerpSpeed;
    [Header("Player 2 Stats")]
    public GameObject playerTwoScoringObject;
    public GameObject playerTwoScoringCorner;
    public TMP_Text playerTwoScoringText;
    public Image playerTwoCornerImage;
    public TMP_Text playerTwoComboText;
    public float playerTwoComboTimer;
    public float playerTwoScoreLerpSpeed;
    [Space]
    public float ratingLayerTimer;
    private float _ratingLayerDefaultTime = 2.2f;
    private int _currentRatingLayer;
    public PlayerStat playerOneStats;
    public PlayerStat playerTwoStats;

    public Stopwatch stopwatch;
    public long stopwatchWithCheckPoint = 0;
    public bool withCheckPoint = false;

    public Stopwatch beatStopwatch;
    [Space] public Camera mainCamera;
    public Camera uiCamera;
    public float cameraBopLerpSpeed;
    public float portraitBopLerpSpeed;
    private float _defaultZoom;
    public float defaultGameZoom;
    

    [Space, TextArea(2, 12)] public string jsonDir;
    public float notesOffset;
    public float noteDelay;
    [Range(-1f, 1f)] public float speedDifference;

    [Space] public Canvas battleCanvas;
    public Canvas menuCanvas;
    public GameObject generatingSongMsg;
    public GameObject songListScreen;
    
    [Space] public GameObject menuScreen;

    [Header("Death Mechanic")] public Camera deadCamera;
    public GameObject deadBoyfriend;
    public Animator deadBoyfriendAnimator;
    public AudioClip deadNoise;
    public AudioClip deadTheme;
    public AudioClip deadConfirm;
    public Image deathBlackout;
    public bool isDead;
    public bool respawning;
    private bool _quitting = false;
    

   
    
    private float _currentInterval;
    
    [Space] public Transform player1Notes;
    public Color[] player1NoteColors = new Color[4];
    public SpriteRenderer[] player1NoteSprites;
    public List<List<NoteObject>> player1NotesObjects;
    public NoteAnimator[] player1NotesAnimators;
    public Transform player1Left;
    public Transform player1Down;
    public Transform player1Up;
    public Transform player1Right;
    [Space] public Transform player2Notes;
    public Color[] player2NoteColors = new Color[4];
    public SpriteRenderer[] player2NoteSprites;
    public List<List<NoteObject>> player2NotesObjects;
    public NoteAnimator[] player2NotesAnimators;
    public Transform player2Left;
    public Transform player2Down;
    public Transform player2Up;
    public Transform player2Right;
    private List<NoteBehaviour> _noteBehaviours = new List<NoteBehaviour>();
    
    [Header("Prefabs")] public GameObject leftArrow;
    public GameObject downArrow;
    public GameObject upArrow;
    public GameObject rightArrow;
    [Space] public GameObject holdNote;
    public Sprite holdNoteEnd;
    public Sprite holdNoteEndOutline;
    public Sprite holdNoteSprite;
    public Sprite holdNoteSpriteOutline;
    [Header("Object Pools")] 
    public ObjectPool leftNotesPool;
    public ObjectPool rightNotesPool;
    public ObjectPool downNotesPool;
    public ObjectPool upNotesPool;
    public ObjectPool holdNotesPool;
    
    [Header("Characters")]
    public string[] characterNames;
    public Character[] characters;
    [Space] public string[] protagonistNames;
    public Protagonist[] protagonists;

    public Dictionary<string, Protagonist> protagonistsDictionary;
    public Dictionary<string, Character> charactersDictionary;
    
    [Space]
    public Character defaultEnemy;
    [Space] public GameObject girlfriendObject;
    public SpriteAnimator girlfriendAnimator;
    public bool altDance;

    [FormerlySerializedAs("enemyObj")] [Header("Enemy")] public GameObject opponentObject;
    public Character enemy;
    public string enemyName;
    [FormerlySerializedAs("enemyAnimator")] public SpriteAnimator opponentAnimator;
    public float enemyIdleTimer = .3f;
    public float enemy2IdleTimer = .25f;
    private float _currentEnemyIdleTimer;
    private float _currentEnemy2IdleTimer;
    public float enemyNoteTimer = .25f;
    private Vector3 _enemyDefaultPos;
    private readonly float[] _currentEnemyNoteTimers = {0, 0, 0, 0};
    private readonly float[] _currentDemoNoteTimers = {0, 0, 0, 0};
    private LTDescr _enemyFloat;


    //
    [FormerlySerializedAs("bfObj")] [Header("Boyfriend")] public GameObject boyfriendObject;
    public SpriteAnimator boyfriendAnimator;
    public float boyfriendIdleTimer = .3f;
    public float boyfriend2IdleTimer = .3f;
    public Sprite boyfriendPortraitNormal;
    public Sprite boyfriendPortraitDead;
    private float _currentBoyfriendIdleTimer;
    private float _currentBoyfriend2IdleTimer;
    public Protagonist protagonist;

    public FNFSong _song;

    public static Song instance;

    [Header("Scenes")] public Dictionary<string, SceneData> scenes;

    [Header("Health")] public float health = 100;

    private const float MAXHealth = 200;
    public float healthLerpSpeed;
    public GameObject healthBar;
    public RectTransform boyfriendHealthIconRect;
    public Image boyfriendHealthIcon;
    public Image boyfriendHealthBar;
    public RectTransform enemyHealthIconRect;
    public Image enemyHealthIcon;
    public Image enemyHealthBar;

    [Space] public GameObject songDurationObject;
    public TMP_Text songDurationText;
    public Image songDurationBar;

    [Space] public GameObject startSongTooltip;
    
    [Space] public NoteObject lastNote;
    public float stepCrochet;
    public float beatsPerSecond;
    public int currentBeat;
    public bool beat;

    private float _bfRandomDanceTimer;
    private float _enemyRandomDanceTimer;

    private bool _portraitsZooming;
    private bool _cameraZooming;

    public string songsFolder;
    public string selectedSongDir;

    public static SongMetaV2 currentSongMeta;
    public static string difficulty;
    public static int modeOfPlay;

    [HideInInspector] public SongListObject selectedSong;


    public bool songStarted;

    [Header("Subtitles")]

    public SubtitleDisplayer subtitleDisplayer;
    public bool usingSubtitles;

    public Week k;
    public static bool weekMode;
    public static Week currentWeek;
    public static int currentWeekIndex;
    public static WeekSong currentSong;
    #endregion

    private void Start()
    {

        if (currentSong.isCustomIcons)
            enemyHealthIcon.sprite = enemy.portrait;

        
        if (Application.systemLanguage == SystemLanguage.Russian)
        {
            sickSprite = rusSprites[0];
            goodSprite = rusSprites[1];
            badSprite = rusSprites[2];
            shitSprite = rusSprites[3];
        }
        else
        {
            sickSprite = engSprites[0];
            goodSprite = engSprites[1];
            badSprite = engSprites[2];
            shitSprite = engSprites[3];
        }

        Time.timeScale = 1;
        /*
         * To allow other scripts to access the Song script without needing the
         * script to be found or referenced, we set a static variable within the
         * Song script itself that can be used at anytime to access the singular used Song
         * script instance.
         */
        if (instance == null) 
            instance = this;

        isActiveShake = OptionsV2.CameraShake;

        
            
        k = currentWeek;
        /*
         * Sets the "songs folder" to %localappdata%/Rei/FridayNight/Songs.
         * This is used to find and load any found songs.
         *
         * This can only be changed within the editor itself and not in-game,
         * though it would not be hard to make that possible.
         */
        songsFolder = Application.persistentDataPath + "/Songs";

        /*
         * Makes sure the UI for the song gameplay is disabled upon load.
         *
         * This disables the notes for both players and the UI for the gameplay.
         */
        player1Notes.gameObject.SetActive(false);
        player2Notes.gameObject.SetActive(false);
        playerOneScoringText.enabled = false;
        playerTwoScoringText.enabled = false;
        playerOneCornerImage.enabled = false;
        playerTwoCornerImage.enabled = false;
        battleCanvas.enabled = true;
        healthBar.SetActive(false);
        songDurationObject.SetActive(false);

        mainCamera = Camera.main;
        
        
        /*
        * Initialize LeanTween
        */
        
       LeanTween.init(9999);
        
        /*
         * Grabs the subtitle displayer.
         */
        subtitleDisplayer = GetComponent<SubtitleDisplayer>();

        if (OptionsV2.DesperateMode)
        {
            boyfriendObject.SetActive(false);
            opponentObject.SetActive(false);

            boyfriendHealthIcon.enabled = false;
            enemyHealthIcon.enabled = false;
        }

        if (OptionsV2.LiteMode)
        {
            girlfriendObject.SetActive(false);
        }

        
        /*
         * In case we want to reset the enemy position later on,
         * we will save their current position.
         */
        _enemyDefaultPos = opponentObject.transform.position;

        /*
         * We'll make a dictionary of characters via the two arrays of character names
         * and character classes.
         *
         * This is later on used to load a character based on their name for "Player2"
         * in an FNF chart.
         */
        charactersDictionary = new Dictionary<string, Character>();
        for (int i = 0; i < characters.Length; i++)
        {
            charactersDictionary.Add(characterNames[i], characters[i]);
        }

        protagonistsDictionary = new Dictionary<string, Protagonist>();
        for (int i = 0; i < protagonists.Length; i++)
        {

            protagonistsDictionary.Add(protagonistNames[i], protagonists[i]);

        }


        _defaultZoom = uiCamera.orthographicSize;

        bool doAuto = false;
        
        switch (modeOfPlay)
        {
            //Boyfriend
            case 1:
                Player.playAsEnemy = false;
                Player.twoPlayers = false;
                break;
            //Opponent
            case 2:
                Player.playAsEnemy = true;
                Player.twoPlayers = false;
                break;
            //Local Multiplayer
            case 3:
                Player.playAsEnemy = false;
                Player.twoPlayers = true;
                break;
            //Auto
            case 4:
                doAuto = true;
                Player.playAsEnemy = false;
                Player.twoPlayers = false;
                break;
        }
        
        PlaySong(doAuto);

        stopwatchWithCheckPoint = SpawnPointManager.timeLastSpawnPointMilliseconds;

        if (stopwatchWithCheckPoint > 0)
        {
            withCheckPoint = true;
        }
    }

    #region Song Gameplay

    public void PlaySong(bool auto)
    {
        /*
         * If the player wants the song to play itself,
         * we'll set the Player script to be on demo mode.
         */
        Player.demoMode = auto;
        
        /*
         * We'll reset any stats then update the UI based on it.
         */
        _currentRatingLayer = 0;
        playerOneStats = new PlayerStat();
        playerTwoStats = new PlayerStat();
        currentBeat = 0;
        UpdateScoringInfo();

        /*
         * We'll enable the gameplay UI.
         *
         * We'll also hide the Menu UI but also reset it
         * so we can instantly go back to the menu
         */
        battleCanvas.enabled = true;
        generatingSongMsg.SetActive(true);

        menuCanvas.enabled = false;
        songListScreen.SetActive(false);

        /*
         * Now we start the song setup.
         *
         * This is a Coroutine so we can make use
         * of the functions to pause it for certain time.
         */
        SetupSong();

    }

    void SetupSong()
    {
        /*
         * First, we have to load the instrumentals from the
         * local file. We use the, although deprecated, WWW function for this.
         *
         * In case of an error, we just stop and output it.
         * Otherwise, we set the clip as the instrumental.
         *
         * Then we wait until it is fully loaded, WaitForSeconds allows us to pause
         * the coroutine for .1 seconds then check if the clip is loaded again.
         * If not, keep waiting until it is loaded.
         *
         * Once the instrumentals is loaded, we repeat the exact same thing with
         * the voices. Then, we generate the rest of the song from the chart file.
         */
        if (weekMode)
        {
            

                musicClip = currentWeek.songs[currentWeekIndex].instrumentals;
                vocalClip = currentWeek.songs[currentWeekIndex].vocals;
                currentSong = currentWeek.songs[currentWeekIndex];
            //inst = currentWeek.songs[currentWeekIndex].instrumentalKey;
            //voices = currentWeek.songs[currentWeekIndex].vocalKey;

        }
        else
        {
            musicClip = currentSong.instrumentals;
            vocalClip = currentSong.vocals;


            //inst = currentSong.instrumentalKey;
            //voices = currentSong.vocalKey;
        }

        

        GenerateSong();
    }



    //private AudioDataProperty inst;
    //private AudioDataProperty voices;
    
    public void GenerateSong()
    {
        for (int i = 0; i < OptionsV2.instance.colorPickers.Length; i++)
        {
            player1NoteColors[i] = OptionsV2.instance.colorPickers[i].color;
            player2NoteColors[i] = OptionsV2.instance.colorPickers[i].color;
        }

        /*
         * Set the health the half of the max so it's smack dead in the
         * middle.
         */

        health = MAXHealth / 2;

        /*
         * Special thanks to KadeDev for creating the .NET FNF Song parsing library.
         *
         * With it, we can load the song as a whole class full of chart information
         * via the chart file.
         */
        if (weekMode)
        {
            if (PlayerPrefs.HasKey("difficult"))
                _song = PlayerPrefs.GetString("difficult") == "normal" ? new FNFSong(currentWeek.songs[currentWeekIndex].chart.text, FNFSong.DataReadType.AsRawJson) : new FNFSong(currentWeek.songs[currentWeekIndex].chartEasy.text, FNFSong.DataReadType.AsRawJson);
            else
                _song = new FNFSong(currentWeek.songs[currentWeekIndex].chart.text, FNFSong.DataReadType.AsRawJson);
        }
        else
        {
            if (PlayerPrefs.HasKey("difficult"))
                _song = PlayerPrefs.GetString("difficult") == "normal" ? new FNFSong(currentSong.chart.text, FNFSong.DataReadType.AsRawJson) : new FNFSong(currentSong.chartEasy.text, FNFSong.DataReadType.AsRawJson);
            else
                _song = new FNFSong(currentSong.chart.text, FNFSong.DataReadType.AsRawJson);
        }
        /*
         * We grab the BPM to calculate the BPS and the Step Crochet.
         */
        beatsPerSecond = 60 / (float) _song.Bpm;

        stepCrochet = (60 / (float) _song.Bpm * 1000 / 4);

        
        /*
         * Just in case, we'll force player 1 and player 2 notes to be wiped to a
         * clean slate.
         */

        if (player1NotesObjects != null)
        {
            foreach (List<NoteObject> list in player1NotesObjects)
            {

                list.Clear();
            }
            


            player1NotesObjects.Clear();
        }

        if (player2NotesObjects != null)
        {
            foreach (List<NoteObject> list in player2NotesObjects)
            {

                list.Clear();
            }

            player2NotesObjects.Clear();
        }

        leftNotesPool.ReleaseAll();
        downNotesPool.ReleaseAll();
        upNotesPool.ReleaseAll();
        rightNotesPool.ReleaseAll();
        holdNotesPool.ReleaseAll();
        //GENERATE PLAYER ONE NOTES
        player1NotesObjects = new List<List<NoteObject>>
        {
            new List<NoteObject>(), new List<NoteObject>(), new List<NoteObject>(), new List<NoteObject>()
        };

        //GENERATE PLAYER TWO NOTES
        player2NotesObjects = new List<List<NoteObject>>
        {
            new List<NoteObject>(), new List<NoteObject>(), new List<NoteObject>(), new List<NoteObject>()
        };

        /*
         * If somehow we fucked up, we stop the song generation process entirely.
         *
         * If we didn't, we keep going.
         */

        if (_song == null)
        {
            Debug.LogError("Error with song data");
            return;
        }
        
        /*
         * Shift the UI for downscroll or not
         */
        if (OptionsV2.Downscroll)
        {
            healthBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 310f);

            uiCamera.transform.position = new Vector3(0, 7,-10);

            playerOneScoringObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 560);
            playerOneScoringCorner.transform.rotation = Quaternion.Euler(0, 0, -270);
            playerTwoScoringObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 560);
            playerTwoScoringCorner.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            healthBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -314f);

            uiCamera.transform.position = new Vector3(0, 2,-10);

            playerOneScoringObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            playerTwoScoringObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            playerTwoScoringCorner.transform.rotation = Quaternion.Euler(0, 0, 0);
            playerOneScoringCorner.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        /*
         * Shift the UI or not for Middlescroll
         */
        
        if(OptionsV2.Middlescroll)
        {
            if (!Player.twoPlayers)
            {
                if (Player.playAsEnemy)
                {
                    foreach (SpriteRenderer sprite in player1NoteSprites)
                    {
                        sprite.enabled = false;
                    }
                    
                    
                    foreach (SpriteRenderer sprite in player2NoteSprites)
                    {
                        sprite.enabled = true;
                    }

                    player2Notes.transform.position = new Vector3(0f, 4.45f, 15);
                }
                else
                {
                    foreach (SpriteRenderer sprite in player1NoteSprites)
                    {
                        sprite.enabled = true;
                    }
                    foreach (SpriteRenderer sprite in player2NoteSprites)
                    {
                        sprite.enabled = false;
                    }

                    player1Notes.transform.position = new Vector3(0f, 4.45f, 15);
                }
            }
        }
        else
        {
            foreach (SpriteRenderer sprite in player2NoteSprites)
            {
                sprite.enabled = true;
            }
            
            foreach (SpriteRenderer sprite in player1NoteSprites)
            {
                sprite.enabled = true;
            }
            
            player2Notes.transform.position = new Vector3(-3.6f, 4.45f, 15);
            player1Notes.transform.position = new Vector3(3.6f, 4.45f, 15);

        }


        if(_song.SongName == "Everything Nice" || _song.SongName == "Lock In")
        {
            foreach (SpriteRenderer sprite in player2NoteSprites)
            {
                sprite.enabled = false;
            }
            blackNotes.gameObject.SetActive(true);
            if (!OptionsV2.Middlescroll) blackNotes.transform.localPosition = new Vector3(0.629999995f, -0.4833f, -18.7747383f);
            else blackNotes.transform.localPosition = new Vector3(-2.81999993f, -0.4833f, -18.7747383f);
        }
        /*
         * "foreach" allows us to go through each and every single section in the
         * chart. Then a nested "foreach" allows to go through every notes in that
         * specific section.
         */
        foreach ( FNFSong.FNFSection section in _song.Sections ) {
            foreach ( var noteData in section.Notes ) {
                _noteBehaviours.Add( new NoteBehaviour( section, noteData ) );
            }
        }
        /*
         * Charts tend to not have organized notes, so we have to sort notes
         * for the game so inputs do not get screwed up.
         *
         * The notes for each player are sorted in ascending order based on strum time.
         */

        for (int i = 0; i < 4; i++)
        {
            player1NotesObjects[i] = player1NotesObjects[i].OrderBy(s => s.strumTime).ToList();
            player2NotesObjects[i] = player2NotesObjects[i].OrderBy(s => s.strumTime).ToList();
        }

        /*foreach (List<NoteObject> nte in player1NotesObjects)
        {
            foreach (NoteObject nte2 in nte)
            {
                print(nte2.transform.position);
            }
        }*/

        /*
         * We now enable the notes UI for both players and disable the
         * generating song message UI.
         */
        player1Notes.gameObject.SetActive(true);
        player2Notes.gameObject.SetActive(true);

        generatingSongMsg.SetActive(false);

        healthBar.SetActive(true);
        if(Player.demoMode || Player.twoPlayers || !Player.playAsEnemy)
        {
            playerOneScoringText.enabled = true;
            playerOneCornerImage.enabled = true;
        }
        if(Player.demoMode || Player.twoPlayers || Player.playAsEnemy)
        {
            playerTwoScoringText.enabled = true;
            playerTwoCornerImage.enabled = true;
        }
        /*
         * Tells the entire script and other attached scripts that the song
         * started to play but has not fully started.
         */
        songSetupDone = true;
        songStarted = false;

       

        /*
         * Reset the stopwatch entirely.
         */
        stopwatch = new Stopwatch();

        /*
         * Stops any current music playing and sets it to not loop.
         */
        musicSources[0].loop = false;
        musicSources[0].volume = OptionsV2.instVolume;
        musicSources[0].Stop();

        vocalSource.loop = false;
        vocalSource.volume = OptionsV2.instVolume;
        vocalSource.Stop();
        /*
         * Initialize combo texts.
         */
        playerOneComboText.alpha = 0;
       playerTwoComboText.alpha = 0;

        /*
         * Disable the entire Menu UI and enable the entire Gameplay UI.
         */
        menuCanvas.enabled = false;
        battleCanvas.enabled = true;
        
        if (!OptionsV2.SongDuration)
            songDurationObject.SetActive(false);
        else
        {
            songDurationObject.SetActive(true);
            
            if (OptionsV2.Downscroll)
            {
                RectTransform rect = songDurationObject.GetComponent<RectTransform>();
                
                rect.anchoredPosition = new Vector3(0,-338,0);
                if (OptionsV2.Middlescroll) songDurationObject.SetActive(false);
            }
        }
        
        
        /*
         * If the player 2 in the chart exists in this engine,
         * we'll change player 2 to the correct character.
         *
         * If not, keep any existing character we selected.
         */
        if(!OptionsV2.DesperateMode)
        {
            if (charactersDictionary.ContainsKey(_song.Player2))
            {
                enemy = charactersDictionary[_song.Player2];
                opponentAnimator.spriteAnimations = enemy.animations;

                /*
                 * Yes, opponents can float if enabled in their
                 * configuration file.
                 */
                if (enemy.doesFloat)
                {
                    _enemyFloat = LeanTween.moveLocalY(opponentObject, enemy.floatToOffset, enemy.floatSpeed)
                        .setEaseInOutExpo()
                        .setLoopPingPong();
                }
                else
                {
                    /*
                     * In case any previous enemy floated before and this new one does not,
                     * we reset their position and cancel the floating tween.
                     */
                    if (_enemyFloat != null && LeanTween.isTweening(_enemyFloat.id))
                    {
                        LeanTween.cancel(_enemyFloat.id);
                        opponentObject.transform.position = _enemyDefaultPos;
                    }
                }

                enemyHealthIcon.sprite = enemy.portrait;
                enemyHealthIconRect.sizeDelta = new Vector2(180,180);
                enemyHealthBar.color = enemy.healthColor;
                playerTwoCornerImage.color = enemy.healthColor;

               
                    Vector3 offset = enemy.cameraOffset;
                    //offset.z = -10;
                    enemy.cameraOffset = offset;
                    CameraMovement.instance.playerTwoOffset = offset;
                    CameraMovement.instance._defaultPositionPlayer2 = offset;
                //CameraMovement.instance.zPosEnemy = offset.z;
                CameraMovement.instance.orthographicSizePlayerTwo = enemy.orthographicSize;
                    CameraMovement.instance.yPosPlayer = offset.y;
                
                

                EnemyPlayAnimation("Idle");
                if (GFSpawn.instance)
                {
                    if(GFSpawn.instance.wowGfSing)
                        GFSpawn.instance.gfAnim.SetTrigger("Idle");
                }

                //opponentAnimator.transform.localScale = new Vector2(enemy.scale, enemy.scale);

                //CameraMovement.instance.playerTwoOffset = enemy.cameraOffset;
            }




            if (protagonistsDictionary.ContainsKey(_song.Player1))
            {
                protagonist = protagonistsDictionary[_song.Player1];
                boyfriendAnimator.spriteAnimations = protagonist.animations;

                boyfriendHealthIcon.sprite = protagonist.portrait;
                boyfriendHealthIconRect.sizeDelta = new Vector2(180,180);
                boyfriendHealthBar.color = protagonist.healthColor;
                playerOneCornerImage.color = protagonist.healthColor;

                
                    Vector3 offset = protagonist.cameraOffset;
                    //offset.z = -10;
                    protagonist.cameraOffset = offset;
                    CameraMovement.instance.playerOneOffset = offset;
                    CameraMovement.instance._defaultPositionPlayer1 = offset;
                    CameraMovement.instance.yPosPlayer = offset.y;
                CameraMovement.instance.orthographicSizePlayerOne = protagonist.orthographicSize;



                if (protagonist.isVideoDeath)
                {
                    deadPlayer.clip = protagonist.videoDeath;
                    deadPlayer.Prepare();
                }
                else
                    deadBoyfriendAnimator.runtimeAnimatorController = protagonist.deathAnimator.runtimeAnimatorController;

                BoyfriendPlayAnimation("Idle");
                if (Enemy1Animator.instance)
                {
                    BoyfriendPlayAnimationChar2("Idle");

                }

                //boyfriendAnimator.transform.localScale = new Vector2(protagonist.scale, protagonist.scale);

                //CameraMovement.instance.playerOneOffset = protagonist.cameraOffset;
            }
            
            
        }
        if (!ready)
        {
            if (weekMode)
            {
                SceneManager.LoadScene(currentWeek.songs[currentWeekIndex].sceneName,
                LoadSceneMode.Additive);

            }
            else
            {
                SceneManager.LoadScene(currentSong.sceneName,
                LoadSceneMode.Additive);
            }
        }
        


        if (isDead)
        {
            isDead = false;
            respawning = false;

            deadCamera.enabled = false;

            
        }
        if(OptionsV2.SongDuration)
        {

                float time = musicClip.length - SpawnPointManager.timeLastSpawnPoint;

                int seconds = (int)(time % 60); // return the remainder of the seconds divide by 60 as an int
                time /= 60; // divide current time y 60 to get minutes
                int minutes = (int)(time % 60); //return the remainder of the minutes divide by 60 as an int

                songDurationText.text = minutes + ":" + seconds.ToString("00");
            songDurationBar.fillAmount = SpawnPointManager.timeLastSpawnPoint / musicClip.length;

            songDurationBar.color = currentSong.songColor;
        }

        mainCamera.enabled = true;
        uiCamera.enabled = true;
        /*
         * Now we can fully start the song in a coroutine.
         */
        pauseImage.sprite = currentSong.imageInPause;
        StartCoroutine(nameof(SongStart), startSound.length);
    }

    public void ChangeColorDuration(Color color)
    {
        songDurationBar.color = color;
    }
    public void OnClickToStart()
    {
        
            ready = true;
        
    }

    public bool isGameStartGf = false;
    IEnumerator SongStart(float delay)
    {
        /*
         * If we are in demo mode, delete any temp charts.
         */
        if (Player.demoMode)
        {
            if(File.Exists(Application.persistentDataPath + "/tmp/ok.json"))
                File.Delete(Application.persistentDataPath + "/tmp/ok.json");
            if(Directory.Exists(Application.persistentDataPath + "/tmp"))
                Directory.Delete(Application.persistentDataPath + "/tmp");
        }
        
        startSongTooltip.SetActive(true);

        startSongTooltip.GetComponentInChildren<TMP_Text>().text =
            Application.systemLanguage == SystemLanguage.Russian ?
            "Нажми сюда, чтобы начать игру." :
            "Click here to start the game.";



        if (!ready)
            LoadingTransition.instance.Hide();
        else
            ready = false;
        
        //DiscordController.instance.EnableGameStateLoop = true;

        yield return new WaitUntil(() => ready);

        if (!Player.demoMode)
        {

            if (PlayerPrefs.GetInt("Toggle") == 1)
            {
                mobileBut.SetActive(true);
                mobileHit.SetActive(false);
            }
            else
            {
                mobileHit.SetActive(true);
                mobileBut.SetActive(false);
            }

        }
        startSongTooltip.SetActive(false);
        /*
        * Start the countdown audio.
        *
        * Unlike FNF, this does not dynamically change based on BPM.
        */
        soundSource.clip = startSound;
        soundSource.Play();
        isGameStartGf = true;
        /*
         * Wait for the countdown to finish.
         */
        yield return new WaitForSeconds(delay);

        if(!OptionsV2.LiteMode)
            mainCamera.orthographicSize = 4;
        
        /*
         * Start the beat stopwatch.
         *
         * This is used to precisely calculate when a beat happens based
         * on the BPM or BPS.
         */
        beatStopwatch = new Stopwatch();
        beatStopwatch.Start();


        /*
         * Sets the voices and music audio sources clips to what
         * they should have.
         */
        musicSources[0].clip = musicClip;


        vocalSource.clip = vocalClip;
        float crochet = ((60f / (float)_song.Bpm) * 1000f);
        CurStepManager.Instance.stepCrochet = (crochet / 4f);
        CurStepManager.Instance.NewSong();
        //CurStepManager.instance.songLength = musicSources[0].clip.length;
        //CurStepManager.instance.stepCrochet = 14.5f/CurStepManager.instance.bpm;
        //CurStepManager.instance.clip.clip = musicClip;
        songStarted = true;
        /*
         * In case we have more than one audio source,
         * let's tell them all to play.
         */
        
        foreach (AudioSource source in musicSources)
        {
            source.Play();
            source.time = SpawnPointManager.timeLastSpawnPoint;
        }
        vocalSource.Play();
        vocalSource.time = SpawnPointManager.timeLastSpawnPoint;
        //musicSourceAndr[0].Play(inst.Key);
        //musicSourceAndr[1].Play(voices.Key);


        /*
         * Plays the vocal audio source then tells this script and other
         * attached scripts that the song fully started.
         */
        //vocalSourceAndr.Play(voices.Key);
        StartCoroutine(CoutDownToDoneMusic(musicClip.length - SpawnPointManager.timeLastSpawnPoint));

        

        modInstance?.Invoke("OnSongStarted");

        /*
         * Start subtitles.
         */
        if(usingSubtitles)
        {
            subtitleDisplayer.paused = false;
            subtitleDisplayer.StartSubtitles();
        }
        /*
         * Restart the stopwatch for the song itself.
         */

        stopwatch.Restart();
    }
    
    
    public void GenNote( FNFSong.FNFSection section, List<decimal> note ) {
        /*
                 * The .NET FNF Chart parsing library already has something specific
                 * to tell us if the note is a must hit.
                 *
                 * But previously I already kind of reverse engineered the FNF chart
                 * parsing process so I used the "ConvertToNote" function in the .NET
                 * library to grab "note data".
                 */
        GameObject newNoteObj;
        List<decimal> data = note;
        /*
         * It sets the "must hit note" boolean depending if the note
         * is in a section focusing on the boyfriend or not, and
         * if the note is for the other section.
         */
        bool mustHitNote = section.MustHitSection;
        if (data[1] > 3)
            mustHitNote = !section.MustHitSection;
        int noteType = Convert.ToInt32(data[1] % 4);
        int noteEvent = Convert.ToInt32(data[3]);
        /*
         * We make a spawn pos variable to later set the spawn
         * point of this note.
         */
        Vector3 spawnPos;

        /*
         * We get the length of this note's hold length.
         */
        float susLength = (float)data[2];

        /*
        if (susLength > 0)
        {
            isSusNote = true;

        }
        */

        /*
         * Then we adjust it to fit the step crochet to get the TRUE
         * hold length.
         */
        susLength /= stepCrochet;

        /*
         * It checks the type of note this is and spawns in a note gameobject
         * tailored for it then sets the spawn point for it depending on if it's
         * a note belonging to player 1 or player 2.
         *
         * If somehow this is the wrong data type, it fails and stops the song generation.
         */
        switch (noteType)
        {
            case 0: //Left
                newNoteObj = leftNotesPool.GetObject();
                spawnPos = mustHitNote ? player1Left.position : player2Left.position;
                newNoteObj.GetComponent<NoteInfo>().target = mustHitNote ? player1Left : player2Left;
                break;
            case 1: //Down
                newNoteObj = downNotesPool.GetObject();
                spawnPos = mustHitNote ? player1Down.position : player2Down.position;
                newNoteObj.GetComponent<NoteInfo>().target = mustHitNote ? player1Down : player2Down;
                break;
            case 2: //Up
                newNoteObj = upNotesPool.GetObject();
                spawnPos = mustHitNote ? player1Up.position : player2Up.position;
                newNoteObj.GetComponent<NoteInfo>().target = mustHitNote ? player1Up : player2Up;
                break;
            case 3: //Right
                newNoteObj = rightNotesPool.GetObject();
                spawnPos = mustHitNote ? player1Right.position : player2Right.position;
                newNoteObj.GetComponent<NoteInfo>().target = mustHitNote ? player1Right : player2Right;
                break;
            default:
                Debug.LogError("Invalid note data.");
                return;
        }

        /*
         * We then move the note to a specific position in the game world.
         */
        spawnPos += Vector3.down *
                    (Convert.ToSingle(data[0] / (decimal)notesOffset) + (_song.Speed * noteDelay));
        spawnPos.y -= (_song.Bpm / 60) * 144 * _song.Speed;
        newNoteObj.transform.position = spawnPos;
        //newNoteObj.transform.position += Vector3.down * Convert.ToSingle(secNoteData[0] / notesOffset);

        /*
         * Each note gameobject has a special component named "NoteObject".
         * It controls the note's movement based on the data provided.
         * It also allows Player 2 to hit their notes.
         *
         * Below we set this note's component data. Simple.
         *
         * DummyNote is always false if generated via a JSON.
         */
        NoteObject nObj = newNoteObj.GetComponent<NoteObject>();

        nObj.ScrollSpeed = -_song.Speed;
        nObj.strumTime = (float)data[0];
        nObj.type = noteType;
        nObj.eventNote = noteEvent;
        nObj.mustHit = mustHitNote;
        nObj.dummyNote = false;
        nObj.layer = section.MustHitSection ? 1 : 2;

        /*
         * We add this new note to a list of either player 1's notes
         * or player 2's notes, depending on who it belongs to.
         */
        if (mustHitNote)
            player1NotesObjects[noteType].Add(nObj);
        else
            player2NotesObjects[noteType].Add(nObj);

        /*
         * This below is for hold notes generation. It tells the future
         * hold note what the previous note is.
         */
        lastNote = nObj;
        /*
         * Now we generate hold notes depending on this note's hold length.
         * The generation of hold notes is more or less the same as normal
         * notes. Hold notes, though, use a different gameobject as it's not
         * a normal note.
         *
         * If there's nothing, we skip.
         */
        nObj.layer = section.MustHitSection ? 1 : 2;

        for (int i = 0; i < Math.Floor(susLength); i++)
        {
            GameObject newSusNoteObj;
            Vector3 susSpawnPos;

            bool setAsLastSus = false;

            /*
             * Math.floor returns the largest integer less than or equal to a given number.
             *
             * I uh... have no clue why this is needed or what it does but we need this
             * in or else it won't do hold notes right so...
             */
            newSusNoteObj = holdNotesPool.GetObject();
            NoteObject noteObject = newSusNoteObj.GetComponent<NoteObject>();
            if ((i + 1) == Math.Floor(susLength))
            {
                noteObject.sprite.sprite = holdNoteEnd;
                noteObject.outlineSprite.sprite = holdNoteEndOutline;
                setAsLastSus = true;
            }
            else
            {
                setAsLastSus = false;
                noteObject.sprite.sprite = holdNoteSprite;
                noteObject.outlineSprite.sprite = holdNoteSpriteOutline;
            }

            switch (noteType)
            {
                case 0: //Left
                    susSpawnPos = mustHitNote ? player1Left.position : player2Left.position;
                    newSusNoteObj.GetComponent<NoteInfo>().target = mustHitNote ? player1Left : player2Left;
                    break;
                case 1: //Down
                    susSpawnPos = mustHitNote ? player1Down.position : player2Down.position;
                    newSusNoteObj.GetComponent<NoteInfo>().target = mustHitNote ? player1Down : player2Down;
                    break;
                case 2: //Up
                    susSpawnPos = mustHitNote ? player1Up.position : player2Up.position;
                    newSusNoteObj.GetComponent<NoteInfo>().target = mustHitNote ? player1Up : player2Up;
                    break;
                case 3: //Right
                    susSpawnPos = mustHitNote ? player1Right.position : player2Right.position;
                    newSusNoteObj.GetComponent<NoteInfo>().target = mustHitNote ? player1Right : player2Right;
                    break;
                default:
                    susSpawnPos = mustHitNote ? player1Left.position : player2Left.position;
                    newSusNoteObj.GetComponent<NoteInfo>().target = mustHitNote ? player1Left : player2Left;
                    break;
            }


            susSpawnPos += Vector3.down *
                           (Convert.ToSingle(data[0] / (decimal)notesOffset) + (_song.Speed * noteDelay));
            susSpawnPos.y -= (_song.Bpm / 60) * startSound.length * _song.Speed;
            newSusNoteObj.transform.position = susSpawnPos;
            NoteObject susObj = noteObject;
            susObj.type = noteType;
            susObj.eventNote = noteEvent;
            susObj.ScrollSpeed = -_song.Speed;
            susObj.mustHit = mustHitNote;
            susObj.strumTime = (float)data[0] + (stepCrochet * i) + stepCrochet;
            susObj.susNote = true;
            susObj.dummyNote = false;
            susObj.lastSusNote = setAsLastSus;
            susObj.layer = section.MustHitSection ? 1 : 2;
            susObj.GenerateHold(lastNote);
            if (mustHitNote)
                player1NotesObjects[noteType].Add(susObj);
            else
                player2NotesObjects[noteType].Add(susObj);
            lastNote = susObj;
        }
    }
    

#region Pause Menu
    public void PauseSong()
    {
        

        subtitleDisplayer.paused = true;
        
        stopwatch.Stop();
        beatStopwatch.Stop();

        foreach (AudioSource source in musicSources)
        {
            source.Pause();
        }
        /*musicSourceAndr[0].Pause();
        musicSourceAndr[1].Pause();*/
        AudioListener.pause = true;

        if (hasVoiceLoaded)
        {
            vocalSource.Pause();
            
        }

        Pause.instance.pauseScreen.SetActive(true);
    }

    public void ContinueSong()
    {
        //AudioAutoPause._instance.isMenuOpen = false;

        stopwatch.Start();
        beatStopwatch.Start();
        isMenu = false;
        Time.timeScale = 1;
        subtitleDisplayer.paused = false;

        try
        {
            if (Song.instance.stopwatch != null)
                Song.instance.stopwatch.Start();
            if (Song.instance.beatStopwatch != null)
                Song.instance.beatStopwatch.Start();
        }
        catch (Exception)
        {


        }

        foreach (AudioSource source in musicSources)
        {
            source.UnPause();
        }
       // musicSourceAndr[0].Focus();
        vocalSource.UnPause();
       // vocalSourceAndr.Focus();
        //AudioAutoPause._instance._isMenu = false;
       // AudioAutoPause._instance._isFocused = true;

        AudioListener.pause = false;

        Pause.instance.pauseScreen.SetActive(false);
    }

    public void RestartSong()
    {
        //SceneManager.UnloadSceneAsync(currentSong.sceneName);
        ContinueSong();

        Time.timeScale = 1;
        subtitleDisplayer.StopSubtitles();
        SpawnPointManager.timeLastSpawnPoint = 0;
        SpawnPointManager.timeLastSpawnPointMilliseconds = 0;
        SpawnPointManager.numberOfCheckpoint = 0;

        
        LoadingTransition.instance.Show(() =>
        {
            Pause.instance.pauseScreen.SetActive(false);
            Pause.instance.pMain.SetActive(true);
            Pause.instance.pDiff.SetActive(false);
            VideoPlayerScene.nextScene = "Game_Backup3";
            SceneManager.LoadScene("Video", LoadSceneMode.Single);
        });
        //SceneManager.UnloadScene(currentSong.sceneName);
        SceneManager.sceneLoaded += SceneLoadede;
        foreach (AudioSource source in musicSources)
        {
            source.Stop();
        }
        
        vocalSource.Stop();
        PlaySong(false);

    }
    private void SceneLoadede(Scene scene, LoadSceneMode mode)
    {
        if(scene == SceneManager.GetSceneByName("Video"))
        {
            

        }

    }

    public void QuitSong()
    {
        Time.timeScale = 1;
        ContinueSong();
        subtitleDisplayer.StopSubtitles();
        SpawnPointManager.timeLastSpawnPoint = 0;
        SpawnPointManager.timeLastSpawnPointMilliseconds = 0;
        SpawnPointManager.numberOfCheckpoint = 0;

        LoadingTransition.instance.Show(() =>
        {
            //uiCamera.GetUniversalAdditionalCameraData().SetRenderer(0);
            //uiCamera.GetUniversalAdditionalCameraData().renderPostProcessing = true;
            //  DiscordController.instance.EnableGameStateLoop = false;
            //AdLoader.instance.ShowInterstitial();
            SceneManager.LoadScene("Title", LoadSceneMode.Single);
            //InterstitialAdShows.RequestInterstitial();
            // DiscordController.instance.EnableGameStateLoop = false;
        });
         foreach (AudioSource source in musicSources)
         {
             source.Stop();
         }

        vocalSource.Stop();
        
    }
    public void ChangeDifficulty(string difficulty)
    {
        // If the player sets the difficulty to "Normal" or "Easy", change the difficulty accordingly and reload the scene. If "Back" or other, then disable/enable the difficulty change screen. 
        //PlayerPrefs.HasKey("difficult")
        switch (difficulty)
        {
            case "normal":
                PlayerPrefs.SetString("difficult", difficulty);
                RestartSong();
                break;
            case "easy":
                PlayerPrefs.SetString("difficult", difficulty);
                RestartSong();
                break;
            case "enter":
                Pause.instance.pMain.SetActive(false);
                Pause.instance.pDiff.SetActive(true);
                break;
            default:
                Pause.instance.pMain.SetActive(true);
                Pause.instance.pDiff.SetActive(false);
                break;
        }
    }

    #endregion
    #endregion

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnClickRestart()
    {
        SpawnPointManager.timeLastSpawnPoint = 0;
        SpawnPointManager.timeLastSpawnPointMilliseconds = 0;
        SpawnPointManager.numberOfCheckpoint = 0;
        restart = true;
    }
    public void OnClickRestartToCheckPoint()
    {

        //RewardedAds.instance.ShowRewardedAd();
        
    }

    #region Animating

    public void EnemyPlayAnimation(string animationName)
    {
        if (enemy.idleOnly || OptionsV2.DesperateMode) return;
        opponentAnimator.Play(animationName);
        _currentEnemyIdleTimer = enemyIdleTimer;
    }
    public void EnemyPlayAnimationChar2(string animationName)
    {
        if (enemy.idleOnly || OptionsV2.DesperateMode) return;
        Enemy2Animator.instance.animator.Play(animationName);
        _currentEnemy2IdleTimer = enemy2IdleTimer;
    }
    public void EnemyPlayAnimationAdditional(string animationName, int character)
    {
        if (enemy.idleOnly || OptionsV2.DesperateMode) return;

        Enemy2Animator.instance.additionalAnimators[character].Play(animationName);
        for (int i = 0; i < Enemy2Animator.instance.additionalAnimators.Length; i++)
        {
            Enemy2Animator.instance._currentEnemyAdditionalIdleTimer[i] = Enemy2Animator.instance.enemyAdditionalIdleTimer[i];
        }
        
    }

    private void BoyfriendPlayAnimation(string animationName)
    {
        if (OptionsV2.DesperateMode) return;
        boyfriendAnimator.Play("BF " + animationName);
        if (MainEventSystem.instance && MainEventSystem.instance.effectsEnabled)
        {
            for (int i = 0; i <= 3; i++)
            {
                if (BoyfriendBackup.instance.effects[i].activeSelf)
                    BoyfriendBackup.instance.effects[i].SetActive(false);
            }
            switch (animationName)
            {
                case "Sing Left":
                    BoyfriendBackup.instance.effects[1].SetActive(true);
                    break;
                case "Sing Down":
                    BoyfriendBackup.instance.effects[0].SetActive(true);
                    break;
                case "Sing Up":
                    BoyfriendBackup.instance.effects[3].SetActive(true);
                    break;
                case "Sing Right":
                    BoyfriendBackup.instance.effects[2].SetActive(true);
                    break;
            }


        }


        _currentBoyfriendIdleTimer = boyfriendIdleTimer;
    }

    private void BoyfriendPlayAnimationChar2(string animationName)
    {
        if (OptionsV2.DesperateMode) return;
        Enemy1Animator.instance.animator.Play("BF " + animationName);

        _currentBoyfriend2IdleTimer = boyfriend2IdleTimer;
    }

    public void AnimateNote(int player, int type, string animName)
    {
        switch (player)
        {
            case 1: //Boyfriend

                player1NotesAnimators[type].Play(animName, type);
                
                /*player1NotesAnimators[type].Play(animName,0,0);
                player1NotesAnimators[type].speed = 0;
                        
                player1NotesAnimators[type].Play(animName);
                player1NotesAnimators[type].speed = 1;

                if (animName == "Activated" & !Player.twoPlayers)
                {
                    if(Player.demoMode)
                        _currentDemoNoteTimers[type] = enemyNoteTimer;
                    else if(Player.playAsEnemy)
                        _currentEnemyNoteTimers[type] = enemyNoteTimer;

                }*/

                break;
            case 2: //Opponent

                player2NotesAnimators[type].Play(animName, type);
                
                /*player2NotesAnimators[type].Play(animName,0,0);
                player2NotesAnimators[type].speed = 0;
                        
                player2NotesAnimators[type].Play(animName);
                player2NotesAnimators[type].speed = 1;

                if (animName == "Activated" & !Player.twoPlayers)
                {
                    if(!Player.playAsEnemy)
                        _currentEnemyNoteTimers[type] = enemyNoteTimer;
                }*/
                break;
        }
    }

#endregion

#region Note & Score Registration

    public enum Rating
    {
        Sick = 1,
        Good = 2,
        Bad = 3,
        Shit = 4
    }

    public float accuracyBf, accuracyDad;

    public void UpdateScoringInfo()
    {
        
        if (!Player.playAsEnemy || Player.twoPlayers || Player.demoMode)
        {
            
            float accuracyPercent;
            if(playerOneStats.totalNoteHits != 0)
            {
                float sickScore = playerOneStats.totalSicks * 4;
                float goodScore = playerOneStats.totalGoods * 3;
                float badScore = playerOneStats.totalBads;
                float shitScore = playerOneStats.totalShits ;

                float totalAccuracyScore = sickScore + goodScore + badScore + shitScore;

                var accuracy = totalAccuracyScore / (playerOneStats.totalNoteHits * 4);
                
                accuracyPercent = (float) Math.Round(accuracy, 4);
                accuracyPercent *= 100;
                
            }
            else
            {
                accuracyPercent = 0;
            }
            accuracyBf = accuracyPercent;
            if(Application.systemLanguage == SystemLanguage.Russian)
                playerOneScoringText.text =
                    $"{playerOneStats.currentScore} :Счёт\n{accuracyPercent:0.00}% :Аккуратность\n{playerOneStats.missedHits} :Ошибок";
            else
                playerOneScoringText.text =
                        $"{playerOneStats.currentScore} :Score\n{accuracyPercent:0.00}% :Acc\n{playerOneStats.missedHits} :Mistakes";

            if (currentSong.isGfEnabled || currentWeekIndex >= 1)
            {
                if (accuracyPercent > 75)
                {
                    isGoodAccuracy = true;
                    GFSpawn.instance.gfAnim.SetTrigger("Good");
                }
                else if(accuracyPercent <= 75 && accuracyPercent > 0)
                {
                    isGoodAccuracy = false;
                    GFSpawn.instance.gfAnim.SetTrigger("Bad");
                }
            }


            //playerOneScoringText.text = playerOneStats.currentScore.ToString("00000000");
        }
        else
        {
            playerOneScoringText.text = string.Empty;
        }

        if (Player.playAsEnemy || Player.twoPlayers || Player.demoMode)
        {
            
            float accuracyPercent;
            if(playerTwoStats.totalNoteHits != 0)
            {
                float sickScore = playerTwoStats.totalSicks * 4;
                float goodScore = playerTwoStats.totalGoods * 3;
                float badScore = playerTwoStats.totalBads * 2;
                float shitScore = playerTwoStats.totalShits;

                float totalAccuracyScore = sickScore + goodScore + badScore + shitScore;

                var accuracy = totalAccuracyScore / (playerTwoStats.totalNoteHits * 4);
                
                accuracyPercent = (float) Math.Round(accuracy, 4);
                accuracyPercent *= 100;
            }
            else
            {
                accuracyPercent = 0;
            }
            accuracyDad = accuracyPercent;
            if(Application.systemLanguage == SystemLanguage.Russian)
                playerTwoScoringText.text =
                    $"Счёт: {playerTwoStats.currentScore}\nАккуратность: {accuracyPercent:0.00}%\nОшибок: {playerTwoStats.missedHits}";
            else
                playerTwoScoringText.text =
                    $"Score: {playerTwoStats.currentScore}\nAcc: {accuracyPercent:0.00}%\nMistakes: {playerTwoStats.missedHits}";
            if (currentSong.isGfEnabled || currentWeekIndex >= 1)
            {
                if (accuracyPercent > 60)
                {
                    GFSpawn.instance.gfAnim.SetTrigger("Bad");
                    isGoodAccuracy = false;
                }
                else if (accuracyPercent <= 60 && accuracyPercent > 0)
                {
                    GFSpawn.instance.gfAnim.SetTrigger("Good");
                    isGoodAccuracy = true;
                }
            }
            //playerTwoScoringText.text = playerTwoStats.currentScore.ToString("00000000");

        }
        else
        {
            playerTwoScoringText.text = string.Empty;
        }
    }
    
    public void NoteHit(NoteObject note)
    {
        if (note == null) return;


        var player = note.mustHit ? 1 : 2;


        vocalSource.mute = false;

        bool invertHealth = false;

        int noteType = note.type;
        int noteEvent = note.eventNote;
        CameraMovement.instance.focusOnPlayerOne = note.layer == 1;
        switch (player)
        {
            case 1:
                if(!Player.playAsEnemy || Player.demoMode || Player.twoPlayers)
                    invertHealth = false;
                switch (noteType)
                {

                    //10 alt-animation

                    case 0:
                        //Left
                        switch (noteEvent)
                        {
                            case 1:
                                
                                break;
                            case 2:
                                BoyfriendPlayAnimation($"Sing Left Alt");
                                break;
                            case 3:
                                BoyfriendPlayAnimation($"Sing Left Alt2");
                                break;
                            case 4:
                                BoyfriendPlayAnimationChar2($"Sing Left");
                                break;
                            case 5:
                                EnemyPlayAnimationChar2($"Sing Left");
                                break;
                            case 6:
                                BoyfriendPlayAnimationChar2($"Sing Left");
                                BoyfriendPlayAnimation($"Sing Left");
                                break;


                            case 100: //health note
                                BoyfriendPlayAnimation("Sing Left");
                                health += 50;
                                break;
                            default:
                                BoyfriendPlayAnimation("Sing Left");
                                break;
                        }

                        //Movement camera to left position
                        if (isActiveShake)
                        {
                            CameraMovement.instance.playerOneOffset.x = protagonist.xToLeft;
                            CameraMovement.instance.playerOneOffset.y = CameraMovement.instance._defaultPositionPlayer1.y;
                        }
                        
                        break;
                    case 1:
                        //Down
                        switch (noteEvent)
                        {
                            case 1:
                                break;
                            case 2:
                                BoyfriendPlayAnimation($"Sing Down Alt");
                                break;
                            case 3:
                                BoyfriendPlayAnimation($"Sing Down Alt2");
                                break;
                            case 4:
                                BoyfriendPlayAnimationChar2($"Sing Down");
                                break;
                            case 5:
                                EnemyPlayAnimationChar2($"Sing Down");
                                break;
                            case 6:
                                BoyfriendPlayAnimationChar2($"Sing Down");
                                BoyfriendPlayAnimation($"Sing Down");
                                break;

                            case 100: //health note
                                BoyfriendPlayAnimation("Sing Down");
                                health += 50;
                                break;
                            default:
                                BoyfriendPlayAnimation("Sing Down");
                                break;
                        }

                        //Movement camera to down position
                        if (isActiveShake)
                        {
                            CameraMovement.instance.playerOneOffset.y = protagonist.yToDown;
                            CameraMovement.instance.playerOneOffset.x = CameraMovement.instance._defaultPositionPlayer1.x;
                        }


                        break;
                    case 2:
                        //Up
                        switch (noteEvent)
                        {
                            case 1:
                                break;
                            case 2:
                                BoyfriendPlayAnimation($"Sing Up Alt");
                                break;
                            case 3:
                                BoyfriendPlayAnimation($"Sing Up Alt2");
                                break;
                            case 4:
                                BoyfriendPlayAnimationChar2($"Sing Up");
                                break;
                            case 5:
                                EnemyPlayAnimationChar2($"Sing Up");
                                break;
                            case 6:
                                BoyfriendPlayAnimationChar2($"Sing Up");
                                BoyfriendPlayAnimation($"Sing Up");
                                break;


                            case 100: //health note
                                BoyfriendPlayAnimation("Sing Up");
                                health += 50;
                                break;
                            default:
                                BoyfriendPlayAnimation("Sing Up");
                                break;
                        }

                        //Movement camera to up position
                        if (isActiveShake)
                        {
                            CameraMovement.instance.playerOneOffset.y = protagonist.yToUp;
                            CameraMovement.instance.playerOneOffset.x = CameraMovement.instance._defaultPositionPlayer1.x;
                        }

                        break;
                    case 3:
                        //Right
                        switch (noteEvent)
                        {
                            case 1:
                                break;
                            case 2:
                                BoyfriendPlayAnimation($"Sing Right Alt");
                                break;
                            case 3:
                                BoyfriendPlayAnimation($"Sing Right Alt2");
                                break;
                            case 4:
                                BoyfriendPlayAnimationChar2($"Sing Right");
                                break;
                            case 5:
                                EnemyPlayAnimationChar2($"Sing Right");
                                break;
                            case 6:
                                BoyfriendPlayAnimationChar2($"Sing Right");
                                BoyfriendPlayAnimation($"Sing Right");
                                break;

                            case 100: //health note
                                BoyfriendPlayAnimation("Sing Right");
                                health += 50;
                                break;
                            default:
                                BoyfriendPlayAnimation("Sing Right");
                                break;
                        }

                        //Movement camera to right position
                        if (isActiveShake)
                        {
                            CameraMovement.instance.playerOneOffset.x = protagonist.xToRight;
                            CameraMovement.instance.playerOneOffset.y = CameraMovement.instance._defaultPositionPlayer1.y;
                        }


                        break;
                }
                AnimateNote(1, noteType,"Activated");
                
                break;
            case 2:
                if(Player.playAsEnemy || Player.demoMode || Player.twoPlayers)
                    invertHealth = true;
                switch (noteType)
                {
                    case 0:
                        //Left
                        switch (noteEvent)
                        {
                            case 1:
                                
                                break;
                            case 2:
                                EnemyPlayAnimation($"Sing Left Alt");
                                break;
                            case 3:
                                EnemyPlayAnimation($"Sing Left Alt2");
                                break;
                            case 4:
                                BoyfriendPlayAnimationChar2($"Sing Left");
                                break;
                            case 5:
                                EnemyPlayAnimationChar2($"Sing Left");
                                break;
                            case 6:
                                EnemyPlayAnimationChar2($"Sing Left");
                                EnemyPlayAnimation($"Sing Left");
                                break;
                            case 9:
                                return;


                            case 101:
                                EnemyPlayAnimationAdditional($"Sing Left", 0);
                                break;
                            case 102:
                                EnemyPlayAnimationAdditional($"Sing Left", 1);
                                break;
                            case 103:
                                EnemyPlayAnimationAdditional($"Sing Left", 2);
                                break;
                            case 104:
                                EnemyPlayAnimationAdditional($"Sing Left", 3);
                                break;


                            default:
                                EnemyPlayAnimation("Sing Left");
                                break;
                        }

                        if (isActiveShake)
                        {
                            CameraMovement.instance.playerTwoOffset.x = enemy.xToLeft;
                            CameraMovement.instance.playerTwoOffset.y = CameraMovement.instance._defaultPositionPlayer2.y;
                        }
                        if(enemy.isBobingWhileSing)
                            CameraMovement.instance.startBob = true;

                        break;
                    case 1:
                        //Down
                        switch (noteEvent)
                        {
                            case 1:
                                break;
                            case 2:
                                EnemyPlayAnimation("Sing Down Alt");
                                break;
                            case 3:
                                EnemyPlayAnimation($"Sing Down Alt2");
                                break;
                            case 4:
                                BoyfriendPlayAnimationChar2($"Sing Down");
                                break;
                            case 5:
                                EnemyPlayAnimationChar2($"Sing Down");
                                break;
                            case 6:
                                EnemyPlayAnimationChar2($"Sing Down");
                                EnemyPlayAnimation($"Sing Down");
                                break;
                            case 9:
                                return;


                            case 101:
                                EnemyPlayAnimationAdditional($"Sing Down", 0);
                                break;
                            case 102:
                                EnemyPlayAnimationAdditional($"Sing Down", 1);
                                break;
                            case 103:
                                EnemyPlayAnimationAdditional($"Sing Down", 2);
                                break;
                            case 104:
                                EnemyPlayAnimationAdditional($"Sing Down", 3);
                                break;


                            default:
                                EnemyPlayAnimation("Sing Down");
                                break;
                        }

                        if (isActiveShake)
                        {
                            CameraMovement.instance.playerTwoOffset.y = enemy.yToDown;
                            CameraMovement.instance.playerTwoOffset.x = CameraMovement.instance._defaultPositionPlayer2.x;
                        }
                        if (enemy.isBobingWhileSing)
                            CameraMovement.instance.startBob = true;

                        break;
                    case 2:
                        //Up
                        switch (noteEvent)
                        {
                            case 1:
                                break;
                            case 2:
                                EnemyPlayAnimation("Sing Up Alt");
                                break;
                            case 3:
                                EnemyPlayAnimation($"Sing Up Alt2");
                                break;
                            case 4:
                                BoyfriendPlayAnimationChar2($"Sing Up");
                                break;
                            case 5:
                                EnemyPlayAnimationChar2($"Sing Up");
                                break;
                            case 6:
                                EnemyPlayAnimationChar2($"Sing Up");
                                EnemyPlayAnimation($"Sing Up");
                                break;
                            case 9:
                                return;


                            case 101:
                                EnemyPlayAnimationAdditional($"Sing Up", 0);
                                break;
                            case 102:
                                EnemyPlayAnimationAdditional($"Sing Up", 1);
                                break;
                            case 103:
                                EnemyPlayAnimationAdditional($"Sing Up", 2);
                                break;
                            case 104:
                                EnemyPlayAnimationAdditional($"Sing Up", 3);
                                break;


                            default:
                                EnemyPlayAnimation("Sing Up");
                                break;
                        }

                        if (isActiveShake)
                        {
                            CameraMovement.instance.playerTwoOffset.y = enemy.yToUp;
                            CameraMovement.instance.playerTwoOffset.x = CameraMovement.instance._defaultPositionPlayer2.x;
                        }
                        if (enemy.isBobingWhileSing)
                            CameraMovement.instance.startBob = true;

                        break;
                    case 3:
                        //Right
                        switch (noteEvent)
                        {
                            case 1:
                                break;
                            case 2:
                                EnemyPlayAnimation("Sing Right Alt");
                                break;
                            case 3:
                                EnemyPlayAnimation($"Sing Right Alt2");
                                break;
                            case 4:
                                BoyfriendPlayAnimationChar2($"Sing Right");
                                break;
                            case 5:
                                EnemyPlayAnimationChar2($"Sing Right");
                                break;
                            case 6:
                                EnemyPlayAnimationChar2($"Sing Right");
                                EnemyPlayAnimation($"Sing Right");
                                break;
                            case 9:
                                return;


                            case 101:
                                EnemyPlayAnimationAdditional($"Sing Right", 0);
                                break;
                            case 102:
                                EnemyPlayAnimationAdditional($"Sing Right", 1);
                                break;
                            case 103:
                                EnemyPlayAnimationAdditional($"Sing Right", 2);
                                break;
                            case 104:
                                EnemyPlayAnimationAdditional($"Sing Right", 3);
                                break;


                            default:
                                EnemyPlayAnimation("Sing Right");
                                break;
                        }

                        if (isActiveShake)
                        {
                            CameraMovement.instance.playerTwoOffset.x = enemy.xToRight;
                            CameraMovement.instance.playerTwoOffset.y = CameraMovement.instance._defaultPositionPlayer2.y;
                        }
                        if (enemy.isBobingWhileSing)
                            CameraMovement.instance.startBob = true;


                        break;
                }
                AnimateNote(2, noteType,"Activated");
                break;
        }

        bool modifyScore = true;

        if (player == 1 & Player.playAsEnemy & !Player.twoPlayers)
            modifyScore = false;
        else if (player == 2 & !Player.playAsEnemy & !Player.twoPlayers)
            modifyScore = false;

        if (Player.demoMode) modifyScore = true;

        CameraMovement.instance.focusOnPlayerOne = note.layer == 1;
        CameraMovement.instance.focusOnPlayerOne = note.layer == 1;
        

        Rating rating;
        if(!note.susNote & modifyScore)
        {
            if (player == 1)
            {
                playerOneStats.totalNoteHits++;
            }
            else
            {
                playerTwoStats.totalNoteHits++;
            }

            float yPos = note.transform.position.y;

            var newRatingObject = player == 1 ? liteRatingObjectP1 : liteRatingObjectP2;
            Vector3 ratingPos = newRatingObject.transform.position;
            
            ratingPos.y = OptionsV2.Downscroll ? 6 : 1;
            
            newRatingObject.transform.position = ratingPos;

            var ratingObjectScript = newRatingObject.GetComponent<RatingObject>();

            /*
             * Rating and difference calulations from FNF Week 6 update
             */
            
            float noteDiff = Math.Abs(note.strumTime - (stopwatch.ElapsedMilliseconds + stopwatchWithCheckPoint) + Player.visualOffset+Player.inputOffset);

            if (noteDiff > 0.9 * Player.safeZoneOffset)
            {
                // way early or late
                rating = Rating.Shit;
            }
            else if (noteDiff > .75 * Player.safeZoneOffset)
            {
                // early or late
                rating = Rating.Bad;
            }
            else if (noteDiff > .35 * Player.safeZoneOffset)
            {
                // your kinda there
                rating = Rating.Good;
            }
            else
            {
                rating = Rating.Sick;
            }

            if (Player.demoMode)
            {
                rating = Rating.Sick;
            }

            switch (rating)
            {
                case Rating.Sick:
                {
                        if (!Player.playAsEnemy && !Player.demoMode && OptionsV2.Splashes)
                        {
                            player1NotesAnimators[noteType].Splash();
                        }
                        else if (Player.playAsEnemy && !Player.demoMode && OptionsV2.Splashes)
                        {
                            player2NotesAnimators[noteType].Splash();
                        }
                        ratingObjectScript.sprite.sprite = sickSprite;

                    if(!invertHealth)
                        health += 5;
                    else
                        health -= 5;
                    if (player == 1)
                    {
                        playerOneStats.currentCombo++;
                        playerOneStats.totalSicks++;
                        playerOneStats.currentScore += 10;
                    }
                    else
                    {
                        playerTwoStats.currentCombo++;
                        playerTwoStats.totalSicks++;
                        playerTwoStats.currentScore += 10;
                    }
                    break;
                }
                case Rating.Good:
                {
                    ratingObjectScript.sprite.sprite = goodSprite;

                    if (!invertHealth)
                        health += 2;
                    else
                        health -= 2;
                
                    if (player == 1)
                    {
                        playerOneStats.currentCombo++;
                        playerOneStats.totalGoods++;
                        playerOneStats.currentScore += 5;
                    }
                    else
                    {
                        playerTwoStats.currentCombo++;
                        playerTwoStats.totalGoods++;
                        playerTwoStats.currentScore += 5;
                    }
                    break;
                }
                case Rating.Bad:
                {
                    ratingObjectScript.sprite.sprite = badSprite;

                    if (!invertHealth)
                        health += 1;
                    else
                        health -= 1;

                    if (player == 1)
                    {
                        playerOneStats.currentCombo++;
                        playerOneStats.totalBads++;
                        playerOneStats.currentScore += 1;
                    }
                    else
                    {
                        playerTwoStats.currentCombo++;
                        playerTwoStats.totalBads++;
                        playerTwoStats.currentScore += 1;
                    }
                    break;
                }
                case Rating.Shit:
                    ratingObjectScript.sprite.sprite = shitSprite;

                    if (player == 1)
                    {
                        playerOneStats.currentCombo = 0;
                        playerOneStats.totalShits++;
                    }
                    else
                    {
                        playerTwoStats.currentCombo = 0;
                        playerTwoStats.totalShits++;
                    }
                    break;
            }
            ratingObjectScript.ShowRating();
            
            if (player == 1)
            {
                if (playerOneStats.highestCombo < playerOneStats.currentCombo)
                {
                    playerOneStats.highestCombo = playerOneStats.currentCombo;
                }
                playerOneStats.hitNotes++;
                playerOneComboTimer = 3f;
                playerOneComboText.text = playerOneStats.currentCombo.ToString("000");

                playerOneComboText.alpha = 1;

                playerOneScoringText.rectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

            }
            else
            {
                if (playerTwoStats.highestCombo < playerTwoStats.currentCombo)
                {
                    playerTwoStats.highestCombo = playerTwoStats.currentCombo;
                }
                playerTwoStats.hitNotes++;
                
                playerTwoComboTimer = 3f;
                playerTwoComboText.text = playerTwoStats.currentCombo.ToString("000");

                playerTwoComboText.alpha = 1;
                
                
                playerOneScoringText.rectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }

            


            _currentRatingLayer++;
            ratingObjectScript.sprite.sortingOrder = _currentRatingLayer;
            ratingLayerTimer = _ratingLayerDefaultTime;
        }

        UpdateScoringInfo();
        if (player == 1)
        {
            player1NotesObjects[noteType].Remove(note);
        }
        else
        {
            player2NotesObjects[noteType].Remove(note);
        }
        Destroy(note.gameObject);
        if (note.susNote)
        {
            holdNotesPool.Release(note.gameObject);
        } else
        {

            switch (note.type)
            {
                case 0:
                    leftNotesPool.Release(note.gameObject);
                    break;
                case 1:
                    downNotesPool.Release(note.gameObject);
                    break;
                case 2:
                    upNotesPool.Release(note.gameObject);
                    break;
                case 3:
                    rightNotesPool.Release(note.gameObject);
                    break;
            }
        }

    }

    public void NoteMiss(NoteObject note)
    {


        


        var player = note.mustHit ? 1 : 2;
        

        bool invertHealth = player == 2;

        int noteType = note.type;
        int noteEvent = note.eventNote;
        bool isGhost = note.ghost;
        if(noteEvent == 2)
        {
            return;
        }
        if (isGhost)
        {
            return;
        }
        vocalSource.mute = true;
        oopsSource.clip = noteMissClip[Random.Range(0, noteMissClip.Length)];
        oopsSource.Play();
        string missString = "";
        if (!protagonist.noMissAnimations)
            missString = " Miss";
        
        switch (player)
        {
            case 1:
                switch (noteType)
                {
                    case 0:
                        //Left
                        switch (noteEvent)
                        {
                            case 5:
                                BoyfriendPlayAnimationChar2($"Sing Left{missString}");
                                break;
                            case 6:
                                BoyfriendPlayAnimationChar2($"Sing Left{missString}");
                                BoyfriendPlayAnimation($"Sing Left{missString}");
                                break;
                            case 100:
                                return;
                            default:
                                BoyfriendPlayAnimation($"Sing Left{missString}");
                                break;
                            
                        }
                        break;
                    case 1:
                        //Down
                        switch (noteEvent)
                        {
                            case 5:
                                BoyfriendPlayAnimationChar2($"Sing Down{missString}");
                                break;
                            case 6:
                                BoyfriendPlayAnimationChar2($"Sing Down{missString}");
                                BoyfriendPlayAnimation($"Sing Down{missString}");
                                break;
                            case 100:
                                return;
                            default:
                                BoyfriendPlayAnimation($"Sing Down{missString}");
                                break;
                        }
                        break;
                    case 2:
                        //Up
                        switch (noteEvent)
                        {
                            case 5:
                                BoyfriendPlayAnimationChar2($"Sing Up{missString}");
                                break;
                            case 6:
                                BoyfriendPlayAnimationChar2($"Sing Up{missString}");
                                BoyfriendPlayAnimation($"Sing Up{missString}");
                                break;
                            case 100:
                                return;
                            default:
                                BoyfriendPlayAnimation($"Sing Up{missString}");
                                break;
                        }
                        break;
                    case 3:
                        //Right
                        switch (noteEvent)
                        {
                            case 5:
                                BoyfriendPlayAnimationChar2($"Sing Right{missString}");
                                break;
                            case 6:
                                BoyfriendPlayAnimationChar2($"Sing Right{missString}");
                                BoyfriendPlayAnimation($"Sing Right{missString}");
                                break;
                            case 100:
                                return;
                            default:
                                BoyfriendPlayAnimation($"Sing Right{missString}");
                                break;
                        }
                        break;
                }
                break;
            default:
                switch (noteType)
                {
                    case 0:
                        //Left
                        switch (noteEvent)
                        {
                            case 5:
                                EnemyPlayAnimationChar2($"Sing Left");
                                break;
                            case 6:
                                EnemyPlayAnimationChar2($"Sing Left");
                                EnemyPlayAnimation($"Sing Left");
                                break;
                            default:
                                EnemyPlayAnimation($"Sing Left");
                                break;
                        }
                        break;
                    case 1:
                        //Down
                        switch (noteEvent)
                        {
                            case 5:
                                EnemyPlayAnimationChar2($"Sing Down");
                                break;
                            case 6:
                                EnemyPlayAnimationChar2($"Sing Down");
                                EnemyPlayAnimation($"Sing Down");
                                break;
                            default:
                                EnemyPlayAnimation($"Sing Down");
                                break;
                        }
                        break;
                    case 2:
                        //Up
                        switch (noteEvent)
                        {
                            case 5:
                                EnemyPlayAnimationChar2($"Sing Up");
                                break;
                            case 6:
                                EnemyPlayAnimationChar2($"Sing Up");
                                EnemyPlayAnimation($"Sing Up");
                                break;
                            default:
                                EnemyPlayAnimation($"Sing Up");
                                break;
                        }
                        break;
                    case 3:
                        //Right
                        switch (noteEvent)
                        {
                            case 5:
                                EnemyPlayAnimationChar2($"Sing Right");
                                break;
                            case 6:
                                EnemyPlayAnimationChar2($"Sing Right");
                                EnemyPlayAnimation($"Sing Right");
                                break;
                            default:
                                EnemyPlayAnimation($"Sing Right");
                                break;
                        }
                        break;
                }
                break;
        }

        bool modifyHealth = true;

        if (player == 1 & Player.playAsEnemy & !Player.twoPlayers)
            modifyHealth = false;
        else if (player == 2 & !Player.playAsEnemy & !Player.twoPlayers)
            modifyHealth = false;

        if (modifyHealth)
        {
            if (!invertHealth)
                health -= 8;
            else
                health += 8;
        }

        if (player == 1)
        {
            playerOneStats.currentScore -= 5;
            playerOneStats.currentCombo = 0;
            playerOneStats.missedHits++;
            playerOneStats.totalNoteHits++;
        }
        else
        {
            playerTwoStats.currentScore -= 5;
            playerTwoStats.currentCombo = 0;
            playerTwoStats.missedHits++;
            playerTwoStats.totalNoteHits++;
        }
        
        UpdateScoringInfo();

    }

#endregion

    
    IEnumerator CoutDownToDoneMusic(float timeSong)
    {
        yield return new WaitForSeconds(timeSong);
        songEnded = true;
    }

    /*private void OnApplicationFocus(bool focus)
    {
        focusedGame = focus;
        if (focusedGame)
        {
            Time.timeScale = 1;
            if(stopwatch != null)
                stopwatch.Start();
            if(beatStopwatch!= null)
                beatStopwatch.Start();
            /*musicSourceAndr[0].UnPause();
            vocalSourceAndr.UnPause();
            AudioListener.pause = false;
        }
        else
        {
            if (stopwatch != null)
                stopwatch.Stop();
            if(beatStopwatch != null)
                beatStopwatch.Stop();

            /*musicSourceAndr[0].Pause();
            vocalSourceAndr.Pause();
            AudioListener.pause = true;
            Time.timeScale = 0;
        }
        if (isMenu)
        {
            if(stopwatch != null)
                stopwatch.Stop();
            if(beatStopwatch != null)
                beatStopwatch.Stop();
            /*musicSourceAndr[0].Pause();
            vocalSourceAndr.Pause();
            AudioListener.pause = true;
            Time.timeScale = 0;
        }
        Debug.LogError(focus);
    }*/
    public int currentEvent;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            musicSources[0].Stop();
            songStarted = true;
            isDead = false;
            respawning = false;
            Pause.instance.pauseScreen.SetActive(false);
            Pause.instance.editingVolume = false;
            songstrat = true;
            songEnded = true;
            SpawnPointManager.timeLastSpawnPoint = 0;
            SpawnPointManager.timeLastSpawnPointMilliseconds = 0;
            SpawnPointManager.numberOfCheckpoint = 0;


        }
        if (!songstrat)
        {
            if(_song.Events.Length > currentEvent)
                if(stopwatchWithCheckPoint > (float)_song.Events[currentEvent].Time)
                    currentEvent++;
            if(SpawnPointManager.numberOfCheckpoint > 0 && !isLoadedEvents)
            {
                EventInScene.instance.FindVoid("LoadBGSave", SpawnPointManager.numberOfCheckpoint); // IMPORTANT!!!!
            }
        }

        if (musicSources[0].isPlaying)
        {
            if (songSetupDone)
            {
                if (modInstance != null)
                    modInstance?.Invoke("Update");
                
                if (_noteBehaviours.Count > 0)
                    foreach (NoteBehaviour nBeh in _noteBehaviours)
                        if (nBeh.count < 1)
                            nBeh.GenerateNote();

                

                if (songStarted & musicSources[0].isPlaying)
                {
                    if (currentEvent >= 0 && currentEvent < _song.Events.Length)
                    {
                        if (musicSources[0].time * 1000 - Player.visualOffset >= (float)_song.Events[currentEvent].Time)
                        {
                            
                            if (isTwoParametres)
                            {
                                
                                EventInScene.instance.FindVoid(_song.Events[currentEvent].Details[0], _song.Events[currentEvent].Details[1]);
                            }
                            else
                            {
                                if (_song.Events[currentEvent].Details.Length == 3)
                                {
                                    if (_song.Events[currentEvent].Details[2] == "MainEvent") 
                                    {
                                        System.Type type = MainEventSystem.instance.GetType();
                                        System.Reflection.MethodInfo method = type.GetMethod(_song.Events[currentEvent].Details[0]);
                                        method.Invoke(MainEventSystem.instance, new object[] { _song.Events[currentEvent].Details[1] });
                                    }
                                    else
                                        EventInScene.instance.FindVoid(_song.Events[currentEvent].Details[0], _song.Events[currentEvent].Details[1], _song.Events[currentEvent].Details[2]);
                                }
                                else if(_song.Events[currentEvent].Details.Length == 4)
                                {
                                    if (_song.Events[currentEvent].Details[3] == "MainEvent")
                                    {
                                        System.Type type = MainEventSystem.instance.GetType();
                                        System.Reflection.MethodInfo method = type.GetMethod(_song.Events[currentEvent].Details[0]);
                                        method.Invoke(MainEventSystem.instance, new object[] { _song.Events[currentEvent].Details[1], _song.Events[currentEvent].Details[2] });
                                    }
                                }
                                else if (_song.Events[currentEvent].Details.Length == 1)
                                {
                                    EventInScene.instance.FindVoid(_song.Events[currentEvent].Details[0]);
                                }
                                else
                                {
                                    EventInScene.instance.FindVoid(_song.Events[currentEvent].Details[0], _song.Events[currentEvent].Details[1]);
                                }
                            }
                           
                            

                            currentEvent++;
                        }
                    }
                    /*musicSourceAndr[0].Loop = false;
                    musicSourceAndr[1].Loop = false;*/
                    playerOneComboTimer -= Time.deltaTime;
                    playerTwoComboTimer -= Time.deltaTime;
                    songstrat = true;
                    playerOneComboText.enabled = !(playerOneComboTimer <= 0);

                    playerTwoComboText.enabled = !(playerTwoComboTimer <= 0);

                    playerOneComboText.rectTransform.localScale = Vector3.Lerp(playerOneComboText.rectTransform.localScale,
                        new Vector3(1f, 1f, 1), playerOneScoreLerpSpeed);
                    playerTwoComboText.rectTransform.localScale = Vector3.Lerp(playerTwoComboText.rectTransform.localScale,
                        new Vector3(1, 1, 1), playerTwoScoreLerpSpeed);

                    if (OptionsV2.SongDuration)
                    {
                        float t;
                        t = musicClip.length - musicSources[0].time;

                        int seconds = (int)(t % 60); // return the remainder of the seconds divide by 60 as an int
                        t /= 60; // divide current time y 60 to get minutes
                        int minutes = (int)(t % 60); //return the remainder of the minutes divide by 60 as an int

                        songDurationText.text = minutes + ":" + seconds.ToString("00");

                        songDurationBar.fillAmount = musicSources[0].time / musicClip.length;
                    }
                    if ((float)beatStopwatch.ElapsedMilliseconds / 1000 >= beatsPerSecond)
                    {
                        beatStopwatch.Restart();
                        currentBeat++;

                        if(modInstance != null)
                            modInstance?.Invoke("OnBeat", currentBeat);
                        if (_currentBoyfriendIdleTimer <= 0 & currentBeat % 2 == 0)
                        {
                            boyfriendAnimator.Play("BF Idle");
                            CameraMovement.instance.playerOneOffset = CameraMovement.instance._defaultPositionPlayer1;
                            
                        }

                        if (_currentBoyfriend2IdleTimer <= 0 & currentBeat % 2 == 0)
                        {
                            if (Enemy1Animator.instance)
                            {
                                Enemy1Animator.instance.animator.Play("BF Idle");
                                if(Enemy1Animator.instance.additionalAnimator != null)
                                {
                                    Enemy1Animator.instance.animator.Play("BF Idle");
                                }
                            }
                        }


                        if (_currentEnemyIdleTimer <= 0 & currentBeat % 2 == 0)
                        {
                            opponentAnimator.Play("Idle");
                            CameraMovement.instance.playerTwoOffset = CameraMovement.instance._defaultPositionPlayer2;
                            
                        }
                        if (_currentEnemy2IdleTimer <= 0 & currentBeat % 2 == 0)
                        {
                            if (Enemy2Animator.instance)
                            {
                                Enemy2Animator.instance.animator.Play("Idle");
                                if(Enemy2Animator.instance.additionalAnimators.Length > 0)
                                {
                                    for (int i = 0; i < Enemy2Animator.instance.additionalAnimators.Length; i++)
                                    {
                                        Enemy2Animator.instance.additionalAnimators[i].Play("Idle");
                                    }
                                }
                                    

                            }
                        }



                        if (!OptionsV2.LiteMode)
                        {
                            if (altDance)
                            {
                                //girlfriendAnimator.Play("GF Dance Left");
                                altDance = false;
                            }
                            else
                            {
                                //girlfriendAnimator.Play("GF Dance Right");
                                altDance = true;
                            }

                            boyfriendHealthIconRect.localScale = new Vector3(-1.25f, 1.25f, 1);
                            enemyHealthIconRect.localScale = new Vector3(1.25f, 1.25f, 1);

                            if (currentBeat % 4 == 0)
                            {
                                mainCamera.orthographicSize = defaultGameZoom - .2f;
                                uiCamera.orthographicSize = _defaultZoom - .2f;
                            }
                        }
                    }

                    boyfriendHealthIconRect.localScale =
                        Vector3.Lerp(boyfriendHealthIconRect.localScale, new Vector3(-1, 1, 1), portraitBopLerpSpeed);
                    enemyHealthIconRect.localScale =
                        Vector3.Lerp(enemyHealthIconRect.localScale, new Vector3(1, 1, 1), portraitBopLerpSpeed);

                    mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, defaultGameZoom, cameraBopLerpSpeed);
                    uiCamera.orthographicSize = Mathf.Lerp(uiCamera.orthographicSize, _defaultZoom, cameraBopLerpSpeed);
                }


                if (health > MAXHealth)
                    health = MAXHealth;
                


                

                
            }
            else
            {
                _bfRandomDanceTimer -= Time.deltaTime;
                _enemyRandomDanceTimer -= Time.deltaTime;

                if (_bfRandomDanceTimer <= 0)
                {
                    switch (Random.Range(0, 4))
                    {
                        case 1:
                            BoyfriendPlayAnimation("Sing Left");
                            break;
                        case 2:
                            BoyfriendPlayAnimation("Sing Down");
                            break;
                        case 3:
                            BoyfriendPlayAnimation("Sing Up");
                            break;
                        case 4:
                            BoyfriendPlayAnimation("Sing Right");
                            break;
                        default:
                            BoyfriendPlayAnimation("Sing Left");
                            break;
                    }

                    _bfRandomDanceTimer = Random.Range(.5f, 3f);
                }
                if (_enemyRandomDanceTimer <= 0)
                {
                    switch (Random.Range(0, 4))
                    {
                        case 1:
                            EnemyPlayAnimation("Sing Left");
                            break;
                        case 2:
                            EnemyPlayAnimation("Sing Down");
                            break;
                        case 3:
                            EnemyPlayAnimation("Sing Up");
                            break;
                        case 4:
                            EnemyPlayAnimation("Sing Right");
                            break;
                        default:
                            EnemyPlayAnimation("Sing Left");
                            break;
                    }

                    _enemyRandomDanceTimer = Random.Range(.5f, 3f);
                }
            }

            if((stopwatch.ElapsedMilliseconds + stopwatchWithCheckPoint) > (stopwatchWithCheckPoint + 1000))
                withCheckPoint = false;
        }

        if (!musicSources[0].isPlaying && songStarted && !isDead && !respawning && !Pause.instance.pauseScreen.activeSelf && !Pause.instance.editingVolume && songstrat && songEnded)
        {
            SpawnPointManager.timeLastSpawnPoint = 0;
            SpawnPointManager.timeLastSpawnPointMilliseconds = 0;
            SpawnPointManager.numberOfCheckpoint = 0;

            PlayerPrefs.SetInt("Complete-" + currentSong.songName, 1);


            if (!Player.demoMode)
            {



                if (!Player.playAsEnemy && !Player.demoMode && playerOneStats.currentScore > PlayerPrefs.GetFloat("statsBf" + currentSong.songIndex))
                {
                    PlayerPrefs.SetFloat("statsBf" + currentSong.songIndex, playerOneStats.currentScore);
                }
                else if (Player.playAsEnemy && !Player.demoMode && playerTwoStats.currentCombo > PlayerPrefs.GetFloat("statsDad" + currentSong.songIndex))
                {
                    PlayerPrefs.SetFloat("statsDad" + currentSong.songIndex, playerTwoStats.currentScore);
                }

                if (!Player.playAsEnemy && !Player.demoMode && accuracyBf > PlayerPrefs.GetFloat("accuracyBf" + currentSong.songIndex))
                {
                    PlayerPrefs.SetFloat("accuracyBf" + currentSong.songIndex, accuracyBf);
                }
                else if (Player.playAsEnemy && !Player.demoMode && accuracyDad > PlayerPrefs.GetFloat("accuracyDad" + currentSong.songIndex))
                {
                    PlayerPrefs.SetFloat("accuracyDad" + currentSong.songIndex, accuracyDad);
                }
            }

            MenuV2.startPhase = MenuV2.StartPhase.SongList;

            LeanTween.cancelAll();

            stopwatch.Stop();
            beatStopwatch.Stop();

            if (usingSubtitles)
            {
                subtitleDisplayer.StopSubtitles();
                subtitleDisplayer.paused = false;
                usingSubtitles = false;

            }
            //girlfriendAnimator.Play("GF Dance Loop");
            //boyfriendAnimator.Play("BF Idle Loop");


            Player.demoMode = false;

            songSetupDone = false;
            songStarted = false;
            foreach (List<NoteObject> noteList in player1NotesObjects.ToList())
            {
                foreach (NoteObject noteObject in noteList.ToList())
                {
                    noteList.Remove(noteObject);
                }
            }

            foreach (List<NoteObject> noteList in player2NotesObjects.ToList())
            {
                foreach (NoteObject noteObject in noteList.ToList())
                {
                    noteList.Remove(noteObject);

                }
            }
            leftNotesPool.ReleaseAll();
            downNotesPool.ReleaseAll();
            upNotesPool.ReleaseAll();
            rightNotesPool.ReleaseAll();
            holdNotesPool.ReleaseAll();
            battleCanvas.enabled = false;

            player1Notes.gameObject.SetActive(false);
            player2Notes.gameObject.SetActive(false);

            healthBar.SetActive(false);


            menuScreen.SetActive(false);
            /*if (weekMode)
            {

            }
             = weekMode ? currentWeek.songs[currentWeekIndex].songName : currentSong.songName;
            
            if(weekMode && currentWeek.isNonsenseWeek)
            {
                songName = NonsenseWeekChecker.instance.songs[currentWeekIndex].songName;
            }*/
            if (!_quitting)
            {
                /*string highScoreSave = songName +
                                       modeOfPlay;*/

                int overallScore = 0;

                // currentHighScore = PlayerPrefs.GetInt(highScoreSave, 0);

                switch (modeOfPlay)
                {
                    //Boyfriend
                    case 1:
                        overallScore = playerOneStats.currentScore;
                        break;
                    //Opponent
                    case 2:
                        overallScore = playerTwoStats.currentScore;
                        break;
                    //Local Multiplayer
                    case 3:
                        overallScore = playerOneStats.currentScore + playerTwoStats.currentScore;
                        break;
                    //Auto
                    case 4:
                        overallScore = 0;
                        break;
                }

                /*if (overallScore > currentHighScore)
                {
                    PlayerPrefs.SetInt(highScoreSave, overallScore);
                    PlayerPrefs.Save();
                }*/
            }

            


            if (weekMode & !_quitting)
            {
                currentWeekIndex++;
                if (currentWeekIndex <= currentWeek.songs.Length - 1)
                {

                    StartCoroutine(NextSong());


                }
                else
                {
                    PlayerPrefs.SetInt(currentWeek.weekName, 0);
                    if (currentSong.songName == "everlasting")
                    {
                        StartCoroutine(LastVideo());
                    }
                    LoadingTransition.instance.Show(() =>
                    {
                        SceneManager.LoadScene("Title", LoadSceneMode.Single);
                    });

                }
            }
            else
            {
                Debug.LogWarning("DoneEnd");
                //SceneManager.LoadScene("Title");
                LoadingTransition.instance.Show(() =>
                {
                    //AdLoader.instance.ShowInterstitial();
                    SceneManager.LoadScene("Title");
                });
            }





        }
        float healthPercent = health / MAXHealth;
        boyfriendHealthBar.fillAmount = healthPercent;
        bfOverlay.fillAmount = healthPercent;
        enemyHealthBar.fillAmount = 1 - healthPercent;

        var rectTransform = enemyHealthIcon.rectTransform;
        var anchoredPosition = rectTransform.anchoredPosition;
        Vector2 enemyPortraitPos = anchoredPosition;
        enemyPortraitPos.x = -(healthPercent * 1018 - (512)) - 75;

        Vector2 boyfriendPortraitPos = anchoredPosition;
        boyfriendPortraitPos.x = -(healthPercent * 1018 - (512)) + 75;

        if (healthPercent >= .80f)
        {
            if(!currentSong.isCustomIcons)
                enemyHealthIcon.sprite = enemy.portraitDead;
            boyfriendHealthIcon.sprite = protagonist.portraitWin;
        }
        else if (healthPercent <= .20f)
        {
            if (!currentSong.isCustomIcons)
                enemyHealthIcon.sprite = enemy.portraitWin;
            boyfriendHealthIcon.sprite = protagonist.portraitDead;
        }
        else
        {
            if (!currentSong.isCustomIcons)
                enemyHealthIcon.sprite = enemy.portrait;
            boyfriendHealthIcon.sprite = protagonist.portrait;
        }



        if (health <= 0 || Input.GetKeyDown(Player.keybinds.resetKeyCode) && songStarted)
        {

            health = 0;
            if (!Player.playAsEnemy & !Player.twoPlayers & !Player.demoMode)
            {
                if (isDead)
                {
                    int deathCount = PlayerPrefs.GetInt("Death");
                    PlayerPrefs.SetInt("Death", deathCount++);
                    if (!respawning)
                    {

                        //restartButton.SetActive(true);
                        if(OptionsV2.Checkpoints && SpawnPointManager.numberOfCheckpoint > 0)
                            restartTextAndr.SetActive(true);

                        if (Input.GetKeyDown(Player.keybinds.pauseKeyCode) || restart)
                        {
                            musicSources[0].Stop();
                            StopAllCoroutines();
                            respawning = true;
                            restartButton.SetActive(false);
                            if (protagonist.isVideoDeath)
                            {
                                deadPlayer.Stop();
                                rawObj.SetActive(false);

                            }
                            else
                                deadBoyfriendAnimator.Play("Dead Confirm");
                            restart = false;

                            musicSources[0].PlayOneShot(deadConfirm);
                            deathBlackout.rectTransform.LeanAlpha(1, 3).setDelay(1).setOnComplete(() =>
                            {
                                
                                LoadingTransition.instance.Show(() =>
                                {
                                    if(deathCount >= 3)
                                    {
                                        deathCount = 0;
                                        PlayerPrefs.SetInt("Death", 0);
                                        //AdLoader.instance.ShowInterstitial();
                                    }
                                    SceneManager.LoadScene("Game_Backup3");
                                    //InterstitialAdShows.RequestInterstitial();
                                    //DiscordController.instance.EnableGameStateLoop = false;
                                });
                            });
                        }
                        else if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            musicSources[0].Stop();
                            respawning = true;
                            
                            LoadingTransition.instance.Show(() =>
                            {
                                //AdLoader.instance.ShowInterstitial();
                                SceneManager.LoadScene("Title");
                                //InterstitialAdShows.RequestInterstitial();
                                //DiscordController.instance.EnableGameStateLoop = false;
                            });
                        }
                    }
                }
                else
                {
                    isDead = true;

                    if(modInstance != null)
                        modInstance?.Invoke("OnDeath");


                    deathBlackout.color = Color.clear;

                    foreach (AudioSource source in musicSources)
                    {
                        source.Stop();
                    }

                    vocalSource.Stop();
                    musicSources[0].Stop();

                    if (protagonist.isVideoDeath)
                        musicSources[0].clip = protagonist.audioDeath;
                    else
                        musicSources[0].clip = deadNoise;

                    musicSources[0].Play();
                    StartCoroutine (EndTheme());

                    battleCanvas.enabled = false;

                    uiCamera.enabled = false;
                    mainCamera.enabled = false;
                    deadCamera.enabled = true;

                    beatStopwatch.Reset();
                    stopwatch.Reset();

                    subtitleDisplayer.StopSubtitles();
                    subtitleDisplayer.paused = false;

                    deadBoyfriend.transform.position = boyfriendObject.transform.position;
                    deadBoyfriend.transform.localScale = boyfriendObject.transform.localScale;

                    deadCamera.orthographicSize = mainCamera.orthographicSize;
                    deadCamera.transform.position = mainCamera.transform.position;

                    if (protagonist.isVideoDeath)
                    {
                        rawObj.SetActive(true);
                        deadPlayer.Play();
                    }
                    else
                        deadBoyfriendAnimator.Play("Dead Start");
                    
                    Vector3 newPos = deadBoyfriend.transform.position;
                    newPos.y += 2.95f;
                    newPos.z = -10;

                    LeanTween.move(deadCamera.gameObject, newPos, .5f).setEaseOutExpo();
                    float videoLength = (float)deadPlayer.length;
                    LeanTween.delayedCall(videoLength, () =>
                    {
                        if (!respawning)
                        {
                            if (protagonist.isVideoDeath)
                            {
                                
                                deadPlayer.clip = clipRetry;
                                deadPlayer.Play();
                            }
                            else
                                deadBoyfriendAnimator.Play("Dead Loop");
                        }
                    });
                }
            }
        }


        anchoredPosition = enemyPortraitPos;
        rectTransform.anchoredPosition = anchoredPosition;
        boyfriendHealthIcon.rectTransform.anchoredPosition = boyfriendPortraitPos;
        /*
        for (int i = 0; i < _currentEnemyNoteTimers.Length; i++)
        {
            if (Player.twoPlayers) continue;
            if (!Player.playAsEnemy)
            {
                if (player2NotesAnimators[i].GetCurrentAnimatorStateInfo(0).IsName("Activated"))
                {
                    _currentEnemyNoteTimers[i] -= Time.deltaTime;
                    if (_currentEnemyNoteTimers[i] <= 0)
                    {
                        AnimateNote(2, i, "Normal");
                    }
                }
            } else
            {
                if (player1NotesAnimators[i].GetCurrentAnimatorStateInfo(0).IsName("Activated"))
                {
                    _currentEnemyNoteTimers[i] -= Time.deltaTime;
                    if (_currentEnemyNoteTimers[i] <= 0)
                    {
                        AnimateNote(1, i, "Normal");
                    }
                }
            }

        }
        */



        if (ratingLayerTimer > 0)
        {
            ratingLayerTimer -= Time.deltaTime;
            if (ratingLayerTimer < 0)
                _currentRatingLayer = 0;
        }
        
        /*
        if(Player.demoMode)
            for (int i = 0; i < _currentDemoNoteTimers.Length; i++)
            {
                if (player1NotesAnimators[i].GetCurrentAnimatorStateInfo(0).IsName("Activated"))
                {
                    _currentDemoNoteTimers[i] -= Time.deltaTime;
                    if (_currentDemoNoteTimers[i] <= 0)
                    {
                        AnimateNote(1, i, "Normal");
                    }
                }

            }
            */

        if (OptionsV2.DesperateMode) return;
        if ((opponentAnimator.CurrentAnimation == null || !opponentAnimator.CurrentAnimation.Name.Contains("Idle")) & !songStarted)
        {
            _currentEnemyIdleTimer -= Time.deltaTime;
            if (_currentEnemyIdleTimer <= 0)
            {
                //opponentAnimator.Play("Idle Loop");
                _currentEnemyIdleTimer = enemyIdleTimer;
            }
        }
        else
        {
            _currentEnemyIdleTimer -= Time.deltaTime;
        }

        if (!songStarted && Enemy2Animator.instance)
        {
            _currentEnemy2IdleTimer -= Time.deltaTime;
            if (_currentEnemy2IdleTimer <= 0)
            {
                _currentEnemy2IdleTimer = enemy2IdleTimer;
            }
            for (int i = 0; i < Enemy2Animator.instance.additionalAnimators.Length; i++)
            {
                Enemy2Animator.instance._currentEnemyAdditionalIdleTimer[i] -= Time.deltaTime;
                if(Enemy2Animator.instance._currentEnemyAdditionalIdleTimer[i] <= 0) Enemy2Animator.instance._currentEnemyAdditionalIdleTimer[i] = Enemy2Animator.instance.enemyAdditionalIdleTimer[i];
            }
        }
        else
        {
            _currentEnemy2IdleTimer -= Time.deltaTime;
        }

        if ( boyfriendAnimator.CurrentAnimation == null|| (!boyfriendAnimator.CurrentAnimation.Name.Contains("Idle")) & !songStarted)
        {

            _currentBoyfriendIdleTimer -= Time.deltaTime;
            if (_currentBoyfriendIdleTimer <= 0)
            {
                //boyfriendAnimator.Play("BF Idle Loop");
                _currentBoyfriendIdleTimer = boyfriendIdleTimer;
            }
        }
        else
        {
            _currentBoyfriendIdleTimer -= Time.deltaTime;
        }


        if (!songStarted && Enemy1Animator.instance)
        {
            _currentBoyfriend2IdleTimer -= Time.deltaTime;
            if (_currentBoyfriend2IdleTimer <= 0)
            {
                _currentBoyfriend2IdleTimer = boyfriend2IdleTimer;
            }
        }
        else
        {
            _currentBoyfriend2IdleTimer -= Time.deltaTime;
        }

    }
    private IEnumerator NextSong()
    {
        PlayerPrefs.SetInt(currentWeek.weekName, currentWeekIndex);


        if (currentWeek.songs[currentWeekIndex].videoRus != null || currentWeek.songs[currentWeekIndex].videoEng != null)
        {
            LoadingTransition.instance.Show(() =>
            {
                VideoPlayerScene.nextScene = "Game_Backup3";
            });
            yield return new WaitForSeconds(0.5f);
            VideoPlayerScene.videoToPlay = Application.systemLanguage == SystemLanguage.Russian ? currentWeek.songs[currentWeekIndex].videoRus : currentWeek.songs[currentWeekIndex].videoEng;
            SceneManager.LoadScene("Video", LoadSceneMode.Single);

        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("Game_Backup3", LoadSceneMode.Single);
        }

    }


    IEnumerator EndTheme()
    {

        yield return new WaitForSeconds((float)deadPlayer.length);
        /* musicSources[0].Stop();
         musicSources[0].PlayOneShot(quote[Random.Range(0, quote.Length)]);
         yield return new WaitForSeconds(1.5f);*/
        restartButton.SetActive(true);

        musicSources[0].clip = deadTheme;
        musicSources[0].loop = true;
        musicSources[0].Play();
    }



    public VideoClip lastVidEng;
    private IEnumerator LastVideo()
    {
        LoadingTransition.instance.Show(() =>
        {
            VideoPlayerScene.nextScene = "Title";

        });
        yield return new WaitForSeconds(0.5f);
        VideoPlayerScene.videoToPlay = Application.systemLanguage == SystemLanguage.Russian ? lastVidEng : lastVidEng;
        SceneManager.LoadScene("Video", LoadSceneMode.Single);
    }

}
