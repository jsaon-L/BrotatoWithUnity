using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class ScrollView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
    {
        public enum ScrollViewType
        {
            EqualLength,
            UnequalLength
        }

        public enum ScrollViewLayoutType
        {
            Horizontal,
            Vertical
        }

        public class ItemObject
        {
            public RectTransform itemObject;

            public bool bUsed;

            public bool isInit;

            public void SetItemShow(bool bShow)
            {
                this.itemObject.gameObject.SetActive(bShow);
                this.bUsed = bShow;
            }
        }

        public class ScrollItem
        {
            public Vector3 position;

            public ScrollView.ItemObject gameObject;

            public string tag;

            public int index;

            public bool bForceRefresh;

            public float height;

            public ScrollItem()
            {
                this.position = Vector3.zero;
                this.gameObject = null;
                this.tag = string.Empty;
                this.index = 0;
                this.bForceRefresh = false;
            }

            public GameObject GetGameObject()
            {
                if (this.gameObject != null)
                {
                    return this.gameObject.itemObject.gameObject;
                }
                return null;
            }
        }

        public class FuncTab
        {
            public Action<ScrollView.ScrollItem> ItemEnter;
            public Action<ScrollView.ScrollItem> ItemRemove;
        }

        private const int EXTRA_CACHE_NUM = 4;

        public ScrollView.ScrollViewLayoutType layoutType;

        public List<string> ItemPrefabDataList = new List<string>();

        public RectTransform listContainer;

        public float offset;

        public float spacing;

        public bool dontNeedPool;

        private GameObject itemTemplate;

        private List<ScrollView.ScrollItem> scrollItemList = new List<ScrollView.ScrollItem>();

        private List<ScrollView.ItemObject> itemObjectPool = new List<ScrollView.ItemObject>();

        private int ItemTotalNum;

        public float CurShowTopY;

        public float CurShowBottomY;

        private float m_curItemUsedY;

        private float m_totalItemHeight;

        private Vector2 m_viewRect;

        private float m_keyboardHeight;

        private float autoScrollTime = 0.2f;

        private bool autoScroll;

        private Vector4 autoScrollParam;

        private bool isVertical;

        private string m_perfabName = string.Empty;

        private Dictionary<string, GameObject> assetObjectDic;
        private Action<ScrollItem> mOnGetItemByIndex;
        private ScrollView.FuncTab m_funcTab;

        public float GetItemHeight(RectTransform rt)
        {
            return (!this.isVertical) ? rt.rect.width : rt.rect.height;
        }

        public void SetContainerHeight(float height)
        {
            if (this.isVertical)
            {
                this.listContainer.sizeDelta = new Vector3(this.listContainer.rect.width, height);
            }
            else
            {
                this.listContainer.sizeDelta = new Vector3(height, this.listContainer.rect.height);
            }
        }

        public float GetContainerHeight()
        {
            if (this.isVertical)
            {
                return this.listContainer.rect.height;
            }
            return this.listContainer.rect.width;
        }

        public float GetContainerHeight1()
        {
            if (this.isVertical)
            {
                return this.listContainer.rect.height;
            }
            return this.listContainer.rect.width;
        }

        public float GetPanelHeight()
        {
            RectTransform component = base.GetComponent<RectTransform>();
            if (this.isVertical)
            {
                return component.rect.height;
            }
            return component.rect.width;
        }

        public void GetUnusedItemObject(Action<ItemObject> action)
        {
            ScrollView.ItemObject itemObject = null;
            for (int i = 0; i < this.itemObjectPool.Count; i++)
            {
                ScrollView.ItemObject itemObject2 = this.itemObjectPool[i];
                if (!itemObject2.bUsed)
                {
                    itemObject = itemObject2;
                    break;
                }
            }
            if (itemObject == null)
            {
                this.CreateItemObject((ItemObject item) =>
                {
                    if (item != null)
                    {
                        this.itemObjectPool.Add(item);
                    }
                    action?.Invoke(item);
                });
                return;
            }
            action?.Invoke(itemObject);
        }

        public void RefreshItemShow()
        {
            for (int i = 0; i < this.scrollItemList.Count; i++)
            {
                ScrollView.ScrollItem scrollItem = this.scrollItemList[i];
                if (this.isVertical)
                {
                    if (scrollItem.position.y <= this.CurShowTopY && scrollItem.position.y >= this.CurShowBottomY)
                    {
                        if (scrollItem.gameObject == null)
                        {
                            this.GetUnusedItemObject((ItemObject unusedItemObject) =>
                            {
                                if (unusedItemObject != null)
                                {
                                    unusedItemObject.SetItemShow(true);
                                    unusedItemObject.itemObject.anchoredPosition3D = scrollItem.position;
                                    scrollItem.gameObject = unusedItemObject;
                                    this.OnMoveIn(scrollItem);
                                }
                            });
                        }
                        else if (scrollItem.bForceRefresh)
                        {
                            scrollItem.gameObject.itemObject.anchoredPosition3D = scrollItem.position;
                            this.OnMoveIn(scrollItem);
                        }
                    }
                    else if (scrollItem.gameObject != null && !this.dontNeedPool)
                    {
                        this.OnMoveOut(scrollItem);
                        scrollItem.gameObject.SetItemShow(false);
                        scrollItem.gameObject = null;
                    }
                }
                else if (scrollItem.position.x >= this.CurShowTopY && scrollItem.position.x <= this.CurShowBottomY)
                {
                    if (scrollItem.gameObject == null)
                    {
                        this.GetUnusedItemObject((ItemObject unusedItemObject2) =>
                        {
                            if (unusedItemObject2 != null)
                            {
                                unusedItemObject2.SetItemShow(true);
                                unusedItemObject2.itemObject.anchoredPosition3D = scrollItem.position;
                                scrollItem.gameObject = unusedItemObject2;
                                this.OnMoveIn(scrollItem);
                            }
                        });
                    }
                    else if (scrollItem.bForceRefresh)
                    {
                        scrollItem.gameObject.itemObject.anchoredPosition3D = scrollItem.position;
                        this.OnMoveIn(scrollItem);
                    }
                }
                else if (scrollItem.gameObject != null && !this.dontNeedPool)
                {
                    this.OnMoveOut(scrollItem);
                    scrollItem.gameObject.SetItemShow(false);
                    scrollItem.gameObject = null;
                }
                scrollItem.bForceRefresh = false;
            }
        }

        public void RefreshShowRect()
        {
            if (this.isVertical)
            {
                this.CurShowTopY = -this.listContainer.anchoredPosition3D.y + this.GetPanelHeight() / 2f;
                this.CurShowBottomY = -(this.listContainer.anchoredPosition3D.y + this.GetPanelHeight() * 1.2f);
            }
            else
            {
                this.CurShowTopY = -this.listContainer.anchoredPosition3D.x - this.GetPanelHeight() / 2f;
                this.CurShowBottomY = -this.listContainer.anchoredPosition3D.x + this.GetPanelHeight() * 1.2f;
            }
        }

        public void OnValueChanged(Vector2 vec2)
        {
            this.RefreshShowRect();
            this.RefreshItemShow();
        }

        private void Start()
        {
            this.m_totalItemHeight += this.offset;
        }

        public void SetInitData(GameObject prefab, ScrollView.FuncTab funcTab)
        {
            this.m_perfabName = prefab.name;
            m_funcTab = funcTab;
            if (prefab)
            {
                assetObjectDic = new Dictionary<string, GameObject>();
                assetObjectDic[this.m_perfabName] = prefab;
                this.Init(prefab);
            }
            if (this.m_viewRect.x == this.m_viewRect.y)
            {
                if (this.isVertical)
                {
                    this.m_viewRect = new Vector2(this.listContainer.anchoredPosition3D.y, this.listContainer.anchoredPosition3D.y - this.GetPanelHeight());
                }
                else
                {
                    this.m_viewRect = new Vector2(this.listContainer.anchoredPosition3D.x, this.listContainer.anchoredPosition3D.x + this.GetPanelHeight());
                }
            }
        }

        public void CreateItemObject(Action<ItemObject> action)
        {
            GameObject gameObject = GameObject.Instantiate(assetObjectDic[this.m_perfabName]);
            //CoreUtils.assetService.Instantiate(this.m_perfabName, (GameObject gameObject) =>
            //{
                gameObject.transform.SetParent(this.listContainer.transform);
                if (gameObject != null)
                {
                    ScrollView.ItemObject itemObject = new ScrollView.ItemObject();
                    itemObject.bUsed = false;
                    RectTransform component = gameObject.GetComponent<RectTransform>();
                    component.anchoredPosition3D = Vector3.zero;
                    component.localScale = Vector3.one;
                    itemObject.itemObject = component;
                    action?.Invoke(itemObject);
                }
                action?.Invoke(null);
            //});
        }

        public void Init(GameObject template)
        {
            this.isVertical = (this.layoutType == ScrollView.ScrollViewLayoutType.Vertical);
            this.itemTemplate = template;
            ScrollRect component = base.GetComponent<ScrollRect>();
            if (component != null)
            {
                component.vertical = this.isVertical;
                component.horizontal = !this.isVertical;
                component.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnValueChanged));
            }
        }

        public ScrollView.ScrollItem AddItem(float itemHeight, string tag)
        {
            this.ItemTotalNum++;
            int index = this.ItemTotalNum - 1;
            ScrollView.ScrollItem scrollItem = new ScrollView.ScrollItem();
            scrollItem.tag = tag;
            scrollItem.index = index;
            scrollItem.height = itemHeight;
            if (this.ItemTotalNum == 1)
            {
                this.m_curItemUsedY -= this.offset;
            }
            if (this.isVertical)
            {
                this.m_curItemUsedY -= this.spacing;
                scrollItem.position = new Vector3(0f, this.m_curItemUsedY, 0f);
                this.m_curItemUsedY -= itemHeight;
            }
            else
            {
                this.m_curItemUsedY += this.spacing;
                scrollItem.position = new Vector3(this.m_curItemUsedY, 0f, 0f);
                this.m_curItemUsedY += itemHeight;
            }
            if (this.isVertical)
            {
                this.m_totalItemHeight = -scrollItem.position.y + scrollItem.height + this.spacing;
            }
            else
            {
                this.m_totalItemHeight = scrollItem.position.x + scrollItem.height + this.spacing;
            }
            this.scrollItemList.Add(scrollItem);
            this.SetContainerHeight(this.m_totalItemHeight);
            this.RefreshShowRect();
            this.RefreshItemShow();
            return scrollItem;
        }

        public GameObject GetTemplateObject()
        {
            return this.itemTemplate;
        }

        public void LocateItemPosition(int index)
        {
            if (this.m_totalItemHeight < this.GetPanelHeight())
            {
                return;
            }
            int index2 = index;
            if (index == -1)
            {
                index2 = this.ItemTotalNum - 1;
            }
            ScrollView.ScrollItem itemByIndex = this.GetItemByIndex(index2);
            Vector3 anchoredPosition3D = this.listContainer.anchoredPosition3D;
            float num2;
            if (this.isVertical)
            {
                float num = this.m_viewRect.y + itemByIndex.height;
                num2 = num - (itemByIndex.position.y + this.listContainer.anchoredPosition3D.y);
                anchoredPosition3D.y += num2;
            }
            else
            {
                float num3 = this.m_viewRect.x + itemByIndex.height;
                num2 = num3 - (itemByIndex.position.x + this.listContainer.anchoredPosition3D.x);
                anchoredPosition3D.x += num2;
            }
            this.listContainer.anchoredPosition3D = anchoredPosition3D;
            this.RefreshShowRect();
            this.RefreshItemShow();
        }

        public void Clear(bool completed)
        {
            for (int i = 0; i < this.scrollItemList.Count; i++)
            {
                ScrollView.ScrollItem scrollItem = this.scrollItemList[i];
                if (scrollItem.gameObject != null)
                {
                    scrollItem.gameObject.SetItemShow(false);
                    scrollItem.gameObject = null;
                }
            }
            this.scrollItemList.Clear();
            if (completed)
            {
                for (int j = 0; j < this.itemObjectPool.Count; j++)
                {
                    ScrollView.ItemObject itemObject = this.itemObjectPool[j];
                    GameObject.Destroy(itemObject.itemObject.gameObject);
                }
                this.itemObjectPool.Clear();
            }
            this.ItemTotalNum = 0;
            this.m_curItemUsedY = 0f;
            this.m_totalItemHeight = this.offset;
            Vector3 anchoredPosition3D = this.listContainer.anchoredPosition3D;
            if (this.isVertical)
            {
                anchoredPosition3D.y = 0f;
            }
            else
            {
                anchoredPosition3D.x = 0f;
            }
            this.listContainer.anchoredPosition3D = anchoredPosition3D;
        }

        public ScrollView.ScrollItem InsertItem(int index,float itemHeight, string tag = "")
        {
            if (index > this.ItemTotalNum)
            {
                Debug.LogWarning("Insert item failed! Index more than the ItemTotalNum.");
                return null;
            }
            ScrollView.ScrollItem scrollItem = new ScrollView.ScrollItem();
            scrollItem.tag = tag;
            scrollItem.index = index;
            scrollItem.height = itemHeight;
            float itemPos = 0f;
            for(int i = 0;i<ItemTotalNum;i++)
            {
                ScrollView.ScrollItem scrollItem2 = this.scrollItemList[i];
                if (scrollItem2.index >= scrollItem.index)
                {
                    scrollItem2.index++;
                    scrollItem2.position += this.isVertical ? new Vector3(0, -itemHeight-spacing, 0) : new Vector3(itemHeight+spacing, 0, 0);
                    scrollItem2.bForceRefresh = true;
                }
                else
                {
                    itemPos += scrollItem2.height + spacing;
                }
            }
            if (isVertical)
            {
                scrollItem.position = new Vector3(0f, -itemPos, 0f);
            }
            else
            {
                scrollItem.position = new Vector3(itemPos, 0f, 0f);
            }
            this.scrollItemList.Add(scrollItem);
            this.ItemTotalNum++;
            this.m_totalItemHeight += itemHeight + this.spacing;
            this.m_curItemUsedY -= (itemHeight + this.spacing);
            this.SetContainerHeight(GetContainerHeight()+ itemHeight);
            this.RefreshShowRect();
            this.RefreshItemShow();
            return scrollItem;
        }

        public void RemoveItem(ScrollView.ScrollItem item)
        {
            if(item==null||item.index<0)
            {
                return;
            }
            for (int i = 0; i < this.ItemTotalNum; i++)
            {
                ScrollView.ScrollItem scrollItem = this.scrollItemList[i];
                if (scrollItem.index > item.index)
                {
                    scrollItem.index--;
                    if (this.isVertical)
                    {
                        scrollItem.position.y += (item.height + this.spacing);
                    }
                    else
                    {
                        scrollItem.position.x -= (item.height + this.spacing);
                    }
                    scrollItem.bForceRefresh = true;
                }
            }
            this.OnMoveOut(item);
            if (item.gameObject != null)
            {
                item.gameObject.SetItemShow(false);
                item.gameObject = null;
            }
            this.scrollItemList.Remove(item);
            this.ItemTotalNum--;
            this.m_totalItemHeight -= item.height + this.spacing;
            this.m_curItemUsedY += item.height + this.spacing;
            this.SetContainerHeight(this.m_totalItemHeight);
            this.RefreshShowRect();
            this.RefreshItemShow();
        }

        public void RemoveItemFromBehind()
        {
            ScrollView.ScrollItem scrollItem = this.scrollItemList[this.ItemTotalNum - 1];
            if (scrollItem != null && scrollItem.gameObject != null)
            {
                this.OnMoveOut(scrollItem);
                scrollItem.gameObject.SetItemShow(false);
                scrollItem.gameObject = null;
                this.scrollItemList.Remove(scrollItem);
                this.ItemTotalNum--;
                this.m_totalItemHeight -= scrollItem.height + this.spacing;
                this.m_curItemUsedY += scrollItem.height + this.spacing;
                this.SetContainerHeight(this.m_totalItemHeight);
                this.RefreshShowRect();
                this.RefreshItemShow();
            }
        }

        public void RefreshAllItemPos(bool bForceRefresh)
        {
            float height = 0f;
            for (int i = 0; i < this.ItemTotalNum; i++)
            {
                ScrollView.ScrollItem scrollItem = this.scrollItemList[i];
                if(scrollItem!=null)
                {
                    if(isVertical)
                    {
                        scrollItem.position.y = -height - spacing;
                    }
                    else
                    {
                        scrollItem.position.x = height + spacing;
                    }
                    height += scrollItem.height + spacing;
                    if(bForceRefresh)
                    {
                        scrollItem.bForceRefresh = true;
                    }
                }
            }
            this.m_totalItemHeight = height;
            this.m_curItemUsedY = -height;
            this.SetContainerHeight(this.m_totalItemHeight);
            this.RefreshShowRect();
            this.RefreshItemShow();
        }

        public void OnMoveOut(ScrollView.ScrollItem item)
        {
            if (m_funcTab != null && m_funcTab.ItemRemove != null)
            {
                m_funcTab.ItemRemove(item);
            }
        }

        public void OnMoveIn(ScrollView.ScrollItem item)
        {
            if (m_funcTab != null && m_funcTab.ItemEnter != null)
            {
                m_funcTab.ItemEnter(item);
            }
        }

        public ScrollView.ScrollItem GetItemByIndex(int index)
        {
            for (int i = 0; i < this.scrollItemList.Count; i++)
            {
                ScrollView.ScrollItem scrollItem = this.scrollItemList[i];
                if (scrollItem != null && scrollItem.index == index)
                {
                    return scrollItem;
                }
            }
            return null;
        }

        public ScrollView.ScrollItem GetItemByTag(string tag)
        {
            for (int i = 0; i < this.scrollItemList.Count; i++)
            {
                ScrollView.ScrollItem scrollItem = this.scrollItemList[i];
                if (scrollItem != null && scrollItem.tag == tag)
                {
                    return scrollItem;
                }
            }
            return null;
        }

        public void ForceRefresh()
        {
            for (int i = 0; i < this.scrollItemList.Count; i++)
            {
                ScrollView.ScrollItem scrollItem = this.scrollItemList[i];
                if (scrollItem != null)
                {
                    scrollItem.bForceRefresh = true;
                }
            }
            this.RefreshShowRect();
            this.RefreshItemShow();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        private void SetContainerPos(float pos)
        {
            this.listContainer.anchoredPosition = ((!this.isVertical) ? new Vector2(pos, 0f) : new Vector2(0f, pos));
        }

        private float GetContainerPos()
        {
            return (!this.isVertical) ? (-this.listContainer.anchoredPosition.x) : this.listContainer.anchoredPosition.y;
        }

        public void ScrollToPos(float dest)
        {
            float containerPos = this.GetContainerPos();
            this.autoScroll = true;
            this.autoScrollParam = new Vector4((dest - containerPos) / this.autoScrollTime, this.autoScrollTime, dest, containerPos);
        }

        public void ScrollPanelToItemInidex(int index)
        {
            if (index >= this.scrollItemList.Count || index < 0)
            {
                return;
            }
            ScrollItem item = GetItemByIndex(index);
            float num = isVertical ? -item.position.y : -item.position.x;
            float containerPos = this.GetContainerPos();
            this.autoScroll = true;
            this.autoScrollParam = new Vector4((num - containerPos) / this.autoScrollTime, this.autoScrollTime, num, containerPos);
        }

        public void MovePanelToItemIndex(int index)
        {
            if (index < this.scrollItemList.Count && index >= 0)
            {
                ScrollItem item = GetItemByIndex(index);
                float num = isVertical ? -item.position.y : -item.position.x;
                SetContainerPos(num);
            }
        }

        public void OnBeginDrag(PointerEventData data)
        {
            this.autoScroll = false;
        }

        public void Update()
        {
            if (this.autoScroll)
            {
                if (this.autoScrollParam.y <= 0f)
                {
                    this.SetContainerPos(this.autoScrollParam.z);
                    this.autoScroll = false;
                }
                else
                {
                    float num = this.GetContainerPos();
                    num += this.autoScrollParam.x * Time.deltaTime;
                    this.autoScrollParam.y = this.autoScrollParam.y - Time.deltaTime;
                    if ((this.autoScrollParam.z > this.autoScrollParam.w && num > this.autoScrollParam.z) || (this.autoScrollParam.z < this.autoScrollParam.w && num < this.autoScrollParam.z))
                    {
                        num = this.autoScrollParam.z;
                        this.autoScroll = false;
                    }
                    this.SetContainerPos(num);
                }
            }
        }

        public void UpdateKeyboradHeight(float height)
        {
            GameObject gameObject = GameObject.Find("Canvas");
            GameObject gameObject2 = gameObject.transform.Find("Camera").gameObject;
            Camera component = gameObject2.GetComponent<Camera>();
            Vector3 vector = component.ScreenToWorldPoint(new Vector3(0f, height, 0f));
            Vector3 vector2 = component.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
            Transform parent = base.transform.parent;
            vector = parent.worldToLocalMatrix.MultiplyVector(vector);
            vector2 = parent.worldToLocalMatrix.MultiplyVector(vector2);
            height = Mathf.Abs(vector.y - vector2.y);
            float num = height - this.m_keyboardHeight;
            this.m_keyboardHeight = height;
            RectTransform component2 = base.GetComponent<RectTransform>();
            component2.sizeDelta = new Vector2(component2.rect.width, component2.rect.height - num);
            if (this.isVertical)
            {
                this.m_viewRect = new Vector2(this.listContainer.anchoredPosition3D.y, this.listContainer.anchoredPosition3D.y - this.GetPanelHeight());
            }
            else
            {
                this.m_viewRect = new Vector2(this.listContainer.anchoredPosition3D.x, this.listContainer.anchoredPosition3D.x + this.GetPanelHeight());
            }
            this.LocateItemPosition(-1);
        }
    }
}