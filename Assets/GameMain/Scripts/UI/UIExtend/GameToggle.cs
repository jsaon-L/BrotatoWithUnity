using System;
using GameFramework;
using Skyunion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class GameToggle : Toggle
{
    [HideInInspector] [SerializeField] public bool AutoPlaySound = true;
    [HideInInspector] [SerializeField] public string SoundAssetName = "Sound_Ui_CommonSidePage";
    private Action clickCallBack;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (AutoPlaySound)
        {
            GameEntry.Sound.PlaySound(SoundAssetName,"UI");
        }
        base.OnPointerClick(eventData);
        if (clickCallBack != null)
        {
            clickCallBack.Invoke();
        }
    }

    public void AddListener(Action callBack)
    {
        clickCallBack += callBack;
    }

    public void RemoveListener(Action callBack)
    {
        clickCallBack -= callBack;
    }

    public void RemoveAllClickListener()
    {
        clickCallBack = null;
    }

    public void SetSound(string soundName)
    {
        SoundAssetName = soundName;
    }

    public void SetMute()
    {
        this.AutoPlaySound = false;
    }
}