using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EventSystemMain : MonoBehaviour
{
    public static EventSystemMain instance;
    public Events ev;


    private void Start()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void FindVoid(string methodName)
    {
        EventInScene.instance.FindVoid(methodName);
        /*for (int i = 0; i < ev.AllEvents.Length; i++)
        {
            var scriptInfo = ev.AllEvents[i].GetClass();
            Debug.Log(scriptInfo);
            MethodInfo method = scriptInfo.GetMethod(methodName);
            if (method != null)
            {
                object instanceObject = Activator.CreateInstance(scriptInfo);

                method.Invoke(instanceObject, null);
            }
            else
            {
                
            }
            
            
        }*/
    }
    public void FindVoidWithParametrs(string methodName, object[] parametrs)
    {
        
        /*for (int i = 0; i < ev.AllEvents.Length; i++)
        {
            var scriptInfo = ev.AllEvents[i].GetClass();
            MethodInfo method = scriptInfo.GetMethod(methodName);
            if (method != null)
            {
                object instanceObject = Activator.CreateInstance(scriptInfo);

                method.Invoke(instanceObject, parametrs);
            }
        }*/
    }
}
