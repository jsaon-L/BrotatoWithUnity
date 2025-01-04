using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Client
{
    public class LongPressBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
    {
        private bool pointerDown;

        private float pointerDownTimer;

        private bool isOtherDown;

        public float reqHoldTimeFristTime;

        public float reqHoldTimeOtherTime;

        public UnityAction action;

        public UnityAction releaseAction;

        private Animation ani;

        public AnimationClip DownAnimation;

        public AnimationClip UpAnimation;

        public bool IsPassEvent;

        private bool IsClickDown;

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

        public void OnPointerDown(PointerEventData eventData)
        {
            this.pointerDown = true;
            if (this.ani != null && this.DownAnimation != null)
            {
                this.ani.cullingType = AnimationCullingType.AlwaysAnimate;
                this.ani.Play("ButtonDownAni");
            }
            this.PassEvent<IPointerDownHandler>(eventData, ExecuteEvents.pointerDownHandler);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (this.isOtherDown)
            {
                this.releaseAction?.Invoke();
            }
            this.Reset();
            if (this.ani != null && this.UpAnimation != null)
            {
                this.ani.cullingType = AnimationCullingType.AlwaysAnimate;
                this.ani.Play("ButtonUpAni");
            }
            this.PassEvent<IPointerUpHandler>(eventData, ExecuteEvents.pointerUpHandler);
        }

        private void Update()
        {
            if (this.pointerDown)
            {
                this.pointerDownTimer += Time.deltaTime;
                float num = (!this.isOtherDown) ? this.reqHoldTimeFristTime : this.reqHoldTimeOtherTime;
                if (this.pointerDownTimer > num)
                {
                    if (this.action != null)
                    {
                        this.action();
                    }
                    this.isOtherDown = true;
                    this.pointerDownTimer = 0f;
                    if (this.reqHoldTimeOtherTime <= 0)
                    {
                        this.pointerDown = false;
                    }
                }
            }
        }

        private void Reset()
        {
            this.pointerDown = false;
            this.isOtherDown = false;
            this.pointerDownTimer = 0f;
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