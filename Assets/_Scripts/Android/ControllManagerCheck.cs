using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ControllManagerCheck : MonoBehaviour
{
    ToggleGroup toggleGroupInstance;
    public Toggle hit;
    public Toggle cureentSelection {
        get { return toggleGroupInstance.ActiveToggles().FirstOrDefault(); } 
    }
    private void Start()
    {
        toggleGroupInstance = GetComponent<ToggleGroup>();
        if (PlayerPrefs.GetInt("Toggle") == 1)
        {
            SelectToggle(1);
        }
        else
        {
            SelectToggle(0);
        }
            Debug.Log("First Selected" + PlayerPrefs.GetInt("Toggle"));
    }
    public void Check()
    {
        if (hit.isOn)
        {
            PlayerPrefs.SetInt("Toggle", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Toggle", 1);

        }
        Debug.Log(PlayerPrefs.GetInt("Toggle") + "dd");
    }
    public void SelectToggle(int id)
    {
        var toggles = toggleGroupInstance.GetComponentsInChildren<Toggle>();
        
        toggles[id].isOn = true;
    }
    public void ClickSettings()
    {
        SceneManager.LoadScene(10);
    }

}
