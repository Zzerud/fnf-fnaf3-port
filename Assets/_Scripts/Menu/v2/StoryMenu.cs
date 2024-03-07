using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryMenu : MonoBehaviour
{
    public Sprite nonsense, nonsensica;
    public Image img, img2;
    private int a = 0;
    public Button start;
    public Week nonsen, nonsensi;

    private void Start()
    {
        start.onClick.AddListener(StartWeek);
    }

    public void NextWeek()
    {
        if(a == 0)
        {
            a++;
            img.sprite = nonsensica;
            img2.sprite = nonsense;
        }
        else
        {
            a = 0;
            img.sprite = nonsense;
            img2.sprite = nonsensica;
        }
    }
    public void StartWeek()
    {
        if(a == 0)
        {
            MenuV2.Instance.LaunchSongScreen(nonsen);
        }
        else
        {
            MenuV2.Instance.LaunchSongScreen(nonsensi);
        }
        
    }
}
