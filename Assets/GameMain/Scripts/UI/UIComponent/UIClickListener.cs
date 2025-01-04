using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client
{
    public class UIClickListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IEventSystemHandler
    {
        public delegate void EventTriggerCB1(PointerEventData data);

        public UIClickListener.EventTriggerCB1 onPointerDown;

        public UIClickListener.EventTriggerCB1 onPointerUp;

        public UIClickListener.EventTriggerCB1 onPointerClick;

        private void Start()
        {
        }

        private void Update()
        {
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if (this.onPointerDown != null)
            {
                this.onPointerDown(pointerEventData);
            }
        }

        public void OnPointerUp(PointerEventData pointerEventData)
        {
            if (this.onPointerUp != null)
            {
                this.onPointerUp(pointerEventData);
            }
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (this.onPointerClick != null)
            {
                this.onPointerClick(pointerEventData);
            }
        }
    }
}