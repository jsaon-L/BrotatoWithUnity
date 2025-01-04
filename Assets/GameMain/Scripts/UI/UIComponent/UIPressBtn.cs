using System;
using System.Collections.Generic;
using Skyunion;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UIPressBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public Action OnPointUpCallback;

        private bool IsPassEvent = false;
        private bool isPress = false;
        private Timer timerPress;
        private Action pressCallback;
        private float tims;

        public void OnPointerUp(PointerEventData eventData)
        {
            isPress = false;
            CancelInvoke("OnPress");
            if (timerPress != null)
            {
                timerPress.Cancel();
            }
            OnPointUpCallback?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (timerPress != null)
            {
                timerPress.Cancel();
            }

            timerPress = Timer.Register(tims, () =>
            {
                isPress = true;
                //InvokeRepeating("OnPress", 0, 1f);       
                OnPress();
                if (timerPress != null)
                {
                    timerPress.Cancel();
                }
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPress = false;
            CancelInvoke("OnPress");
            if (timerPress != null)
            {
                timerPress.Cancel();
            }
        }

        private void OnPress()
        {
            if (isPress)
            {
                if (pressCallback != null)
                {
                    pressCallback.Invoke();
                }
            }
        }

        public void Register(float times = 1.5f, bool isBgActived = false)
        {
            this.IsPassEvent = isBgActived;
            tims = times;
        }

        public void AddPressClick(Action callback)
        {
            pressCallback += callback;
        }

        public void RemovePressClick(Action callback)
        {
            pressCallback -= callback;
        }

        public void RemoveAllPressClick()
        {
            pressCallback = null;
        }
        
    }
}