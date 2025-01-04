using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarForce;
using UnityEngine.UI;
using TMPro;
using UnityGameFramework.Runtime;
using DG.Tweening;
using GameFramework;

namespace StarForce
{
    public class UIPopupBase : UIFormLogic
    {

        protected OpenPopupData _openPopupData;

        public GameObject Bg;
        public GameObject Content;

        public List<Button> Buttons;
        public List<TextMeshProUGUI> Texts;



        public void Close()
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            StopAllCoroutines();

            if (ignoreFade)
            {
                GameEntry.UI.CloseUIForm(this.UIForm);
            }
            else
            {
                StartCoroutine(CloseCo());
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            _openPopupData = userData as OpenPopupData;
            RegisterPopupData(_openPopupData);

            StopAllCoroutines();
            StartCoroutine(OpenAnimation());
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            if (_openPopupData!=null)
            {
                _openPopupData.CloseAction?.Invoke();
            }
            UnRegisterPopupData(_openPopupData);
            ReferencePool.Release(_openPopupData);
        }


        #region animations

        private IEnumerator CloseCo()
        {

            yield return CloseAnimation();
            GameEntry.UI.CloseUIForm(this.UIForm);
        }


        protected virtual IEnumerator OpenAnimation()
        {
            yield break;
        }

        protected virtual IEnumerator CloseAnimation()
        {
            yield break;
        }

        public void AutoClose(float delay)
        {
            StartCoroutine(CloseSelf(delay));
        }

        IEnumerator CloseSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
            Close();
        }
        #endregion

        #region RegisterPopupData

        private void RegisterPopupData(OpenPopupData openPopupData)
        {
            if (openPopupData == null)
            {
                return;
            }

            //ÅäÖÃ
            if (openPopupData.Texts != null && Texts != null)
            {
                int textCount = openPopupData.Texts.Count >= Texts.Count ? openPopupData.Texts.Count : Texts.Count;

                for (int i = 0; i < textCount; i++)
                {
                    if (Texts[i])
                    {
                        Texts[i].text = openPopupData.Texts[i];
                    }
                }
            }
            if (openPopupData.ButtonActions != null && Buttons != null)
            {
                int buttonCount = openPopupData.ButtonActions.Count >= Buttons.Count ? openPopupData.ButtonActions.Count : Buttons.Count;

                for (int i = 0; i < buttonCount; i++)
                {
                    if (Buttons[i])
                    {
                        Buttons[i].onClick.AddListener(openPopupData.ButtonActions[i]);
                    }
                }
            }

            //×Ô¶¯¹Ø±Õ
            if (openPopupData.AutoClose)
            {
                AutoClose(openPopupData.AutoCloseDelay);
            }
        }
        private void UnRegisterPopupData(OpenPopupData openPopupData)
        {
            if (openPopupData == null)
            {
                return;
            }

            //ÅäÖÃ
            if (openPopupData.Texts != null && Texts != null)
            {
                int textCount = openPopupData.Texts.Count >= Texts.Count ? openPopupData.Texts.Count : Texts.Count;

                for (int i = 0; i < textCount; i++)
                {
                    if (Texts[i])
                    {
                        Texts[i].text = string.Empty;
                    }
                }
            }
            if (openPopupData.ButtonActions != null && Buttons != null)
            {
                int buttonCount = openPopupData.ButtonActions.Count >= Buttons.Count ? openPopupData.ButtonActions.Count : Buttons.Count;

                for (int i = 0; i < buttonCount; i++)
                {
                    if (Buttons[i])
                    {
                        Buttons[i].onClick.RemoveListener(openPopupData.ButtonActions[i]);
                    }
                }
            }
        }
        #endregion

    }
}
