using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class BtnAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
    {
        private Animation ani;

        public AnimationClip DownAnimation;

        public AnimationClip UpAnimation;

        public bool IsPassEvent;

        private bool IsClickDown;

        private Button mButton;

        private void Start()
        {
            this.ani = base.gameObject.GetComponent<Animation>();
            if (this.ani == null)
            {
                this.ani = base.gameObject.AddComponent<Animation>();
            }
            this.ani.playAutomatically = false;
            this.ani.cullingType = AnimationCullingType.BasedOnRenderers;
            if (this.DownAnimation != null)
            {
                this.ani.AddClip(this.DownAnimation, "ButtonDownAni");
            }
            if (this.UpAnimation != null)
            {
                this.ani.AddClip(this.UpAnimation, "ButtonUpAni");
            }
            UICommon.ResetAnimation(base.gameObject, "ButtonDownAni");
            UICommon.ResetAnimation(base.gameObject, "ButtonUpAni");

            mButton = gameObject.GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (this.ani != null)
            {
                this.ani.cullingType = AnimationCullingType.AlwaysAnimate;
                if (this.UpAnimation != null)
                {
                    this.ani["ButtonUpAni"].time = this.ani["ButtonUpAni"].length;
                    this.ani.Play("ButtonUpAni");
                }
            }
        }

        private void OnDisable()
        {
            if (this.ani != null)
            {
                this.ani.cullingType = AnimationCullingType.BasedOnRenderers;
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (mButton != null && (mButton.enabled == false || mButton.interactable == false))
            {
                return;
            }

            if (this.ani != null && this.DownAnimation != null)
            {
                this.ani.cullingType = AnimationCullingType.AlwaysAnimate;
                this.ani.Play("ButtonDownAni");
            }
            this.PassEvent<IPointerDownHandler>(data, ExecuteEvents.pointerDownHandler);
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (mButton != null && (mButton.enabled == false || mButton.interactable == false))
            {
                return;
            }
            if (this.ani != null && this.UpAnimation != null)
            {
                this.ani.cullingType = AnimationCullingType.AlwaysAnimate;
                this.ani.Play("ButtonUpAni");
            }
            this.PassEvent<IPointerUpHandler>(data, ExecuteEvents.pointerUpHandler);
        }

        public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
        {
            if (!this.IsPassEvent)
            {
                return;
            }
            List<RaycastResult> list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, list);
            GameObject gameObject = data.pointerCurrentRaycast.gameObject;
            for (int i = 0; i < list.Count; i++)
            {
                if (gameObject != list[i].gameObject)
                {
                    BtnAnimation component = list[i].gameObject.GetComponent<BtnAnimation>();
                    if (component == null)
                    {
                        ExecuteEvents.Execute<T>(list[i].gameObject, data, function);
                    }
                }
            }
        }
    }
}