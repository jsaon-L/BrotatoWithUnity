using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class OpenPopupData : IReference
{

    public List<string> Texts;
    public List<UnityAction> ButtonActions;
    public UnityAction CloseAction;

    public bool AutoClose;
    public float AutoCloseDelay;

    public static OpenPopupData Create(UnityAction onClose,List<string> texts, List<UnityAction> buttonActions,bool autoClose,float autoCloseDelay)
    {
        // ʹ�����óؼ��g�������l���ȴ����
        OpenPopupData e = ReferencePool.Acquire<OpenPopupData>();
        //�����¼������������ ׃����Ҫ�xֵ �o�@�������Ӆ��� ���@���xֵ
        e.Texts = texts;
        e.ButtonActions = buttonActions;
        e.CloseAction = onClose;

        e.AutoClose = autoClose;
        e.AutoCloseDelay = autoCloseDelay;
        
        return e;
    }


    public void Clear()
    {
        Texts = null;
        ButtonActions = null;
        CloseAction = null;

        AutoClose = false;
        AutoCloseDelay = 0;
    }
}
