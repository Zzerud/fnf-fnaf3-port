using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class AchivSaves : MonoBehaviour
{
    public static AchivSaves instance {  get; private set; }

    public ALlAchievments info = new ALlAchievments();
    public GameObject[] closedSprites;
    public TMP_Text text;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        if(File.Exists(Application.persistentDataPath + "/Achievements.json"))
        {
            LoadSave();
        }
        else
        {
            Save();
        }
        Debug.Log(Application.persistentDataPath + "/Achievements.json"); 
    }
    public void Save()
    {
        string data = JsonUtility.ToJson(info);
        string path = Application.persistentDataPath + "/Achievements.json";
        File.WriteAllText(path, data);
    }
    public void LoadSave()
    {
        string path = Application.persistentDataPath + "/Achievements.json";
        string data = File.ReadAllText(path);
        info = JsonUtility.FromJson<ALlAchievments>(data);

       


        DateTime now = DateTime.Now;
        Debug.Log(now.DayOfWeek);
        Debug.Log(now.Hour);
        if (now.DayOfWeek == DayOfWeek.Friday && now.Hour >= 18 && now.Hour <= 23)
        {
            GetAchievment.instance.GetAchiv(0);
            
        }

        for (int i = 0; i < closedSprites.Length; i++)
        {
            if (info.allAchievments[i].isEarned)
                closedSprites[i].SetActive(false);
        }
    }

    public void SelectNewAchiv(int idAchiv)
    {
        text.text = $"{info.allAchievments[idAchiv].names} \r\n {info.allAchievments[idAchiv].desc}";
    }
    public void DisableAchiv()
    {
        text.text = "";
    }
}
[Serializable]
public class ALlAchievments
{
    public List<AchivInfo> allAchievments = new List<AchivInfo>();
}
