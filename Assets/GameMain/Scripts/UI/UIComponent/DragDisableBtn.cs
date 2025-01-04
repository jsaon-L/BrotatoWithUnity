using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Client
{
    public class DragDisableBtn : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public MaskableGraphic RaycastTarger;

        public void OnDrag(PointerEventData data)
        {
            if(RaycastTarger!=null)
            {
                RaycastTarger.raycastTarget = false;
            }
        }

        public void OnEndDrag(PointerEventData data)
        {
            if (RaycastTarger != null)
            {
                RaycastTarger.raycastTarget = true;
            }
        }
    }
}
