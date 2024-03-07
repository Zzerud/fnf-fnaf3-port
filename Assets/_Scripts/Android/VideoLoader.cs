using UnityEngine;
using System.IO;

public class VideoLoader : MonoBehaviour
{
    public string videoFileName = "Nonsensica.mp4";

    private void Start()
    {
        // Путь к файлу в StreamingAssets
        string sourcePath = Path.Combine(Application.streamingAssetsPath, videoFileName);

        // Путь, куда мы скопируем файл
        string destinationPath = Path.Combine(Application.persistentDataPath, videoFileName);

        // Если файл не существует в persistentDataPath, скопируем его туда
        if (!File.Exists(destinationPath))
        {
            byte[] videoBytes;

            // Зависит от платформы, как загрузить файл (для WebGL используем UnityWebRequest)
#if UNITY_WEBGL && !UNITY_EDITOR
                UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(sourcePath);
                www.SendWebRequest();
                while (!www.isDone) { }
                videoBytes = www.downloadHandler.data;
#else
            videoBytes = File.ReadAllBytes(sourcePath);
#endif

            File.WriteAllBytes(destinationPath, videoBytes);
        }

        // Теперь используем destinationPath для воспроизведения видео
        //YourPlugin.PlayVideo(destinationPath);
    }
}
