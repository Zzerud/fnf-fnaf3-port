using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AndroidControllByArrow : MonoBehaviour, IDragHandler
{
    public int idButton; //left, down, up, right
    private RectTransform rect;
    public Vector2 defaultPosition;
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        
    }
    private void Update()
    {
        if (transform.localPosition.x <= -555) transform.localPosition = new Vector2(-555, transform.localPosition.y);
        else if (transform.localPosition.x >= 555) transform.localPosition = new Vector2(555, transform.localPosition.y);
        if (transform.localPosition.y <= -275) transform.localPosition = new Vector2(transform.localPosition.x, -275);
        else if (transform.localPosition.y >= 275) transform.localPosition = new Vector2(transform.localPosition.x, 275);
    }
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (PlayerPrefs.HasKey("posx" + idButton))
        {
            rect.anchoredPosition = new Vector2(PlayerPrefs.GetFloat("posx" + idButton), PlayerPrefs.GetFloat("posy" + idButton));
        }
        else
        {
            rect.anchoredPosition = defaultPosition;
        }
    }
}
