//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using TMPro;

namespace StarForce
{

    /// <summary>
    /// 当GF资源管理器未加载时使用此对话框
    /// </summary>
    public class BuiltinDialogForm : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_TitleText = null;

        [SerializeField]
        private TextMeshProUGUI m_MessageText = null;

        [SerializeField]
        private GameObject[] m_ModeObjects = null;

        [SerializeField]
        private TextMeshProUGUI[] m_ConfirmTexts = null;

        [SerializeField]
        private TextMeshProUGUI[] m_CancelTexts = null;

        [SerializeField]
        private TextMeshProUGUI[] m_OtherTexts = null;

        private int m_DialogMode = 1;
        private bool m_PauseGame = false;
        private object m_UserData = null;
        private GameFrameworkAction<DialogResule<object>> m_OnClickConfirm = null;
        private GameFrameworkAction<DialogResule<object>> m_OnClickCancel = null;
        private GameFrameworkAction<DialogResule<object>> m_OnClickOther = null;

        private object _userData;


        public int DialogMode
        {
            get
            {
                return m_DialogMode;
            }
        }

        public bool PauseGame
        {
            get
            {
                return m_PauseGame;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }
        public void SetUserData(object userData)
        {
            _userData = userData;
        }
        private void Close()
        {
            Destroy(gameObject);
        }
        public void OnConfirmButtonClick()
        {
            if (m_OnClickConfirm != null)
            {
                m_OnClickConfirm(new DialogResule<object> {UserData = m_UserData } );
            }

            Close();
        }

        public void OnCancelButtonClick()
        {
            if (m_OnClickCancel != null)
            {
                m_OnClickCancel(new DialogResule<object> { UserData = m_UserData });
            }

            Close();
        }

        public void OnOtherButtonClick()
        {
            if (m_OnClickOther != null)
            {
                m_OnClickOther(new DialogResule<object> { UserData = m_UserData });
            }
            Close();
        }


        private void Start()
        {
            DialogParams<object> dialogParams = (DialogParams<object>)_userData;
            if (dialogParams == null)
            {
                Log.Warning("DialogParams is invalid.");
                return;
            }

            m_DialogMode = dialogParams.Mode;
            RefreshDialogMode();

            m_TitleText.text = dialogParams.Title;
            m_MessageText.text = dialogParams.Message;

            //m_PauseGame = dialogParams.PauseGame;
            //RefreshPauseGame();

            m_UserData = dialogParams.UserData;

            RefreshConfirmText(dialogParams.ConfirmText);
            m_OnClickConfirm = dialogParams.OnClickConfirm;

            RefreshCancelText(dialogParams.CancelText);
            m_OnClickCancel = dialogParams.OnClickCancel;

            RefreshOtherText(dialogParams.OtherText);
            m_OnClickOther = dialogParams.OnClickOther;
        }

        private void OnDestroy()
        {


            //if (m_PauseGame)
            //{
            //    GameEntry.Base.ResumeGame();
            //}

            //m_DialogMode = 1;
            //m_TitleText.text = string.Empty;
            //m_MessageText.text = string.Empty;
            //m_PauseGame = false;
            //m_UserData = null;

            //RefreshConfirmText(string.Empty);
            //m_OnClickConfirm = null;

            //RefreshCancelText(string.Empty);
            //m_OnClickCancel = null;

            //RefreshOtherText(string.Empty);
            //m_OnClickOther = null;

        }

        private void RefreshDialogMode()
        {
            for (int i = 1; i <= m_ModeObjects.Length; i++)
            {
                m_ModeObjects[i - 1].SetActive(i == m_DialogMode);
            }
        }

        private void RefreshPauseGame()
        {
            if (m_PauseGame)
            {
                GameEntry.Base.PauseGame();
            }
        }

        private void RefreshConfirmText(string confirmText)
        {
            if (string.IsNullOrEmpty(confirmText))
            {
                //confirmText = GameEntry.Localization.GetString("Dialog.ConfirmButton");
                confirmText = "确定";
            }

            for (int i = 0; i < m_ConfirmTexts.Length; i++)
            {
                m_ConfirmTexts[i].text = confirmText;
            }
        }

        private void RefreshCancelText(string cancelText)
        {
            if (string.IsNullOrEmpty(cancelText))
            {
                cancelText = "取消";
            }

            for (int i = 0; i < m_CancelTexts.Length; i++)
            {
                m_CancelTexts[i].text = cancelText;
            }
        }

        private void RefreshOtherText(string otherText)
        {
            if (string.IsNullOrEmpty(otherText))
            {
                otherText = "其他";
            }

            for (int i = 0; i < m_OtherTexts.Length; i++)
            {
                m_OtherTexts[i].text = otherText;
            }
        }
    }
}
