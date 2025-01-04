
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client
{
    public class ScrollBaseView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
    {
        public delegate void DragHandler(PointerEventData data);

        public ScrollBaseView.DragHandler onBeginDrag;

        public ScrollBaseView.DragHandler onDrag;

        public ScrollBaseView.DragHandler onEndDrag;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (this.onBeginDrag != null)
            {
                this.onBeginDrag(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (this.onDrag != null)
            {
                this.onDrag(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.onEndDrag != null)
            {
                this.onEndDrag(eventData);
            }
        }
    }
}
