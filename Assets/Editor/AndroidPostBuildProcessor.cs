using System.IO;
using UnityEditor.Android;
using UnityEngine;

public class AndroidPostBuildProcessor : IPostGenerateGradleAndroidProject
{
    public int callbackOrder
    {
        get
        {
            return 999;
        }
    }

    void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path)
    {
        Debug.Log("Bulid path : " + path);

        string gradlePropertiesFile = path + "/build.gradle";
        string content = File.ReadAllText(gradlePropertiesFile);
        if (!content.Contains("configurations.implementation"))
        {
            content += "\r\nconfigurations.implementation {\r\n    exclude(group : \"com.android.support\")\r\n\texclude(group : \"com.google.android.gms\")\r\n}";
            File.WriteAllText(gradlePropertiesFile, content);
        }
    }
}