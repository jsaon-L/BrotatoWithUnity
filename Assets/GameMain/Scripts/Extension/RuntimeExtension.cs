using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public static class RuntimeExtension
{

    public static T AddComponent<T>(this UnityObject uo) where T : Component
    {
        if (uo is GameObject)
        {
            return ((GameObject)uo).AddComponent<T>();
        }
        else if (uo is Component)
        {
            return ((Component)uo).gameObject.AddComponent<T>();
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    public static T GetOrAddComponent<T>(this UnityObject uo) where T : Component
    {
        return uo.GetComponent<T>() ?? uo.AddComponent<T>();
    }

    public static T GetComponent<T>(this UnityObject uo)
    {
        if (uo is GameObject)
        {
            return ((GameObject)uo).GetComponent<T>();
        }
        else if (uo is Component)
        {
            return ((Component)uo).GetComponent<T>();
        }
        else
        {
            throw new NotSupportedException();
        }
    }

}
