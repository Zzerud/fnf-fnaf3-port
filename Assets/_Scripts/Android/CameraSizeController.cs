using UnityEngine;

public class CameraSizeController : MonoBehaviour
{
    public Camera mainCamera;
    public float defaultSize = 6.4f;
    public float minSize = 4f; // ћинимальный размер видимости камеры
    public float maxSize = 8f; // ћаксимальный размер видимости камеры

    void Start()
    {
        // ”станавливаем изначальный размер видимости камеры
        mainCamera.orthographicSize = defaultSize;
        AdjustCameraSize();
    }

    void Update()
    {
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        // ѕолучаем текущее разрешение экрана
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        // ¬ычисл€ем пропорциональный размер видимости камеры в зависимости от текущего разрешени€ экрана
        float proportionalSize = defaultSize * (screenSize.y / screenSize.x);

        // ќграничиваем размер видимости камеры в диапазоне от minSize до maxSize
        float clampedSize = Mathf.Clamp(proportionalSize, minSize, maxSize);

        // »змен€ем размер видимости камеры
        mainCamera.orthographicSize = clampedSize;
    }
}
