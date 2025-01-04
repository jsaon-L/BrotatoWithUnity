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
        // 使用引用池技術，避免頻繁內存分配
        OpenPopupData e = ReferencePool.Acquire<OpenPopupData>();
        //生成事件實例后如果有 變量需要賦值 給這個函數加參數 在這里賦值
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
