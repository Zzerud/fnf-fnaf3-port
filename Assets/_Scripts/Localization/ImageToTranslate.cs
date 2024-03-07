using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageToTranslate : MonoBehaviour
{
    private Image img;
    public Sprite rusVersion, engVersion;
    [Header("RUS")]
    public SpriteState stateRus;
    [Header("ENG")]
    public SpriteState stateEng;

    private Button btn;
    private void Start()
    {
        img = GetComponent<Image>();
        img.sprite = Application.systemLanguage == SystemLanguage.Russian ? rusVersion : engVersion;
        btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.spriteState = Application.systemLanguage == SystemLanguage.Russian ? stateRus : stateEng;
        }
    }
}
