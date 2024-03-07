using UnityEngine;

public class ObjectSizeController : MonoBehaviour
{
    public Camera mainCamera;
    public Transform objectTransform;
    public float defaultWidth = 12.8f; // Ширина объекта по умолчанию
    public float defaultHeight = 7.2f; // Высота объекта по умолчанию
    public float minWidth = 8f; // Минимальная ширина объекта
    public float minHeight = 4.5f; // Минимальная высота объекта
    public float maxWidth = 16f; // Максимальная ширина объекта
    public float maxHeight = 9f; // Максимальная высота объекта

    private float defaultAspect;

    void Start()
    {
        defaultAspect = defaultWidth / defaultHeight;
        AdjustObjectSize();
    }

    void Update()
    {
        AdjustObjectSize();
    }

    void AdjustObjectSize()
    {
        // Получаем текущее разрешение экрана
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Вычисляем пропорциональный размер объекта в зависимости от текущего разрешения экрана
        float proportionalWidth = defaultWidth;
        float proportionalHeight = defaultHeight;

        float screenAspect = screenWidth / screenHeight;

        if (screenAspect > defaultAspect)
        {
            // Уменьшаем ширину объекта, чтобы поддерживать пропорции
            proportionalWidth = defaultHeight * screenAspect;
        }
        else if (screenAspect < defaultAspect)
        {
            // Уменьшаем высоту объекта, чтобы поддерживать пропорции
            proportionalHeight = defaultWidth / screenAspect;
        }

        // Ограничиваем размер объекта в диапазоне от minWidth до maxWidth и от minHeight до maxHeight
        float clampedWidth = Mathf.Clamp(proportionalWidth, minWidth, maxWidth);
        float clampedHeight = Mathf.Clamp(proportionalHeight, minHeight, maxHeight);

        // Изменяем размер и масштаб объекта
        float scale = clampedWidth / defaultWidth;
        objectTransform.localScale = new Vector3(scale, scale, scale);
        mainCamera.orthographicSize = clampedHeight / 2f;
    }
}
