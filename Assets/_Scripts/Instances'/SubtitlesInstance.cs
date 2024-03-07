using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitlesInstance : MonoBehaviour
{
    public static SubtitlesInstance instance { get; private set; }
    public TMP_Text txt;

    private void Start()
    {
        instance = this;
    }
}
