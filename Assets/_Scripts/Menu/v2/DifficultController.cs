using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultController : MonoBehaviour
{
    public Image normalImg, easyImg;
    public Sprite normalInactive, normalActive, easyInactive, easyActive;
    public Sprite normalInactiveRu, normalActiveRu, easyInactiveRu, easyActiveRu;

    private void Start()
    {
        if (PlayerPrefs.HasKey("difficult"))
        {
            if(PlayerPrefs.GetString("difficult") == "easy")
            {
                normalImg.sprite = Application.systemLanguage == SystemLanguage.Russian ? normalInactiveRu : normalInactive;
                easyImg.sprite = Application.systemLanguage == SystemLanguage.Russian ? easyActiveRu : easyActive;
            }
            else if (PlayerPrefs.GetString("difficult") == "normal")
            {
                normalImg.sprite = Application.systemLanguage == SystemLanguage.Russian ? normalActiveRu : normalActive;
                easyImg.sprite = Application.systemLanguage == SystemLanguage.Russian ? easyInactiveRu : easyInactive;
            }
        }
        else
        {
            PlayerPrefs.SetString("difficult", "normal");
            PlayerPrefs.Save();
            normalImg.sprite = Application.systemLanguage == SystemLanguage.Russian ? normalActiveRu : normalActive;
            easyImg.sprite = Application.systemLanguage == SystemLanguage.Russian ? easyInactiveRu : easyInactive;
        }
    }
    public void ChangeDiff(string diff)
    {
        PlayerPrefs.SetString("difficult", diff);
        PlayerPrefs.Save();
        if (diff == "easy")
        {
            normalImg.sprite = Application.systemLanguage == SystemLanguage.Russian ? normalInactiveRu : normalInactive;
            easyImg.sprite = Application.systemLanguage == SystemLanguage.Russian ? easyActiveRu : easyActive;
        }
        else if (diff == "normal")
        {
            normalImg.sprite = Application.systemLanguage == SystemLanguage.Russian ? normalActiveRu : normalActive;
            easyImg.sprite = Application.systemLanguage == SystemLanguage.Russian ? easyInactiveRu : easyInactive;
        }
    }
    public void StartWeek(string diff)
    {
        PlayerPrefs.SetString("difficult", diff);
        PlayerPrefs.Save();

    }
}
