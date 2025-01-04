//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using TMPro;
using System.Collections.Generic;
using System.Linq;

namespace StarForce
{
    public class DialogForm<T> : UGuiForm
    {
        //[SerializeField]
        private TextMeshProUGUI m_TitleText = null;

        //[SerializeField]
        private TextMeshProUGUI m_MessageText = null;

        //[SerializeField]
        private GameObject[] m_ModeObjects = null;

        //[SerializeField]
        private TextMeshProUGUI[] m_ConfirmTexts = null;

        //[SerializeField]
        private TextMeshProUGUI[] m_CancelTexts = null;

        //[SerializeField]
        private TextMeshProUGUI[] m_OtherTexts = null;

        private List<CommonButton> _confirmButton;
        private List<CommonButton> _cancelButton;
        private List<CommonButton> _otherButton;

        private int m_DialogMode = 1;
        private bool m_PauseGame = false;
        private object m_UserData = null;
        private GameFrameworkAction<DialogResule<T>> m_OnClickConfirm = null;
        private GameFrameworkAction<DialogResule<T>> m_OnClickCancel = null;
        private GameFrameworkAction<DialogResule<T>> m_OnClickOther = null;

        private DialogResule<T> _dialogResule = null;
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

        public void OnConfirmButtonClick()
        {
            _dialogResule.DialogValue = GetDialogResuleData();  
            Close();

            if (m_OnClickConfirm != null)
            {
                m_OnClickConfirm(_dialogResule);
            }
        }

        public void OnCancelButtonClick()
        {
            _dialogResule.DialogValue = GetDialogResuleData();
            Close();

            if (m_OnClickCancel != null)
            {
                m_OnClickCancel(_dialogResule);
            }
        }

        public void OnOtherButtonClick()
        {
            _dialogResule.DialogValue = GetDialogResuleData();
            Close();

            if (m_OnClickOther != null)
            {
                m_OnClickOther(_dialogResule);
            }
        }

        protected virtual T GetDialogResuleData()
        {
            return default;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_TitleText = FindObject<TextMeshProUGUI>("Title");
            m_MessageText = FindObject<TextMeshProUGUI>("Message");

            m_ModeObjects = FindObjects<Transform>("ButtonGroup", true).Select(t => t.gameObject).ToArray();
            m_ConfirmTexts = FindObjects<TextMeshProUGUI>("Confirm", true).ToArray();
            m_CancelTexts = FindObjects<TextMeshProUGUI>("Cancel", true).ToArray();
            m_OtherTexts = FindObjects<TextMeshProUGUI>("Other", true).ToArray();


            _confirmButton = FindObjects<CommonButton>("Confirm");
            _cancelButton = FindObjects<CommonButton>("Cancel");
            _otherButton = FindObjects<CommonButton>("Other");

            foreach (var bt in _confirmButton)
            {
                bt.m_OnClick.AddListener(OnConfirmButtonClick);
            }
            foreach (var bt in _cancelButton)
            {
                bt.m_OnClick.AddListener(OnCancelButtonClick);
            }
            foreach (var bt in _otherButton)
            {
                bt.m_OnClick.AddListener(OnOtherButtonClick);
            }
        }



#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            DialogParams<T> dialogParams = (DialogParams<T>)userData;
            if (dialogParams == null)
            {
                Log.Warning("DialogParams is invalid.");
                return;
            }

            m_DialogMode = dialogParams.Mode;
            RefreshDialogMode();

            m_TitleText.text = dialogParams.Title;
            m_MessageText.text = dialogParams.Message;

            m_PauseGame = dialogParams.PauseGame;
            RefreshPauseGame();

            m_UserData = dialogParams.UserData;
            _dialogResule = new DialogResule<T>();
            _dialogResule.UserData = dialogParams.UserData;

            RefreshConfirmText(dialogParams.ConfirmText);
            m_OnClickConfirm = dialogParams.OnClickConfirm;

            RefreshCancelText(dialogParams.CancelText);
            m_OnClickCancel = dialogParams.OnClickCancel;

            RefreshOtherText(dialogParams.OtherText);
            m_OnClickOther = dialogParams.OnClickOther;
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            if (m_PauseGame)
            {
                GameEntry.Base.ResumeGame();
            }

            m_DialogMode = 1;
            m_TitleText.text = string.Empty;
            m_MessageText.text = string.Empty;
            m_PauseGame = false;
            m_UserData = null;

            RefreshConfirmText(string.Empty);
            m_OnClickConfirm = null;

            RefreshCancelText(string.Empty);
            m_OnClickCancel = null;

            RefreshOtherText(string.Empty);
            m_OnClickOther = null;

            base.OnClose(isShutdown, userData);
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
            for (int i = 0; i < m_ConfirmTexts.Length; i++)
            {
                m_ConfirmTexts[i].text = confirmText;
            }
        }

        private void RefreshCancelText(string cancelText)
        {
            for (int i = 0; i < m_CancelTexts.Length; i++)
            {
                m_CancelTexts[i].text = cancelText;
            }
        }

        private void RefreshOtherText(string otherText)
        {
            for (int i = 0; i < m_OtherTexts.Length; i++)
            {
                m_OtherTexts[i].text = otherText;
            }
        }
    }
}
