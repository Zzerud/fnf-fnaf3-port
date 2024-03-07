using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitP : MonoBehaviour
{
    public Image[] pi;
    private void Start()
    {
        pi[0].GetComponent<Image>().color = new Color(ColorPicker.left.r, ColorPicker.left.g, ColorPicker.left.b, (PlayerPrefs.GetFloat("HitAlpha") / 100));
        pi[1].GetComponent<Image>().color = new Color(ColorPicker.down.r, ColorPicker.down.g, ColorPicker.down.b, (PlayerPrefs.GetFloat("HitAlpha") / 100));
        pi[2].GetComponent<Image>().color = new Color(ColorPicker.up.r, ColorPicker.up.g, ColorPicker.up.b, (PlayerPrefs.GetFloat("HitAlpha") / 100));
        pi[3].GetComponent<Image>().color = new Color(ColorPicker.right.r, ColorPicker.right.g, ColorPicker.right.b, (PlayerPrefs.GetFloat("HitAlpha") / 100));
    }
}
