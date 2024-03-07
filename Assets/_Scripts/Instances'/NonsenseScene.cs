using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonsenseScene : MonoBehaviour
{
    public static NonsenseScene Instance { get; private set; }

    public SpriteRenderer firstSprite, secondSprite;
    public Sprite[] OGSprites;
    public Sprite[] notOGSprites;
    public GameObject newsenses;

    private void Start()
    {
        Instance = this;
        firstSprite.sprite = OGSprites[0];
        secondSprite.sprite = OGSprites[1];
        if (Song.instance._song.SongName == "Newsensical")
        {
            DadBackup.instance.isFlying = true;
            newsenses.SetActive(true);
            firstSprite.enabled = false;
            secondSprite.enabled = false;
            if (OptionsV2.SongDuration)
            {
                float time = 4200;

                int seconds = (int)(time % 60); // return the remainder of the seconds divide by 60 as an int
                time /= 60; // divide current time y 60 to get minutes
                int minutes = (int)(time % 60); //return the remainder of the minutes divide by 60 as an int

                Song.instance.songDurationText.text = minutes + ":" + seconds.ToString("00");

                Song.instance.songDurationBar.fillAmount = 0;
            }
        }
        if(Song.currentSong.name == "Rethink")
        {
            BgFade(0, 0);
        }
    }

    public void BgFade(float to, float time)
    {
        firstSprite.DOColor(new Color(to, to, to, 1), time);
        secondSprite.DOColor(new Color(to, to, to, 1), time);
    }
    public void BgFadeCancel()
    {
        firstSprite.DOComplete();
        secondSprite.DOComplete();
    }


}
