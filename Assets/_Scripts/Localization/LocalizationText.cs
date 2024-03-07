using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationText : MonoBehaviour
{
    public string rusSubtitles, engSubtitles;

    private void Start()
    {
        GetComponent<TMP_Text>().text = Application.systemLanguage == SystemLanguage.Russian ? rusSubtitles : engSubtitles;
    }
}
