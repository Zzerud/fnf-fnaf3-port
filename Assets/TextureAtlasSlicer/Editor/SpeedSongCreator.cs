using UnityEditor;
using UnityEngine;

public class SpeedSongCreator : EditorWindow
{
    public Texture logo;
    [MenuItem("UNITY PARTY ENGINE/Create Song")]
    public static void ShowWindow()
    {
        GetWindow<SpeedSongCreator>("Create Song");
    }

    private string songName;
    private string[] currentNameMod;

    void OnGUI()
    {
        GUILayout.Box(logo, GUILayout.Width(400), GUILayout.Height(170));
        EditorGUILayout.LabelField("Song name:");
        songName = EditorGUILayout.TextArea(songName);

        if (GUILayout.Button("Create Song"))
        {
            StartMakingSong();
        }
        Repaint();
    }

    public void StartMakingSong()
    {
        currentNameMod = AssetDatabase.GetSubFolders("Assets/__MOD ASSETS");
        string pathToSong;
        if (currentNameMod.Length > 0)
        {
            pathToSong = currentNameMod[0] + "/Data/" + songName;

            if (!AssetDatabase.IsValidFolder(pathToSong))
                pathToSong = currentNameMod[0] + "/Data/Later/" + songName;

            WeekSong song = ScriptableObject.CreateInstance<WeekSong>();

            song.songName = songName;
            song.sceneName = songName;

            string pathToCharts =$"{pathToSong}/Jsons/";
            song.chart = AssetDatabase.LoadAssetAtPath<TextAsset>(pathToCharts + songName + ".Json");
            song.chartEasy = AssetDatabase.LoadAssetAtPath<TextAsset>(pathToCharts + songName + "-easy" + ".Json");

            string pathToSongInstVocal = $"{pathToSong}/Song/";
            song.instrumentals = AssetDatabase.LoadAssetAtPath<AudioClip>(pathToSongInstVocal + "/Inst.ogg");
            song.vocals = AssetDatabase.LoadAssetAtPath<AudioClip>(pathToSongInstVocal + "/Voices.ogg");

            //Delete if no sprites
            string pathToImageInPause = "Assets/__MOD ASSETS/MENU/pauseMenu/songs/";
            song.imageInPause = AssetDatabase.LoadAssetAtPath<Sprite>(pathToImageInPause + songName + ".png");

            string pathToCreateSong = currentNameMod[0] + "/_Asset Song";
            string[] guids = AssetDatabase.FindAssets("t:WeekSong", new string[] { pathToCreateSong });
            int i = guids.Length;
            song.songIndex = i;

            song.songColor = new Color(1, 1, 1, 1);
            song.additionalSongColor = new Color(1, 1, 1, 1);

            AssetDatabase.CreateAsset(song, $"{pathToCreateSong}/{songName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }

    }


}
