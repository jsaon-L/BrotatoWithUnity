using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client
{
	public class UIEventTrigger : EventTrigger
	{
		public delegate void EventTriggerCB1(PointerEventData data);

		public delegate void EventTriggerCB2(BaseEventData data);

		public delegate void EventTriggerCB3(AxisEventData data);

		public EventTriggerCB1 onBeginDrag;

		public EventTriggerCB2 onCancel;

		public EventTriggerCB2 onDeselect;

		public EventTriggerCB1 onDrag;

		public EventTriggerCB1 onDrop;

		public EventTriggerCB1 onEndDrag;

		public EventTriggerCB1 onInitializePotentialDrag;

		public EventTriggerCB3 onMove;

		public EventTriggerCB1 onPointerClick;

		public EventTriggerCB1 onPointerDown;

		public EventTriggerCB1 onPointerEnter;

		public EventTriggerCB1 onPointerExit;

		public EventTriggerCB1 onPointerUp;

		public EventTriggerCB1 onScroll;

		public EventTriggerCB2 onSelect;

		public EventTriggerCB2 onSubmit;

		public EventTriggerCB2 onUpdateSelected;

		public override void OnBeginDrag(PointerEventData data)
		{
			base.OnBeginDrag(data);
			onBeginDrag?.Invoke(data);
		}


		public override void OnDrag(PointerEventData data)
		{
			base.OnDrag(data);
			onDrag?.Invoke(data);
		}

		public override void OnEndDrag(PointerEventData data)
		{
			base.OnEndDrag(data);
			onEndDrag?.Invoke(data);
		}

		public void SetOnSelectCB(EventTriggerCB2 cb)
		{
			if (onSelect != null)
			{
				onSelect = cb;
			}
		}

		public override void OnCancel(BaseEventData data)
		{
			base.OnCancel(data);
			onCancel?.Invoke(data);
		}

		public override void OnDeselect(BaseEventData data)
		{
			base.OnDeselect(data);
			onDeselect?.Invoke(data);
		}

		public override void OnDrop(PointerEventData data)
		{
			base.OnDrop(data);
			onDrop?.Invoke(data);
		}

		public override void OnInitializePotentialDrag(PointerEventData data)
		{
			base.OnInitializePotentialDrag(data);
			onInitializePotentialDrag?.Invoke(data);
		}

		public override void OnMove(AxisEventData data)
		{
			base.OnMove(data);
			onMove?.Invoke(data);
		}

		public override void OnPointerClick(PointerEventData data)
		{
			base.OnPointerClick(data);
			onPointerClick?.Invoke(data);
		}

		public override void OnPointerDown(PointerEventData data)
		{
			base.OnPointerDown(data);
			onPointerDown?.Invoke(data);
		}

		public override void OnPointerEnter(PointerEventData data)
		{
			base.OnPointerEnter(data);
			onPointerEnter?.Invoke(data);
		}

		public override void OnPointerExit(PointerEventData data)
		{
			base.OnPointerExit(data);
			onPointerExit?.Invoke(data);
		}

		public override void OnPointerUp(PointerEventData data)
		{
			base.OnPointerUp(data);
			onPointerUp?.Invoke(data);
		}

		public override void OnScroll(PointerEventData data)
		{
			base.OnScroll(data);
			onScroll?.Invoke(data);
		}

		public override void OnSelect(BaseEventData data)
		{
			base.OnSelect(data);
			onSelect?.Invoke(data);
		}

		public override void OnSubmit(BaseEventData data)
		{
			base.OnSubmit(data);
			onSubmit?.Invoke(data);
		}

		public override void OnUpdateSelected(BaseEventData data)
		{
			base.OnUpdateSelected(data);
			onUpdateSelected?.Invoke(data);
		}
	}
}

