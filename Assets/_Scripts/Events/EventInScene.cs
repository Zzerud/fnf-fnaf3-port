using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EventInScene : MonoBehaviour
{
    public static EventInScene instance;
    public MonoBehaviour targetScript;
    private void Start()
    {
        if(instance == null) instance = this;
    }
    public void FindVoid(string methodName)
    {
        targetScript.Invoke(methodName, 0);
    }




    public void FindVoid(string methodName, object parameter1, object parameter2)
    {

        System.Type type = targetScript.GetType();
        System.Reflection.MethodInfo method = type.GetMethod(methodName);

        if (method == null)
        {
            Debug.LogError("Method " + methodName + " not found in " + targetScript.ToString());
            return;
        }

        method.Invoke(targetScript, new object[] { parameter1, parameter2 });
    }

    public void FindVoid(string methodName, object parameter1)
    {
        System.Type type = targetScript.GetType();
        System.Reflection.MethodInfo method = type.GetMethod(methodName);

        if (method == null)
        {
            Debug.LogError("Method " + methodName + " not found in " + targetScript.ToString());
            return;
        }

        method.Invoke(targetScript, new object[] { parameter1 });
    }

}
