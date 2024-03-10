using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitP : MonoBehaviour
{
    public Image[] pi;
    private void Start()
    {
        pi[0].GetComponent<Image>().color = new Color(ColorPicker.left.r, ColorPicker.left.g, ColorPicker.left.b, .2f);
        pi[1].GetComponent<Image>().color = new Color(ColorPicker.down.r, ColorPicker.down.g, ColorPicker.down.b, .2f);
        pi[2].GetComponent<Image>().color = new Color(ColorPicker.up.r, ColorPicker.up.g, ColorPicker.up.b, .2f);
        pi[3].GetComponent<Image>().color = new Color(ColorPicker.right.r, ColorPicker.right.g, ColorPicker.right.b, .2f);
    }
}
