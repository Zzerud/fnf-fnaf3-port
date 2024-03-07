using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ButtonControllManager : MonoBehaviour
{
    public RectTransform[] btnsTransforms;
    public GameObject[] open_close;
    public RectTransform transMenu;
    public GameObject[] menuBtns, menuHits;

    [Header("Buttons")]
    [Space]
    public TMP_Text sizeBtnText;
    public Slider sizeBtnSlider;
    public TMP_Text alphaBtnText;
    public Slider alphaBtnSlider;
    public Image btnPreview;

    [Header("Hits")]
    [Space]
    public Image[] hits;
    public TMP_Text alphaHitText;
    public Slider alphaHitSlider;

    private bool isMenuOpen = false;
    private float toRotateBtn;
    private Vector2 toTrans, defTrans;


    private void Start()
    {
        toTrans = transMenu.anchoredPosition;
        defTrans = transMenu.anchoredPosition;

        if (PlayerPrefs.GetInt("Toggle") == 0)
        {
            foreach (GameObject j in menuHits) j.SetActive(true);
            foreach (GameObject t in menuBtns) t.SetActive(false);
            for (int i = 0; i < hits.Length; i++)
            {
                hits[i].CrossFadeAlpha(1, 0, false);
                hits[i].CrossFadeAlpha((PlayerPrefs.GetFloat("HitAlpha") / 100), 0, false);
            }
            alphaHitText.text = PlayerPrefs.GetFloat("HitAlpha").ToString("") + "%";
        }
        else
        {
            foreach (GameObject j in menuHits) j.SetActive(false);
            foreach (GameObject t in menuBtns) t.SetActive(true);
            btnPreview.CrossFadeAlpha(1, 0, false);
            btnPreview.CrossFadeAlpha((PlayerPrefs.GetFloat("BtnAlpha") / 100), 0, false);
            alphaBtnText.text = PlayerPrefs.GetFloat("BtnAlpha").ToString("") + "%";
            alphaBtnSlider.value = PlayerPrefs.GetFloat("BtnAlpha");

            sizeBtnSlider.value = PlayerPrefs.GetFloat("BtnSize");
            sizeBtnText.text = ((PlayerPrefs.GetFloat("BtnSize") / sizeBtnSlider.maxValue) * 100f).ToString("") + "%";
            for (int i = 0; i < btnsTransforms.Length; i++)
            {
                btnsTransforms[i].sizeDelta = new Vector2((PlayerPrefs.GetFloat("BtnSize") * 10), (PlayerPrefs.GetFloat("BtnSize") * 10));
            }
        }        
    }
    private void OnEnable()
    {
        Start();
    }
    private void Update()
    {
        transMenu.anchoredPosition = Vector2.MoveTowards(transMenu.anchoredPosition, toTrans, 7f);
    }

    public void OnExit()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerPrefs.SetFloat("posx" + i, btnsTransforms[i].anchoredPosition.x);
            PlayerPrefs.SetFloat("posy" + i, btnsTransforms[i].anchoredPosition.y);
        }
        gameObject.SetActive(false);
    }

    public void ButtonInteractable()
    {
        isMenuOpen = !isMenuOpen;
        if(isMenuOpen)
        {
            toTrans = new Vector2(43, transMenu.anchoredPosition.y);
            toRotateBtn = 180;
        }
        else
        {
            toTrans = new Vector2(defTrans.x, transMenu.anchoredPosition.y);
            toRotateBtn = 0;
        }
        for (int i = 0; i < open_close.Length; i++)
        {
            open_close[i].LeanRotate(new Vector3(0, 0, toRotateBtn), 0.5f);
        }
    }
    public void ChangeSizeButton(float value)
    {
        PlayerPrefs.SetFloat("BtnSize", value);
        PlayerPrefs.Save();

        sizeBtnText.text = ((value / sizeBtnSlider.maxValue) * 100f).ToString("") + "%";
        for (int i = 0; i < btnsTransforms.Length; i++)
        {
            btnsTransforms[i].sizeDelta = new Vector2((value * 10), (value * 10));
        }
    }
    public void ChangeAlphaButton(float value)
    {
        PlayerPrefs.SetFloat("BtnAlpha", value);
        PlayerPrefs.Save();

        btnPreview.CrossFadeAlpha((value / 100f), 0, false);
        alphaBtnText.text = value.ToString("") + "%";
    }
    public void ChangeAlphaHit(float value)
    {
        PlayerPrefs.SetFloat("HitAlpha", value);
        PlayerPrefs.Save();

        foreach (Image hit in hits) hit.CrossFadeAlpha((PlayerPrefs.GetFloat("HitAlpha") / 100), 0, false);

        alphaHitText.text = value.ToString("") + "%";
    }
}
