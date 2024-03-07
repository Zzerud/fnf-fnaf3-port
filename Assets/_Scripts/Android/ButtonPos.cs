using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPos : MonoBehaviour
{
    private RectTransform rect;
    public int id;
    public Vector2 defPos;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        if(PlayerPrefs.HasKey("posx" + id) && PlayerPrefs.HasKey("posy" + id))
        {
            rect.anchoredPosition = new Vector2(PlayerPrefs.GetFloat("posx" + id), PlayerPrefs.GetFloat("posy" + id));
        }
        else
        {
            rect.anchoredPosition = defPos;
        }

        rect.sizeDelta = new Vector2(PlayerPrefs.GetFloat("BtnSize") * 10, PlayerPrefs.GetFloat("BtnSize") * 10);
    }
}
