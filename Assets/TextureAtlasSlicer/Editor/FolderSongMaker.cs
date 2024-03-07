using UnityEditor;
using UnityEngine;
using System.IO;

public class FolderSongMaker : EditorWindow
{
    public Texture logo;
    [MenuItem("UNITY PARTY ENGINE/Create Song Folder")]
    public static void ShowWindow()
    {
        GetWindow<FolderSongMaker>("Create Folders Song");
    }
    private string[] folderNames = { "Animations", "Dad", "BG", "Characters", "Characters_Info", "Jsons", "Song", "_CharactersData" };

    void OnGUI()
    {
        GUILayout.Box(logo, GUILayout.Width(400), GUILayout.Height(170));
        if (GUILayout.Button("Create Folders"))
        {
            CreateNewFolders();
        }
    }
    void CreateNewFolders()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        for (int i = 0; i < folderNames.Length; i++)
        {
            if (!AssetDatabase.IsValidFolder(path + "/" + folderNames[i]))
            {
                if (folderNames[i] == "Dad")
                {
                    string path2 = AssetDatabase.GetAssetPath(Selection.activeObject) + "/Animations";
                    AssetDatabase.CreateFolder(path2, folderNames[i]);
                }
                else if (folderNames[i] == "_CharactersData")
                {
                    string path3 = AssetDatabase.GetAssetPath(Selection.activeObject) + "/Characters_Info";
                    AssetDatabase.CreateFolder(path3, folderNames[i]);
                }
                else
                {
                    AssetDatabase.CreateFolder(path, folderNames[i]);
                    if (folderNames[i] == "Song")
                    {
                        CopyFilesFromSourceToDestination("Assets/__MOD ASSETS/_FNAF3/Song/" + Selection.activeObject.name, path + "/Song");
                    }
                }
            }
        }

        MoveJsons(path, path + "/Jsons");
        AssetDatabase.Refresh();
    }

    void CopyFilesFromSourceToDestination(string sourcePath, string destinationPath)
    {
        if (!Directory.Exists(sourcePath))
        {
            Debug.LogError("Source path does not exist: " + sourcePath);
            return;
        }

        string[] files = Directory.GetFiles(sourcePath);

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            FileUtil.CopyFileOrDirectory(file, destinationPath + "/" + fileName);
        }
        AssetDatabase.DeleteAsset(sourcePath);
    }
    void MoveJsons(string sourcePath, string destinationPath)
    {
        if (!Directory.Exists(sourcePath))
        {
            Debug.LogError("Source path does not exist: " + sourcePath);
            return;
        }

        string[] files = Directory.GetFiles(sourcePath);

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string extension = Path.GetExtension(file);
            if (extension == ".json" || extension == ".lua")
            {
                FileUtil.MoveFileOrDirectory(file, destinationPath + "/" + fileName);
            }
        }
    }
}
