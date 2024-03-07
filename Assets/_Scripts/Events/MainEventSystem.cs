using DG.Tweening;
using FridayNightFunkin;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class MainEventSystem : MonoBehaviour
{
    public static MainEventSystem instance { get; private set; }
    #region Variables
    public GameObject par;
    public Image redConsequences;
    public Image redAlways;
    public Image blackConsequences;

    public Animator creditMenu;
    public Animator inGameAnimation;
    public Animator jumpScare;
    public TMP_Text subtitles;
    public Animator subtitleAnim;
    public Vector3 firstOffsetPlayer1;
    public Vector3 firstOffsetPlayer2;
    public bool isAnimationOver = true;
    public bool isAnimationOverBF = true;
    private bool isSetMiddleScroll = false;
    public bool effectsEnabled = false;
    private float elapsedTime;
    private float _targetSpeed;
    private float lastTime;
    public NoteObject[] c;
    public Character[] characters;
    public Protagonist[] protagonists;
    public Protagonist nonsensess, bff;

    public TMP_Text txtSpawnPoint;
    public Animator setSpawn;
    public AudioSource sourceSetSpawn;
    public AudioClip setSpawnClip;

    public GameObject subtitleImg;
    public TMP_Text subtitleText;
    #endregion

    public void SaveProgress(int number)
    {
        if (!Player.demoMode && !Player.playAsEnemy && OptionsV2.Checkpoints)
        {
            float time = (float)Song.instance._song.Events[Song.instance.currentEvent].Time;
            SpawnPointManager.SetSpawnPoint(number, time);
            setSpawn.SetTrigger("Check");
            sourceSetSpawn.PlayOneShot(setSpawnClip);
        }
    }
    public void ToggleSubtitles(string TargetText = "", string targetColor = null)
    {
        Color colorText = new Color();
        if(TargetText != "")
        {
            subtitleImg.SetActive(true);
            subtitleText.text = TargetText;
            ColorUtility.TryParseHtmlString(targetColor, out colorText);
            subtitleText.color = colorText;
        }
        else
        {
            subtitleImg.SetActive(false);
            subtitleText.text = "";
        }
    }
    
    private void Start()
    {
        redConsequences.CrossFadeAlpha(0, 0, false);
        redAlways.CrossFadeAlpha(0, 0, false);
        blackConsequences.CrossFadeAlpha(0,0,false);

        subtitleText.CrossFadeAlpha(1, 0, false);
        txtSpawnPoint.CrossFadeAlpha(0, 0, false);
        instance = this;
        firstOffsetPlayer1 = CameraMovement.instance.playerOneOffset;
        firstOffsetPlayer2 = CameraMovement.instance.playerTwoOffset;
        _targetSpeed = Song.instance._song.Speed;
    }
    private void Update()
    {
        //subtitles.alpha = 1 - Mathf.Clamp01(Time.time - lastTime - 1.5f);
        Song.instance._song.Speed = Mathf.MoveTowards(Song.instance._song.Speed, _targetSpeed, Time.deltaTime);
        c = (NoteObject[])FindObjectsByType(typeof(NoteObject), FindObjectsSortMode.None);
        for (int i = 0; i < c.Length; i++)
        {
            if (c[i])
                c[i].ScrollSpeed = Mathf.MoveTowards(c[i].ScrollSpeed, -_targetSpeed, Time.deltaTime);
        }

        if (isSetMiddleScroll && !OptionsV2.Middlescroll)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / 3f;
            if (!Player.playAsEnemy)
            {

                Song.instance.player2Notes.transform.position = Vector3.Lerp(new Vector3(-3.6f, 4.45f, 15), new Vector3(-15f, 4.45f, 15), percent);
                Song.instance.player1Notes.transform.position = Vector3.Lerp(new Vector3(3.6f, 4.45f, 15), new Vector3(0f, 4.45f, 15), percent);
            }
            else
            {
                Song.instance.player1Notes.position = Vector3.Lerp(new Vector3(3.6f, 4.45f, 15), new Vector3(15f, 4.45f, 15), percent);
                Song.instance.player2Notes.position = Vector3.Lerp(new Vector3(-3.6f, 4.45f, 15), new Vector3(0f, 4.45f, 15), percent);
            }
        }
    }

    // Each event has its own region to which it belongs, above the region all events belonging to this region are indicated

    // animation of player/enemy
    #region PlayerAnimation
    public void PlayCutSceneEnemy(string animationName)
    {
        StartCoroutine(PlayCutSceneEnemys(animationName));
    }
    private IEnumerator PlayCutSceneEnemys(string animationName)
    {
        isAnimationOver = false;

        CameraMovement.instance.enableMovement = true;
        CameraMovement.instance.focusWhenAnimations = true;
        CameraMovement.instance.focusWhenAnimationsPlayer = false;
        DadBackup.instance.dad.enabled = false;
        DadBG.instance.dad.enabled = true;
        DadBG.instance.dadAnim.enabled = true;
        DadBG.instance.dadAnim.SetTrigger(animationName);
        DadBG.instance.dadAnim.Update(Time.deltaTime);

        yield return new WaitUntil(() => isAnimationOver);

        DadBackup.instance.dad.enabled = true;
        DadBG.instance.dad.enabled = false;
        DadBG.instance.dadAnim.enabled = false;
        CameraMovement.instance.playerTwoOffset = firstOffsetPlayer2;
        CameraMovement.instance.focusWhenAnimations = false;
    }
    public void PlayCutScenePlayer(string animationName)
    {
        StartCoroutine(PlayCutScenePlayers(animationName));
    }
    private IEnumerator PlayCutScenePlayers(string animationName)
    {
        isAnimationOverBF = false;

        CameraMovement.instance.playerOneOffset = BoyfriendBG.instance.offset;
        BoyfriendBackup.instance.bf.enabled = false;
        BoyfriendBG.instance.bf.enabled = true;
        BoyfriendBG.instance.bfAnim.enabled = true;
        BoyfriendBG.instance.bfAnim.SetTrigger(animationName);

        yield return new WaitUntil(() => isAnimationOverBF);

        BoyfriendBackup.instance.bf.enabled = true;
        BoyfriendBG.instance.bf.enabled = false;
        BoyfriendBG.instance.bfAnim.enabled = false;
        CameraMovement.instance.playerOneOffset = firstOffsetPlayer1;
    }

    public void EndCutSceneBF()
    {
        BoyfriendBG.instance.OnEndAnim();
    }
    public void EndCutSceneDad()
    {
        DadBG.instance.OnEndAnim();
    }
    #endregion 

    // all about effects and changes characters
    #region Characters&Effects
    /*public void ChangeCharacterEnemy(string character)
    {
        if(Song.instance.charactersDictionary.ContainsKey(character))
        {
            CameraShake.instance.Flash("0.7");
            Character currentCharacter = Song.instance.charactersDictionary[character];
            DadBackup.instance.Change(currentCharacter);
            if (currentCharacter.isCustomPosition)
            {
                DadBackup.instance.dadTransform.position = currentCharacter.newTransform;
                if (currentCharacter.changeColor)
                {
                    Song.instance.ChangeColorDuration(currentCharacter.songDurationColor);
                }
            }

            Song.instance.enemy = Song.instance.charactersDictionary[character];
            Song.instance.enemyHealthIcon.sprite = Song.instance.enemy.portrait;
            Song.instance.enemyHealthBar.color = Song.instance.enemy.healthColor;
            CameraMovement.instance.playerTwoOffset = currentCharacter.cameraOffset;
            CameraMovement.instance._defaultPositionPlayer2 = currentCharacter.cameraOffset;
            DadBackup.instance.dadTransform.DOScale(currentCharacter.scale, 0);

        }

    }*/
    public void ChangeCharacterEnemyWithoutFlash(string character)
    {
        if (Song.instance.charactersDictionary.ContainsKey(character))
        {
            Character currentCharacter = Song.instance.charactersDictionary[character];
            DadBackup.instance.Change(currentCharacter);
            if (currentCharacter.isCustomPosition)
            {
                DadBackup.instance.dadTransform.position = currentCharacter.newTransform;
                if (currentCharacter.changeColor)
                {
                    Song.instance.ChangeColorDuration(currentCharacter.songDurationColor);
                }
            }
            /*if (currentCharacter.isRotated)
            {
                //float x = DadBackup.instance.dadTransform.localScale.x;
                DadBackup.instance.dadTransform.DOScaleX(-.7f, 0);
            }
            else
            {
                DadBackup.instance.dadTransform.DOScaleX(.7f, 0);
            }*/
            Song.instance.enemy = Song.instance.charactersDictionary[character];
            Song.instance.enemyHealthIcon.sprite = Song.instance.enemy.portrait;
            Song.instance.enemyHealthBar.color = Song.instance.enemy.healthColor;
            CameraMovement.instance.playerTwoOffset = currentCharacter.cameraOffset;
            CameraMovement.instance._defaultPositionPlayer2 = currentCharacter.cameraOffset;
            CameraMovement.instance.orthographicSizePlayerTwo = currentCharacter.orthographicSize;
            CameraMovement.instance._defaultOrthographicSizePlayer2 = currentCharacter.orthographicSize;

            DadBackup.instance.dadTransform.DOScale(currentCharacter.scale, 0);
        }

    }
    public void ChangeCharacterPlayer(string protagonist)
    {
        if(Song.instance.protagonistsDictionary.ContainsKey(protagonist))
        {
            Protagonist currentCharacter = Song.instance.protagonistsDictionary[protagonist];
            BoyfriendBackup.instance.Change(currentCharacter);
            if (currentCharacter.isCustomPosition)
                BoyfriendBackup.instance.bfTransform.position = currentCharacter.newTransform;
            Song.instance.protagonist = Song.instance.protagonistsDictionary[protagonist];
            Song.instance.boyfriendHealthIcon.sprite = Song.instance.protagonist.portrait;
            Song.instance.boyfriendHealthBar.color = Song.instance.protagonist.healthColor;
            CameraMovement.instance.playerOneOffset = currentCharacter.cameraOffset;
            CameraMovement.instance._defaultPositionPlayer1 = currentCharacter.cameraOffset;
            CameraMovement.instance.orthographicSizePlayerOne = currentCharacter.orthographicSize;
            CameraMovement.instance._defaultOrthographicSizePlayer1 = currentCharacter.orthographicSize;

            if (currentCharacter.isVideoDeath)
            {
                Song.instance.deadPlayer.clip = currentCharacter.videoDeath;
                Song.instance.deadPlayer.Prepare();
            }
            else
                Song.instance.deadBoyfriendAnimator.runtimeAnimatorController = currentCharacter.deathAnimator.runtimeAnimatorController;

           // Song.instance.boyfriendAnimator.transform.localScale = new Vector2(currentCharacter.scale, currentCharacter.scale);
            BoyfriendBackup.instance.bfTransform.DOScale(currentCharacter.scale, 0);
        }
        
    }
    public void CharacterBfChangeOld()
    {
        BoyfriendBackup.instance.gameObject.LeanScaleX(-1, 0);
        BoyfriendBackup.instance.Change(nonsensess);
        Song.instance.protagonist = nonsensess;
        Song.instance.boyfriendHealthIcon.sprite = Song.instance.protagonist.portrait;
        Song.instance.boyfriendHealthBar.color = Song.instance.protagonist.healthColor;
    }
    public void CharacterBfChangeOldNormall()
    {
        BoyfriendBackup.instance.gameObject.LeanScaleX(1, 0);
        BoyfriendBackup.instance.Change(bff);
        Song.instance.protagonist = bff;
        Song.instance.boyfriendHealthIcon.sprite = Song.instance.protagonist.portrait;
        Song.instance.boyfriendHealthBar.color = Song.instance.protagonist.healthColor;
    }



    public void EnableEffects()
    {
        effectsEnabled = true;
    }
    public void DisableEffects()
    {
        effectsEnabled = false;
        for (int i = 0; i <= 3; i++)
        {
            if (BoyfriendBackup.instance.effects[i].activeSelf)
                BoyfriendBackup.instance.effects[i].SetActive(false);
        }
    }
    public void StartRedImage()
    {
        redImage.instance.startRed();
    }
    public void EndRedImage()
    {
        redImage.instance.endRed();
    }
    #endregion

    //about UI Camera, UI notes, UI
    #region UI & Notes
    public void SetMiddleScroll()
    {
        isSetMiddleScroll = true;
        if (!OptionsV2.Middlescroll)
        {
            if (!Player.playAsEnemy)
            {
                foreach (SpriteRenderer sprite in Song.instance.player2NoteSprites)
                {
                    sprite.DOFade(1, 0);
                    sprite.DOFade(0, 2.2f);
                }
            }
            else
            {
                foreach (SpriteRenderer sprite in Song.instance.player1NoteSprites)
                {
                    sprite.DOFade(1, 0);
                    sprite.DOFade(0, 2.2f);
                }
            }
            StartCoroutine(HideScroll());
        }


    }
    IEnumerator HideScroll()
    {
        yield return new WaitForSeconds(2.4f);
        if (!Player.playAsEnemy)
        {
            foreach (SpriteRenderer sprite in Song.instance.player2NoteSprites)
            {
                sprite.enabled = false;
            }
        }
        else
        {
            foreach (SpriteRenderer sprite in Song.instance.player1NoteSprites)
            {
                sprite.enabled = false;
            }
        }
    }
    public void ChangeScrollSpeed(string speed)
    {
        float s = float.Parse(speed, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
        _targetSpeed = s;
    }
    public void SubtitlesOn(int index)
    {
        if (subtitlesInSong.instance)
        {
            subtitles.SetText(subtitlesInSong.instance.subtitlesText[index]);
            subtitleAnim.SetTrigger("NewString");
            lastTime = Time.time;
        }
    }
    IEnumerator HideSubtitles()
    {
        yield return new WaitForSeconds(3);
        subtitles.DOFade(0, 2);
    }
    public void TrustTest(string enable)
    {
        if(enable == "yes")
        {
            GamePlayCamera.instance.cam.enabled = false;
        }
        else
        {
            GamePlayCamera.instance.cam.enabled = true;
        }
    }

    #endregion

    #region Camera
    public void CameraZoomEnemy(string to, string time)
    {
        float s = float.Parse(to, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
        float d = float.Parse(time, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
        CameraMovement.instance.orthographicSizePlayerTwo = Song.instance.defaultGameZoom / s;
        Song.instance.mainCamera.orthographicSize = Mathf.Lerp(Song.instance.mainCamera.orthographicSize, Song.instance.defaultGameZoom, Time.deltaTime * d);
    }

    #endregion


    public GameObject statics;
    public Graphic redStatic, whiteStatic, staticss;
    public void Static(string staticNum) // 0 - toggle static, 1 - toggle red static, 2 - toggle 1 white static, 3 - off static
    {
        int s = int.Parse(staticNum, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
        switch (s)
        {
            case 0:
                statics.SetActive(true);
                redStatic.CrossFadeAlpha(0, 0, false);
                whiteStatic.CrossFadeAlpha(0, 0f, false);
                staticss.CrossFadeAlpha(1, 0f, false);
                break;
            case 1:
                redStatic.CrossFadeAlpha(1, 0, false);
                redStatic.CrossFadeAlpha(0, 2, false);
                break;
            case 2:
                whiteStatic.CrossFadeAlpha(0, 0, false);
                whiteStatic.CrossFadeAlpha(1, 1f, false);
                break;
            case 3:
                redStatic.CrossFadeAlpha(0, 0, false);
                whiteStatic.CrossFadeAlpha(0, 0f, false);
                staticss.CrossFadeAlpha(1, 0f, false);
                statics.SetActive(false);
                break;
            case 4:
                statics.SetActive(true);
                redStatic.CrossFadeAlpha(0, 0, false);
                whiteStatic.CrossFadeAlpha(0, 0f, false);
                staticss.CrossFadeAlpha(0, 0f, false);
                staticss.CrossFadeAlpha(0.7f, 2f, false);
                break;
        }
    }
    public void SongCredit(string songName)
    {
        creditMenu.SetTrigger(songName);
    }
    public void InGameAnimation(string animationName)
    {
        inGameAnimation.SetTrigger(animationName);
    }
    public Animator mangle;
    public AudioSource mangleSounds;
    public Animator tablet;
    public GameObject tabletObj;
    bool tabletClick = false;
    public Animator puppet;
    public GameObject fireMaterial;
    public void Mangle()
    {
        mangle.ResetTrigger("out");
        mangle.SetTrigger("in");
        if(OptionsV2.Middlescroll)
            mangle.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-260, mangle.gameObject.GetComponent<RectTransform>().anchoredPosition.y);
        if(Player.playAsEnemy)
            mangle.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-520, mangle.gameObject.GetComponent<RectTransform>().anchoredPosition.y);
        mangleSounds.Play();
    }
    public void DisableMangle()
    {
        mangle.SetTrigger("out");
        mangleSounds.Stop();
    }
    public void JumpScare(string name)
    {
        jumpScare.gameObject.SetActive(true);
        jumpScare.SetTrigger(name);
    }
    
    public void OnTablet()
    {
        tablet.SetTrigger("OnTable");
        tablet.ResetTrigger("Open");
        StartCoroutine(ConsequencesRed());
        StartCoroutine(ConsequencesBLack());
        /*if (isTablet)
        {
            isTablet = false;
            tabletClick = false;
            tablet.gameObject.SetActive(false);
        }*/
    }
    IEnumerator ConsequencesRed()
    {
        while (true)
        {
            redConsequences.CrossFadeAlpha(0.2f, 0f, false);
            redConsequences.CrossFadeAlpha(0f, 0.3f, false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator ConsequencesBLack()
    {
        while (true)
        {
            blackConsequences.CrossFadeAlpha(1f, 1f, false);
            yield return new WaitForSeconds(1f);
            blackConsequences.CrossFadeAlpha(0f, 1f, false);
            yield return new WaitForSeconds(1f);
        }
    }
    public void OnClickUpdateTablet()
    {
        tablet.SetTrigger("Open");
        tablet.ResetTrigger("OnTable");
    }
    public void OnClickTablet()
    {
        if (!tabletClick)
        {
            tabletClick = true;
            tablet.SetTrigger("RestartingSystem");

            LeanTween.delayedCall(3f, () =>
            {
                OnTablet();
                StopCoroutine(ConsequencesRed());
                StopCoroutine(ConsequencesBLack());
                StopAllCoroutines();
                tablet.ResetTrigger("OnTable");
                blackConsequences.CrossFadeAlpha(0f, 0f, false);
                redConsequences.CrossFadeAlpha(0f, 0f, false);
                tabletClick = false;
            });
        }
    }
    public void Puppet()
    {
        if (OptionsV2.Middlescroll)
        {
            puppet.SetBool("Middle", true);
        }
        else
        {
            puppet.SetBool("Middle", false);
        }
        if (Player.playAsEnemy)
        {
            puppet.SetBool("Left", true);
        }
        puppet.SetTrigger("Puppet");
    }
}
