using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class parallax : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool scrollLeft;
    float width;

    private void Start()
    {
        SetupTexture();
        if(scrollLeft) speed =-speed;
    }
    private void SetupTexture()
    {
        Sprite sprite = GetComponent<Image>().sprite;
        width = sprite.texture.width / sprite.pixelsPerUnit;
    }

    private void Scroll()
    {
        float delta = speed * Time.deltaTime;
        transform.localPosition += new Vector3(delta, 0, 0);
    }

    private void CheckReset()
    {
        if((Mathf.Abs(transform.localPosition.x) - width) > 1000)
        {
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }
    }

    private void Update()
    {
        Scroll();
        CheckReset();
    }

}
