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
    /// �����ֱ���
    /// </summary>
    /// 
    [SerializeField]
    private Vector2 _HorizontalResolution = new Vector2(2001, 1125);
    /// <summary>
    /// �����ֱ���
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
    /// ע�ⷵ�ص����к�id ����ҳ������к�id
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
        //1. ���η��ز��˳�app,��һ�η��ص���ȷ���˳���ʾ
        //2. ���˵�ֻʣһ��form������ٻ���(Ҳ��������ֶ��ж���Щform���Ի���)
        //3. deeplink ʹ��������תapp�ض�ҳ�����,���߸��ƿ�����ת�ض�ҳ��
        //4. uiform ��ҳ��
        //5. ui��ʾ��ȫ��(��ÿ��ҳ��ֳ�bg����Ͱ�ȫ��,���ݶ��ŵ���ȫ����)
        //6. Ĭ��UI loading״̬(������Ĭ��ʵ��һ�������������form��)
        //7. �ӹ�UI����������(��������ҳ���������ڹ����Զ���������)
        //8. ShowToastPop����

        //ȫ�ַ��ذ�ť
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
    /// ���_���@ʾһ���´���
    /// �ϴ��ڕ������w�[��
    /// </summary>
    /// <param name="openForm"></param>
    /// <param name="userData"></param>
    public int OpenUIForm(string openForm, object userData = null)
    {
        return GameEntry.UI.OpenUIForm(openForm, UIGroupConfig.UIGroupName, userData);
    }

    /// <summary>
    /// �P�]��ǰ���� �����@ʾ����һ������
    /// </summary>
    public void CloseCurrentPanel()
    {
        GameEntry.UI.CloseUIForm(CurrentPanelSerialId);
    }

    /// <summary>
    /// �P�]��ǰ���� �����@ʾ����һ������
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
    /// �P�]��ǰ���� ���Ҵ��_һ���´����@ʾ
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
    /// �뗣һ�����ڲ����P�]�@������ Visible�@������
    /// Ȼ����_һ�������
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
    /// ��������ҳ�ȼ���Tapҳ��ʹ��
    /// �����Ҫ�򿪵�tapҳ���� ��ر�tapҳ���������ҳ�� Ȼ�󼤻�tapҳ
    /// tapҳ������ �˺�����ر�����ҳ�� Ȼ���tapҳ��
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
    /// ֻ��һ��ȷ�ϰ�ť
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
    /// ������ť һ��ȷ�ϰ�ťһ��ȡ����ť
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
    ///// ϵ�y��ʾ��
    ///// </summary>
    ///// <param name="msg">����</param>
    //public void ShowToastPop(string msg)
    //{
    //    var pop = Doozy.Engine.UI.UIPopup.GetPopup("Toast");
    //    pop.Data.SetLabelsTexts(msg);
    //    pop.Show();
    //}

    ///// <summary>
    ///// ϵ�y��ʾ�� msg �����M�ж��Z���D�Q���@ʾ
    ///// </summary>
    ///// <param name="msg"></param>
    //public void ShowToastPopWidthLocalText(string msg)
    //{
    //    var localMsg = GameEntry.GetLocalText(msg);
    //    ShowToastPop(localMsg);
    //}



    /// <summary>
    /// �O�ÙM��
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
    /// �O���Q��
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
    /// ��GF��Դδ��ʼ��ʱʹ�ô˶Ի���
    /// </summary>
    public void OpenBuiltinDialog(DialogParams<object> dialogParams)
    {
        var dialog = Instantiate(_builtinDialogForm);
        dialog.SetUserData(dialogParams);
    }

}


