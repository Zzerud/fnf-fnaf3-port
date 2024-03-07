using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreePlayCharactersPresets : MonoBehaviour
{
    public Image character;
    public RectTransform centerPoint;
    public ScrollElementContent[] scrollElements;
    
    private void Start()
    {
        // Disable component if we don't have elements
        if (scrollElements.Length == 0) enabled = false;
    }

    private void Update()
    {
        Sprite current = scrollElements[0].Sprite;
        float targetY = centerPoint.transform.position.y;

        for (int i = 0; i < scrollElements.Length; i++)
        {
            if (scrollElements[i].transform.position.y <= targetY)
            {
                current = scrollElements[i].Sprite;
                break;
            }
        }
        character.sprite = current;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float targetY = centerPoint.transform.position.y;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(centerPoint.transform.position, 12f);

        for (int i = 0; i < scrollElements.Length; i++)
        {
            if (scrollElements[i].transform.position.y <= targetY)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(scrollElements[i].transform.position, 12f);
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(scrollElements[i].transform.position, 12f);
            }
        }
    }
#endif
}
