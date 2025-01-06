using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.UI;
//using UnityGameFramework.Runtime;
using StarForce;
using System;
using GameFramework;

public class UIStackComponent : UnityGameFramework.Runtime.GameFrameworkComponent
{
    [HideInInspector]
    public bool IgnoreGlobalBack = false;


    /// <summary>
    /// 横屏分辨率
    /// </summary>
    /// 
    [SerializeField]
    private Vector2 _HorizontalResolution = new Vector2(2001, 1125);
    /// <summary>
    /// 竖屏分辨率
    /// </summary>
    /// 
    [SerializeField]
    private Vector2 _VerticalResolution = new Vector2(1125, 2001);


    [SerializeField]
    private BuiltinDialogForm _builtinDialogForm;

    private SubUIFormManager subUIFormManager;

    public IUIForm CurrentPanel
    {
        get
        {
            return GameEntry.UI.GetUIGroup(UIGroupConfig.UIGroupName).CurrentUIForm;
        }
    }
    public int CurrentPanelSerialId
    {
        get
        {
            return GameEntry.UI.GetUIGroup(UIGroupConfig.UIGroupName).CurrentUIForm.SerialId;
        }
    }

    private void Start()
    {
        subUIFormManager  = new SubUIFormManager();
    }


    #region SubForm

    /// <summary>
    /// 注意返回的序列号id 是子页面的序列号id
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="parent"></param>
    /// <param name="subFormContainer"></param>
    /// <param name="userData"></param>
    /// <returns></returns>
    public int OpenSubForm(string assetName, Transform parent, SubFormContainer subFormContainer, object userData)
    {
        return subUIFormManager.OpenSubUIForm(assetName,parent, subFormContainer, userData);
    }
    public int OpenSubForm(string assetName, Transform parent, SubFormContainer subFormContainer)
    {
        return OpenSubForm(assetName, parent, subFormContainer, null);
    }

    public void CloseSubForm(int subSerialId, SubFormContainer subFormContainer, object userData)
    {
        subUIFormManager.CloseSubUIForm(subSerialId, subFormContainer, userData);
    }
    public void CloseSubForm(int subSerialId, SubFormContainer subFormContainer)
    {
        CloseSubForm(subSerialId, subFormContainer, null);
    }
    #endregion

    private void Update()
    {
        //TODO:UIStack Component
        //1. 两次返回才退出app,第一次返回弹出确认退出提示
        //2. 回退到只剩一个form最后不能再回退(也可以添加字段判断哪些form可以回退)
        //3. deeplink 使用链接跳转app特定页面或功能,或者复制口令跳转特定页面
        //4. uiform 子页面
        //5. ui显示安全区(将每个页面分成bg区域和安全区,内容都放到安全区下)
        //6. 默认UI loading状态(父类中默认实现一个有网络请求的form类)
        //7. 接管UI中网络请求(将请求与页面声明周期关联自动管理销毁)
        //8. ShowToastPop弹窗

        //全局返回按钮
        if (!IgnoreGlobalBack && Input.GetKeyDown(KeyCode.Escape))
        {
            var popupGroup = GameEntry.UI.GetUIGroup(UIGroupConfig.PopupUIGroupName);
            if (popupGroup.UIFormCount > 0)
            {
                GameEntry.UI.CloseUIForm(popupGroup.CurrentUIForm.SerialId);
            }
            else
            {
                Back();
            }
        }
    }


    /// <summary>
    /// 打開并顯示一個新窗口
    /// 老窗口會被覆蓋隱藏
    /// </summary>
    /// <param name="openForm"></param>
    /// <param name="userData"></param>
    public int OpenUIForm(string openForm, object userData = null)
    {
        return GameEntry.UI.OpenUIForm(openForm, UIGroupConfig.UIGroupName, userData);
    }

    /// <summary>
    /// 關閉當前窗口 并且顯示下面一個窗口
    /// </summary>
    public void CloseCurrentPanel()
    {
        GameEntry.UI.CloseUIForm(CurrentPanelSerialId);
    }

    /// <summary>
    /// 關閉當前窗口 并且顯示下面一個窗口
    /// </summary>
    public void Back()
    {
        var uiform = GameEntry.UI.GetUIForm(CurrentPanelSerialId);

        if (!uiform)
        {
            Debug.LogError("Current UI Form is null");
            return;
        }



        UGUIFormLogicBase logicBase = uiform.Logic as UGUIFormLogicBase;
        if (logicBase)
        {
            if (logicBase.CanBack)
            {
                GameEntry.UI.CloseUIForm(CurrentPanelSerialId);
            }
        }
        else
        {
            GameEntry.UI.CloseUIForm(CurrentPanelSerialId);
        }
    }



    /// <summary>
    /// 關閉當前窗口 并且打開一個新窗口顯示
    /// </summary>
    public void CloseAndOpen(string openForm, object openFormData = null,object closeFormData = null)
    {
        if (!string.IsNullOrEmpty(openForm))
        {
            GameEntry.UI.CloseUIForm(CurrentPanelSerialId, CloseUIFormParam.Create(closeFormData, false));
            GameEntry.UI.OpenUIForm(openForm, UIGroupConfig.UIGroupName, openFormData);
        }
        else
        {
            Debug.LogError("UIStack Open UI AssetName is Null!!!");
        }
    }

