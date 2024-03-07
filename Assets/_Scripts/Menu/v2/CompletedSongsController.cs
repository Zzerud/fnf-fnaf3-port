using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedSongsController : MonoBehaviour
{
    public WeekSong[] songs;
    public GameObject[] songsLock;
    private void Start()
    {
        for (int i = 0; i < songs.Length; i++)
        {
            if (PlayerPrefs.HasKey("Complete-" + songs[i].songName))
            {
                if (PlayerPrefs.GetInt("Complete-" + songs[i].songName) >= 1)
                {
                    songsLock[i].SetActive(false);
                }
                else
                {
                    songsLock[i].SetActive(true);
                }
            }
            else
            {
                songsLock[i].SetActive(true);
            }
        }
    }
}
