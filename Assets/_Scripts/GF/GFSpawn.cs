using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFSpawn : MonoBehaviour
{
    public GameObject gf, gfRock, gf2;
    public Animator gfAnim, _def;
    public Animator gfAnimRock, gf2Anim;
    public static GFSpawn instance;

    public GameObject first, twice;

    public bool isEnabledAnim = true;
    public bool isGfEnabled = true;

    public SpriteRenderer sr;
    public void ChangeGF(bool isRock)
    {
        
        if (isGfEnabled)
        {
            if (isRock)
            {
                gf.SetActive(false);
                gfRock.SetActive(true);
            }
            else
            {
                gf.SetActive(true);
                gfRock.SetActive(false);
                gfAnim.SetTrigger("happyFromPirate");
                gfAnim.SetTrigger("happyFromPirate");
            }
        }

    }

    public GameObject bg1, bg2;
    public bool wowGfSing;
    public void ChangeGFRethink(bool a, bool isGFSing)
    {
        if (a)
        {
            gf.SetActive(false);
            gf2.SetActive(true);
            bg1.SetActive(false);
            bg2.SetActive(true);
            gfAnim = gf2Anim;
            if (!isGFSing)
            {
                gfAnim.SetTrigger("happyFromPirate");
                gfAnim.SetTrigger("happyFromPirate");
            }
            else
            {
                wowGfSing = true;
                gfAnim.SetTrigger("Idle");
                gfAnim.SetTrigger("Idle");
            }
            
        }
        else
        {
            gf.SetActive(true);
            gf2.SetActive(false);
            bg1.SetActive(true);
            bg2.SetActive(false);
            gfAnim = _def;
            if (!isGFSing)
            {
                gfAnim.SetTrigger("happyFromPirate");
                gfAnim.SetTrigger("happyFromPirate");
            }
            else
            {
                wowGfSing = true;
                gfAnim.SetTrigger("Idle");
                gfAnim.SetTrigger("Idle");
            }
            

        }
    }

    public void fadeGF(float to, float time)
    {
        sr.DOColor(new Color(to,to,to, 1), time);
    }


    private void Start()
    {
        sr.DOFade(1, 0);
        if(Song.currentSong.name == "Rethink")
        {
            sr.DOColor(new Color(0,0,0,1), 0);
        }
        instance = this;
        if (isEnabledAnim)
        {
            bool t = false;
           /* if (Song.currentWeek)
            {
                    t = Song.currentWeek.songs[Song.currentWeekIndex].isGfEnabled;
            }
            if (Song.currentSong.isGfEnabled || t)
            {
                gf.SetActive(true);
            }*/
        }
    }
    private void Update()
    {
        
        if (Song.instance.isGameStartGf && isEnabledAnim)
        {
            gfAnim.speed = 0.93f;
            gfAnim.SetTrigger("GO");
            Song.instance.isGameStartGf = false;
        }
    }
}