    /// <summary>
    /// 入棧一個窗口并且關閉這個窗口 Visible這個窗口
    /// 然后打開一個新頁面
    /// </summary>
    public void PushAndOpen(string openForm, object openFormData = null)
    {
        if (!string.IsNullOrEmpty(openForm))
        {
            GameEntry.UI.OpenUIForm(openForm, UIGroupConfig.UIGroupName, openFormData);
        }
        else
        {
            Debug.LogError("UIStack Open UI AssetName is Null!!!");
        }
    }




    /// <summary>
    /// 用来打开首页等几个Tap页面使用
    /// 如果想要打开的tap页存在 会关闭tap页上面的所有页面 然后激活tap页
    /// tap页不存在 此函数会关闭所有页面 然后打开tap页面
    /// </summary>
    /// <param name="openForm"></param>
    /// <param name="openFormData"></param>
    public void OpenTab(string openForm, object openFormData = null)
    {
        var uigroup = GameEntry.UI.GetUIGroup(UIGroupConfig.UIGroupName);

        if (uigroup.HasUIForm(openForm))
        {
            while (uigroup.CurrentUIForm.UIFormAssetName != openForm)
            {
                GameEntry.UI.CloseUIForm(uigroup.CurrentUIForm.SerialId);
            }
        }
        else
        {
            while (uigroup.CurrentUIForm != null)
            {
                GameEntry.UI.CloseUIForm(uigroup.CurrentUIForm.SerialId);
            }
            GameEntry.UI.OpenUIForm(openForm, UIGroupConfig.UIGroupName);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetName"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public int OpenDialog<T>(string assetName, DialogParams<T> param)
    {
        return GameEntry.UI.OpenUIForm(assetName, UIGroupConfig.PopupUIGroupName, param);

    }
    private int OpenDialog(DialogParams<object> dialogParams)
    {
        return GameEntry.UI.OpenUIForm("Assets/GameMain/UI/UIDialogs/DialogForm.prefab", UIGroupConfig.PopupUIGroupName, dialogParams);
    }

    /// <summary>
    /// 只有一个确认按钮
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="msg"></param>
    /// <param name="confirmText"></param>
    /// <param name="OnClickConfirm"></param>
    /// <param name="userData"></param>
    /// <param name="pauseGame"></param>
    /// <returns></returns>
    public int OpenDialogWithOneBtn(string Title,string msg,string confirmText, GameFrameworkAction<DialogResule<object>> OnClickConfirm,object userData,bool pauseGame = false)
    {
        DialogParams<object> dialogParams = new DialogParams<object>();
        dialogParams.Mode = 1;
        dialogParams.Title = Title;
        dialogParams.Message = msg;
        dialogParams.ConfirmText = confirmText;
        dialogParams.OnClickConfirm = OnClickConfirm;
        dialogParams.PauseGame = pauseGame;
        dialogParams.UserData = userData;

        return OpenDialog(dialogParams);
    }
    /// <summary>
    /// 两个按钮 一个确认按钮一个取消按钮
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="msg"></param>
    /// <param name="confirmText"></param>
    /// <param name="cancelText"></param>
    /// <param name="onClickConfirm"></param>
    /// <param name="onClickCancel"></param>
    /// <param name="userData"></param>
    /// <param name="pauseGame"></param>
    /// <returns></returns>
    public int OpenDialogWithTwoBtn(string Title, string msg, string confirmText, string cancelText, GameFrameworkAction<object> onClickConfirm, GameFrameworkAction<object> onClickCancel,object userData, bool pauseGame = false)
    {
        DialogParams<object> dialogParams = new DialogParams<object>();
        dialogParams.Mode = 2;
        dialogParams.Title = Title;
        dialogParams.Message = msg;
        dialogParams.ConfirmText = confirmText;
        dialogParams.CancelText = cancelText;
        dialogParams.OnClickConfirm = onClickConfirm;
        dialogParams.OnClickCancel = onClickCancel;
        dialogParams.PauseGame = pauseGame;
        dialogParams.UserData = userData;

        return OpenDialog(dialogParams);
    }

    public void ClosePopup(int serialId, object userData)
    {
        GameEntry.UI.CloseUIForm(serialId, userData);
    }


    ///// <summary>
    ///// 系統提示框
    ///// </summary>
    ///// <param name="msg">內容</param>
    //public void ShowToastPop(string msg)
    //{
    //    var pop = Doozy.Engine.UI.UIPopup.GetPopup("Toast");
    //    pop.Data.SetLabelsTexts(msg);
    //    pop.Show();
    //}

    ///// <summary>
    ///// 系統提示框 msg 會先進行多語言轉換再顯示
    ///// </summary>
    ///// <param name="msg"></param>
    //public void ShowToastPopWidthLocalText(string msg)
    //{
    //    var localMsg = GameEntry.GetLocalText(msg);
    //    ShowToastPop(localMsg);
    //}



    /// <summary>
    /// 設置橫屏
    /// </summary>
    public void HorizontalScreen()
    {
        if (Screen.orientation != ScreenOrientation.LandscapeLeft)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            GameEntry.UI.RootCanvasScaler.referenceResolution = _HorizontalResolution;
        }
    }
    /// <summary>
    /// 設置豎屏
    /// </summary>
    public void VerticalScreen()
    {
        if (Screen.orientation != ScreenOrientation.Portrait)
        {
            Screen.orientation = ScreenOrientation.Portrait;
            GameEntry.UI.RootCanvasScaler.referenceResolution = _VerticalResolution;
        }
    }

    /// <summary>
    /// 当GF资源未初始化时使用此对话框
    /// </summary>
    public void OpenBuiltinDialog(DialogParams<object> dialogParams)
    {
        var dialog = Instantiate(_builtinDialogForm);
        dialog.SetUserData(dialogParams);
    }

}


