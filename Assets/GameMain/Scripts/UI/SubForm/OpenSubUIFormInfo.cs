using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OpenSubUIFormInfo : IReference
{
    private int m_SerialId;
    private object m_UserData;

    public Transform Parent;
    public SubFormContainer SubFormContainer;

    public int SerialId
    {
        get
        {
            return m_SerialId;
        }
    }


    public object UserData
    {
        get
        {
            return m_UserData;
        }
    }

    public static OpenSubUIFormInfo Create(int serialId, object userData, Transform parent, SubFormContainer subFormContainer)
    {
        OpenSubUIFormInfo openUIFormInfo = ReferencePool.Acquire<OpenSubUIFormInfo>();
        openUIFormInfo.m_SerialId = serialId;
        openUIFormInfo.m_UserData = userData;
        openUIFormInfo.Parent = parent;
        openUIFormInfo.SubFormContainer = subFormContainer;
        return openUIFormInfo;
    }

    public void Clear()
    {
        m_SerialId = 0;
        m_UserData = null;
        Parent = null;
        SubFormContainer = null;
    }
}

