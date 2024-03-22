using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetAchievment : MonoBehaviour
{
    public static GetAchievment instance { get; private set; }
    public Animator achiv;
    public Sprite[] achivSprites;
    public Image achivImage;
    public TMP_Text achivText;
    public AudioSource achivAudioSource;

    private Queue<int> achivQueue = new Queue<int>();
    private bool isPlaying = false;
    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void GetAchiv(int idAchiv)
    {
        if (!AchivSaves.instance.info.allAchievments[idAchiv].isEarned)
        {
            achivQueue.Enqueue(idAchiv);
            if (!isPlaying)
            {
                StartCoroutine(PlayAchivAnimations());
            }

        }
    }
    private IEnumerator PlayAchivAnimations()
    {
        isPlaying = true;
        while (achivQueue.Count > 0)
        {
            int idAchiv = achivQueue.Dequeue();
            achivImage.sprite = achivSprites[idAchiv];
            achivText.text = $"{AchivSaves.instance.info.allAchievments[idAchiv].names} \r\n \r\n {AchivSaves.instance.info.allAchievments[idAchiv].desc}";
            AchivSaves.instance.info.allAchievments[idAchiv].isEarned = true;
            achiv.SetTrigger("Achiv");
            AchivSaves.instance.Save();
            achivAudioSource.Play();

            yield return new WaitForSeconds(achiv.GetCurrentAnimatorStateInfo(0).length);
        }
        isPlaying = false;
    }
}
