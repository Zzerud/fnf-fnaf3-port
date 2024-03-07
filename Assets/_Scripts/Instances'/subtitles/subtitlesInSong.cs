using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subtitlesInSong : MonoBehaviour
{
    public string[] subtitlesText;
    public static subtitlesInSong instance { get; private set; }

    private void Start()
    {
        instance = this;
    }
}
