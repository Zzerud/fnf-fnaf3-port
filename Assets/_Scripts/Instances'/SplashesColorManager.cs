using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashesColorManager : MonoBehaviour
{
    public SpriteRenderer[] splashes;

    private void Start()
    {
        splashes[0].color = new Color(ColorPicker.down.r, ColorPicker.down.g, ColorPicker.down.b);
        splashes[1].color = new Color(ColorPicker.left.r, ColorPicker.left.g, ColorPicker.left.b);
        splashes[2].color = new Color(ColorPicker.right.r, ColorPicker.right.g, ColorPicker.right.b);
        splashes[3].color = new Color(ColorPicker.up.r, ColorPicker.up.g, ColorPicker.up.b);
    }
}
