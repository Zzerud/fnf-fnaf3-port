using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupInstances : MonoBehaviour
{
    public static PopupInstances instance;
    public string[] popups;
    private RectTransform rect;
    private AudioSource source;
    public AudioClip pop;

    private Vector2 minPos, maxPos;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        source = GetComponent<AudioSource>();
        if(instance == null) instance = this;
        minPos = rect.rect.min;
        maxPos = rect.rect.max;
    }

    public IEnumerator Popups(int spam, int x = 0, int y = 0, bool isPlayingInGame = false)
    {
        int totalSpawned = 0;
        while (totalSpawned < spam)
        {

            if(isPlayingInGame)
            {
                yield return new WaitForSeconds(Random.Range(5, 7));
            }

            GameObject prefab = Resources.Load<GameObject>(popups[Random.Range(0, popups.Length)]);
            GameObject done = Instantiate(prefab, transform);

            if(x > 0)
                done.transform.localPosition = new Vector2(x,y);
            else
                done.transform.localPosition = new Vector2(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y));

            totalSpawned++;
            source.PlayOneShot(pop);
            if (isPlayingInGame)
            {
                yield return new WaitForSeconds(3);
                Destroy(done);
            }
            else
            {
                yield return new WaitForSeconds(0.03f);
            }
        }
    }
}
