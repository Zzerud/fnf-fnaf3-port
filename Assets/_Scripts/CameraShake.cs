using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public Transform cameraTransform;
    private Vector3 originalCameraPosition;
    public Image flashWhite;
    public Image flashBlack;
    public Graphic whiteUi;
    public Graphic purple;
    public Graphic waffleGlow;
    public Image hallucinations;
    public Sprite[] hallucinationsImg;
    public string[] hallucinationsName;

    private float shakeIntensity = 0.3f;
    private float shakeDuration = 0.5f;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
        instance = this;
        flashWhite.CrossFadeAlpha(0, 0, false);
        flashBlack.CrossFadeAlpha(0,0, false);
        whiteUi.CrossFadeAlpha(0, 0, false);
        purple.CrossFadeAlpha(0, 0, false);
        waffleGlow.CrossFadeAlpha(0, 0, false);
        hallucinations.enabled = false;
        if (cameraTransform == null)
        {
            cameraTransform = GetComponent<Transform>();
        }
    }
    private void OnEnable()
    {
        flashWhite.CrossFadeAlpha(0, 0, false);
        flashBlack.CrossFadeAlpha(0, 0, false);
    }

    public void StartShake(float intensity, float duration)
    {
        shakeIntensity = intensity;
        shakeDuration = duration;
        originalCameraPosition = cameraTransform.localPosition;
        StartCoroutine(Shake());
    }
    public void Flash(string time)
    {
        float s = float.Parse(time, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
        long mil = (long)s * 1000;
        //Vibration.Vibrate(mil);
        flashWhite.CrossFadeAlpha(1, 0, false);
        flashWhite.CrossFadeAlpha(0, s, false);
    }
    public void FlashBlack(string time)
    {
        float s = float.Parse(time, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
        long mil = (long)s * 1000;
        flashBlack.CrossFadeAlpha(1, 0, false);
        flashBlack.CrossFadeAlpha(0, s, false);
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0;

        while (elapsedTime < shakeDuration)
        {
            CameraMovement.instance.enableMovement = false;
            Vector3 randomPoint = originalCameraPosition + Random.insideUnitSphere * shakeIntensity;

            // Применяем дрожание к позиции камеры
            cameraTransform.localPosition = randomPoint;

            // Увеличиваем прошедшее время
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Возвращаем камеру в исходное положение
        cameraTransform.localPosition = originalCameraPosition;
        CameraMovement.instance.enableMovement = true;
    }

    public void ChangeHallucinations(string Name, string isOn)
    {
        if(isOn == "on")
        {
            hallucinations.enabled = true;

            for (int i = 0; i < hallucinationsName.Length; i++)
            {
                if (Name == hallucinationsName[i])
                {
                    hallucinations.sprite = hallucinationsImg[i];
                }
            }
        }
        else
        {
            hallucinations.enabled = false;
        }
        
    }
}
