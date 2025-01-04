using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class PageView : ScrollBaseView
    {
        public enum ListViewLayoutType
        {
            TopToBottom,
            BottomToTop,
            LeftToRight,
            RightToLeft,
        }

        public class ListItem
        {
            public GameObject go;

            public string tag;

            public string prefabName;

            public int index;

            public float startPos;

            public float endPos;

            public bool isInit;

            public ListItem()
            {
                this.go = null;
                this.prefabName = string.Empty;
                this.tag = string.Empty;
                this.index = 0;
                this.startPos = 0f;
                this.endPos = 0f;
                this.isInit = false;
            }

            public bool HasGameObject()
            {
                return this.go != null;
            }

            public void ExportItem()
            {
                if (!this.go)
                {
                    return;
                }
            }
        }

        public class FuncTab
        {
            public delegate float ReturnFloat(PageView.ListItem item);
            public delegate string ReturnString(PageView.ListItem item);
            public FuncTab.ReturnFloat GetItemSize;
            public FuncTab.ReturnString GetItemTag;
            public FuncTab.ReturnString GetItemPrefabName;
            public Action<PageView.ListItem> ItemEnter;
            public Action<PageView.ListItem> ItemRemove;
        }

        public PageView.ListViewLayoutType layoutType;

        public List<string> ItemPrefabDataList = new List<string>();

        public RectTransform listContainer;

        public float offset;

        public float spacing;

        public float cacheSize;

        private float autoScrollTime = 0.2f;

        private bool isVertical;

        private Dictionary<string, float> prefabSizeMap = new Dictionary<string, float>();

        private string defaultPrefabName;

        private List<PageView.ListItem> itemList = new List<PageView.ListItem>();

        private float totalSize;

        private float viewSize;

        private float viewStartPos;

        private float viewEndPos;

        private float containerLastPos;

        private ScrollRect parentScrollRect;

        private bool parentScrollEnable;

        private bool autoScroll;

        private Vector4 autoScrollParam;

        private static Vector2 TopToDownPreset = new Vector2(0.5f, 1);
        private static Vector2 DownToTopPreset = new Vector2(0.5f, 0);
        private static Vector2 LeftToRightPreset = new Vector2(0, 0.5f);
        private static Vector2 RightToLeftPreset = new Vector2(1, 0.5f);
        private static Vector2 Pivot = new Vector2(0.5f, 0.5f);
        private Dictionary<string, GameObject> assetObjectDic;
        private bool m_isPositive;      //是否是正向
        private FuncTab m_funcTab;

        private bool isDrag;
        private bool stopMove;
        public float smooting = 4;                      //滑动速度  
        private float startTime;
        private ScrollRect rect;                        //滑动组件  
        private List<float> posList = new List<float>();//求出每页的临界角，页索引从0开始  
        private float startDragHorizontal;
        private float targethorizontal = 0;             //滑动的起始坐标  
        public float sensitivity = 0.2f;
        private Dictionary<string, List<GameObject>> mItemPoolDict = new Dictionary<string, List<GameObject>>();

        private bool m_isInitDragHandle;

        void Awake()
        {
            rect = transform.GetComponent<ScrollRect>();
        }

        private float GetItemSize(PageView.ListItem item)
        {
            if (m_funcTab != null && m_funcTab.GetItemSize != null)
            {
                return m_funcTab.GetItemSize(item);
            }
            return this.prefabSizeMap[item.prefabName];
        }

        private string GetItemTag(PageView.ListItem item)
        {
            if (m_funcTab != null && m_funcTab.GetItemTag != null)
            {
                return m_funcTab.GetItemTag(item);
            }
            return string.Empty;
        }

        private string GetItemPrefabName(PageView.ListItem item)
        {
            if (m_funcTab != null && m_funcTab.GetItemPrefabName != null)
            {
                return m_funcTab.GetItemPrefabName(item);
            }
            return this.defaultPrefabName;
        }

        private void SetContainerSize(float size)
        {
            this.listContainer.sizeDelta = ((!this.isVertical) ? new Vector3(size, this.listContainer.rect.height) : new Vector3(this.listContainer.rect.width, size));
        }

        private float GetContainerSize()
        {
            return (!this.isVertical) ? this.listContainer.rect.width : this.listContainer.rect.height;
        }

        public void SetContainerPos(float pos)
        {
            this.listContainer.anchoredPosition = ((!this.isVertical) ? new Vector2(pos, 0f) : new Vector2(0f, pos));
        }

        public float GetContainerPos()
        {
            return (!this.isVertical) ? this.listContainer.anchoredPosition.x : this.listContainer.anchoredPosition.y;
        }

        private void SetViewRect(float viewStart)
        {
            this.viewStartPos = this.m_isPositive ? (-viewStart - this.cacheSize) : (-viewStart + this.cacheSize);
            this.viewEndPos = this.m_isPositive ? (this.viewStartPos + this.viewSize + 2f * this.cacheSize) : (this.viewStartPos - this.viewSize - 2f * this.cacheSize);
        }

        private bool ItemVisible(PageView.ListItem item)
        {
            if (m_isPositive)
            {
                if (item.endPos < this.viewStartPos || item.startPos > this.viewEndPos)
                {
                    return false;
                }
            }
            else
            {
                if (item.endPos > this.viewStartPos || item.startPos < this.viewEndPos)
                {
                    return false;
                }
            }
            return true;
        }

        private bool ShowItem(PageView.ListItem item, bool force = false)
        {
            if (this.ItemVisible(item))
            {
                this.OnItemEnter(item, force);
                return true;
            }
            this.OnItemLeave(item);
            return false;
        }

        public void SetInitData(Dictionary<string, GameObject> prefabDic, FuncTab funcObj)
        {
            if (!m_isInitDragHandle)
            {
                m_isInitDragHandle = true;
                onBeginDrag += BeginDrag;
                onDrag += Draging;
                onEndDrag += EndDrag;
            }
            this.isVertical = (this.layoutType == PageView.ListViewLayoutType.TopToBottom || this.layoutType == PageView.ListViewLayoutType.BottomToTop);
            if (this.layoutType == PageView.ListViewLayoutType.TopToBottom || this.layoutType == PageView.ListViewLayoutType.RightToLeft)
            {
                this.m_isPositive = false;
            }
            else
            {
                this.m_isPositive = true;
            }
            this.viewSize = ((!this.isVertical) ? base.GetComponent<RectTransform>().rect.width : base.GetComponent<RectTransform>().rect.height);
            string text = "";
            assetObjectDic = new Dictionary<string, GameObject>();

            for (int i = 0; i < ItemPrefabDataList.Count; i++)
            {
                if (prefabDic.ContainsKey(ItemPrefabDataList[i]))
                {
                    text = ItemPrefabDataList[i];
                    assetObjectDic[text] = prefabDic[ItemPrefabDataList[i]];
                    Rect rect = prefabDic[ItemPrefabDataList[i]].GetComponent<RectTransform>().rect;
                    this.prefabSizeMap[text] = (!this.isVertical) ? rect.width : rect.height;
                    this.mItemPoolDict[text] = new List<GameObject>();
                    this.defaultPrefabName = text;
                }
                else
                {
                    Debug.LogError(string.Format("List not find Item Prefab:{0}", ItemPrefabDataList[i]));
                }
            }

            if (this.listContainer == null)
            {
                Debug.LogError("Please set Container");
                return;
            }
            m_funcTab = funcObj;
            ScrollRect component = base.GetComponent<ScrollRect>();
            if (component != null)
            {
                component.vertical = this.isVertical;
                component.horizontal = !this.isVertical;
                component.onValueChanged.RemoveAllListeners();
                component.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnValueChanged));
            }
            switch (this.layoutType)
            {
                case PageView.ListViewLayoutType.BottomToTop:
                    listContainer.pivot = DownToTopPreset;
                    listContainer.anchorMax = DownToTopPreset;
                    listContainer.anchorMin = DownToTopPreset;
                    break;
                case PageView.ListViewLayoutType.TopToBottom:
                    listContainer.pivot = TopToDownPreset;
                    listContainer.anchorMax = TopToDownPreset;
                    listContainer.anchorMin = TopToDownPreset;
                    break;
                case PageView.ListViewLayoutType.LeftToRight:
                    listContainer.pivot = LeftToRightPreset;
                    listContainer.anchorMax = LeftToRightPreset;
                    listContainer.anchorMin = LeftToRightPreset;
                    break;
                case PageView.ListViewLayoutType.RightToLeft:
                    listContainer.pivot = RightToLeftPreset;
                    listContainer.anchorMax = RightToLeftPreset;
                    listContainer.anchorMin = RightToLeftPreset;
                    break;
                default: break;
            }
        }

        public void SetParent(ScrollRect parent)
        {
            this.parentScrollRect = parent;
        }

        public void RemoveAt(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }
            PageView.ListItem listItem = this.itemList[index];
            float num = Mathf.Abs(listItem.endPos - listItem.startPos) + this.spacing;
            this.totalSize -= num;
            this.SetContainerSize(this.totalSize);
            this.OnItemLeave(listItem);
            for (int i = index + 1; i < this.itemList.Count; i++)
            {
                PageView.ListItem listItem2 = this.itemList[i];
                listItem2.index--;
                listItem2.startPos = this.m_isPositive ? (listItem2.startPos - num) : (listItem2.startPos + num);
                listItem2.endPos = this.m_isPositive ? (listItem2.endPos - num) : (listItem2.endPos + num);
                if (listItem2.go != null)
                {
                    this.UpdateItemGameObjectPosition(listItem2);
                }
                this.ShowItem(listItem2, false);
            }
            this.itemList.RemoveAt(index);
        }

        public void ForceRefresh()
        {
            for (int i = 0; i < this.itemList.Count; i++)
            {
                this.ShowItem(this.itemList[i], true);
            }
        }

        public void RefreshItem(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }
            PageView.ListItem listItem = this.itemList[index];
            this.ShowItem(listItem, true);
            float num = Mathf.Abs(listItem.endPos - listItem.startPos);
            float itemSize = this.GetItemSize(listItem);
            listItem.endPos = this.m_isPositive ? (listItem.startPos + itemSize) : (listItem.startPos - itemSize);
            float num2 = itemSize - num;
            this.totalSize += num2;
            this.SetContainerSize(this.totalSize);
            for (int i = index + 1; i < this.itemList.Count; i++)
            {
                PageView.ListItem listItem2 = this.itemList[i];
                listItem2.startPos = this.m_isPositive ? (listItem2.startPos + num2) : (listItem2.startPos - num2);
                listItem2.endPos = this.m_isPositive ? (listItem2.endPos + num2) : (listItem2.endPos - num2);
                if (listItem2.go != null)
                {
                    this.UpdateItemGameObjectPosition(listItem2);
                }
                this.ShowItem(listItem2, false);
            }
        }

        public void ClearPostion()
        {
            this.containerLastPos = 0f;
        }

        public void FillContent(int listLength)
        {
            stopMove = true;
            int count = this.itemList.Count;
            this.totalSize = ((!this.isVertical) ? this.offset : (-this.offset));
            for (int i = 0; i < listLength; i++)
            {
                PageView.ListItem listItem;
                if (i < count)
                {
                    listItem = this.itemList[i];
                    this.OnItemLeave(listItem);
                }
                else
                {
                    listItem = new PageView.ListItem();
                    this.itemList.Add(listItem);
                }
                listItem.index = i;
                listItem.prefabName = this.GetItemPrefabName(listItem);
                listItem.tag = this.GetItemTag(listItem);
                float itemSize = this.GetItemSize(listItem);
                if (this.m_isPositive)
                {
                    listItem.startPos = this.totalSize;
                    listItem.endPos = this.totalSize + itemSize;
                    this.totalSize += (itemSize + this.spacing);
                }
                else
                {
                    listItem.startPos = this.totalSize;
                    listItem.endPos = this.totalSize - itemSize;
                    this.totalSize += (-(itemSize + this.spacing));
                }
            }
            for (int j = count - 1; j >= listLength; j--)
            {
                this.OnItemLeave(this.itemList[j]);
                this.itemList.RemoveAt(j);
            }
            this.totalSize = Mathf.Abs(this.totalSize);
            this.SetContainerSize(this.totalSize);
            this.SetContainerPos(this.containerLastPos);
            this.SetHorzontalPosition(listLength);
            this.ShowContentAt(this.containerLastPos);
        }

        private void SetHorzontalPosition(int listLength)
        {
            if (rect==null)
            {
                rect = transform.GetComponent<ScrollRect>();
            }
            posList.Clear();

            float horizontalLength = 0;
            if (this.isVertical)
            {
                horizontalLength = rect.content.rect.height - GetComponent<RectTransform>().rect.height;
            }
            else
            {
                horizontalLength = rect.content.rect.width - GetComponent<RectTransform>().rect.width;
            }

            posList.Add(0);
            for (int i = 1; i < listLength - 1; i++)
            {
                if (isVertical)
                {
                    posList.Add(GetComponent<RectTransform>().rect.height * i / horizontalLength);
                }
                else
                {
                    posList.Add(GetComponent<RectTransform>().rect.width * i / horizontalLength);
                }
            }
            posList.Add(1);
        }

        private void UpdateItemGameObjectPosition(PageView.ListItem item)
        {
            Vector3 v = (!this.isVertical) ? new Vector2(item.startPos, 0f) : new Vector2(0f, item.startPos);
            RectTransform component = item.go.GetComponent<RectTransform>();
            component.anchoredPosition = v;
            component.localScale = Vector3.one;
        }

        private void OnItemEnter(PageView.ListItem item, bool force = false)
        {
            if (item.go != null && !force)
            {
                return;
            }
            if (item.go == null)
            {
                item.go = GetRecycleItem(item.prefabName);
                if (item.go == null)
                {
                    item.go = GameObject.Instantiate(assetObjectDic[item.prefabName]);
                    item.isInit = false;
                }
                else
                {
                    item.isInit = true;
                }
                item.go.transform.SetParent(this.listContainer);
                if (!item.go.activeSelf)
                {
                    item.go.SetActive(true);
                }
                RectTransform rect = item.go.GetComponent<RectTransform>();
                switch (this.layoutType)
                {
                    case ListViewLayoutType.TopToBottom:
                        rect.pivot = TopToDownPreset;
                        rect.anchorMin = TopToDownPreset;
                        rect.anchorMax = TopToDownPreset;
                        break;
                    case ListViewLayoutType.BottomToTop:
                        rect.pivot = DownToTopPreset;
                        rect.anchorMin = DownToTopPreset;
                        rect.anchorMax = DownToTopPreset;
                        break;
                    case ListViewLayoutType.LeftToRight:
                        rect.pivot = LeftToRightPreset;
                        rect.anchorMin = LeftToRightPreset;
                        rect.anchorMax = LeftToRightPreset;
                        break;
                    case ListViewLayoutType.RightToLeft:
                        rect.pivot = RightToLeftPreset;
                        rect.anchorMin = RightToLeftPreset;
                        rect.anchorMax = RightToLeftPreset;
                        break;
                    default: break;
                }
            }
            this.UpdateItemGameObjectPosition(item);
            m_funcTab.ItemEnter(item);
        }

        private void OnItemLeave(PageView.ListItem item)
        {
            if (item.go == null)
            {
                return;
            }
            //CoreUtils.assetService.Destroy(item.go);
            //item.go = null;
            RecycleItem(item);
        }

        public void RecycleItem(PageView.ListItem item)
        {
            item.go.SetActive(false);
            this.mItemPoolDict[item.prefabName].Add(item.go);
            item.go = null;
        }

        public GameObject GetRecycleItem(string prefabName)
        {
            if (mItemPoolDict[prefabName].Count > 0)
            {
                int count = mItemPoolDict[prefabName].Count - 1;
                GameObject go = mItemPoolDict[prefabName][mItemPoolDict[prefabName].Count - 1];
                mItemPoolDict[prefabName].RemoveAt(count);
                go.transform.localScale = Vector3.zero;
                go.SetActive(true);
                return go;
            }
            return null;
        }

        public void ShowContentAt(float pos)
        {
            this.SetViewRect(pos);
            for (int i = 0; i < this.itemList.Count; i++)
            {
                PageView.ListItem item = this.itemList[i];
                this.ShowItem(item, false);
            }
            this.containerLastPos = pos;
        }

        public void OnValueChanged(Vector2 vec2)
        {
            this.ShowContentAt(this.GetContainerPos());
        }

        public void Clear()
        {
            for (int i = 0; i < this.itemList.Count; i++)
            {
                PageView.ListItem listItem = this.itemList[i];
                if (listItem.go != null)
                {
                    GameObject.Destroy(listItem.go);
                    listItem.go = null;
                }
            }
            this.prefabSizeMap.Clear();
            this.containerLastPos = 0f;
            this.itemList.Clear();
            this.totalSize = 0f;
            this.SetContainerSize(0f);
            ScrollRect component = base.GetComponent<ScrollRect>();
            if (component != null)
            {
                component.StopMovement();
            }
            this.posList.Clear();
        }

        private void OnDestroy()
        {
            this.Clear();
        }

        public PageView.ListItem GetItemByIndex(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return null;
            }
            return this.itemList[index];
        }

        public PageView.ListItem GetItemByTag(string tag)
        {
            for (int i = 0; i < this.itemList.Count; i++)
            {
                if (this.itemList[i] != null && this.itemList[i].tag == tag)
                {
                    return this.itemList[i];
                }
            }
            return null;
        }

        //滚动到目标位置
        public void ScrollPanelToItemIndex(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }
            float num = -this.itemList[index].startPos;
            float containerPos = this.GetContainerPos();
            this.autoScroll = true;
            this.autoScrollParam = new Vector4((num - containerPos) / this.autoScrollTime, this.autoScrollTime, num, containerPos);
        }

        //直接切换到目标位置
        public void MovePanelToItemIndex(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }
            float num = -this.itemList[index].startPos;
            if (!this.isVertical)
            {
                num = Mathf.Clamp(num, -this.totalSize + this.viewSize, 0f);
            }
            else
            {
                num = Mathf.Clamp(num, 0f, this.totalSize - this.viewSize);
            }
            this.SetContainerPos(num);
            this.ShowContentAt(num);
        }

        public void ScrollToPos(float dest)
        {
            float containerPos = this.GetContainerPos();
            this.autoScroll = true;
            this.autoScrollParam = new Vector4((dest - containerPos) / this.autoScrollTime, this.autoScrollTime, dest, containerPos);
        }

        public void BeginDrag(PointerEventData eventData)
        {
            this.autoScroll = false;
            if ((!this.isVertical && Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y)) || 
                (this.isVertical && Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y)))
            {
                this.parentScrollEnable = false;
            }
            else
            {
                this.parentScrollEnable = true;
            }
            if (this.parentScrollRect != null && this.parentScrollEnable)
            {
                this.parentScrollRect.OnBeginDrag(eventData);
                ScrollRect component = base.GetComponent<ScrollRect>();
                if (component != null)
                {
                    component.vertical = false;
                    component.horizontal = false;
                }
            }
            isDrag = true;
            startDragHorizontal = this.isVertical ? rect.verticalNormalizedPosition : rect.horizontalNormalizedPosition;
        }

        public void Draging(PointerEventData eventData)
        {
            if (this.parentScrollRect != null && this.parentScrollEnable)
            {
                this.parentScrollRect.OnDrag(eventData);
            }
        }

        public void EndDrag(PointerEventData eventData)
        {
            if (this.parentScrollRect != null && this.parentScrollEnable)
            {
                this.parentScrollRect.OnEndDrag(eventData);
                ScrollRect component = base.GetComponent<ScrollRect>();
                if (component != null)
                {
                    component.vertical = this.isVertical;
                    component.horizontal = !this.isVertical;
                }
            }

            int index = GetDragIndex();

            targethorizontal = posList[index]; //设置当前坐标，更新函数进行插值  
            startTime = 0;
            isDrag = false;
            stopMove = false;
        }

        private int GetDragIndex()
        {
            float posX = this.isVertical ? rect.verticalNormalizedPosition : rect.horizontalNormalizedPosition;
            float unitLength = posList[1] - posList[0];
            float offsetX = unitLength * sensitivity;

            float initVal1 = posX / unitLength;
            int endIndex = (int)Mathf.Floor(initVal1);

            if (endIndex < 0)
            {
                endIndex = 0;
            }
            else if (endIndex >= posList.Count)
            {
                endIndex = posList.Count-1;
            }

            int index = 0;
            if (posX - startDragHorizontal >= 0)
            {
                int num = endIndex;
                if ((posX - posList[num]) >= offsetX)
                {
                    index = num + 1;
                }
                else
                {
                    index = num;
                }
                if (index >= posList.Count)
                {
                    index = posList.Count - 1;
                }
            }
            else
            {
                offsetX = unitLength - offsetX;
                int num = endIndex;
                if ((posX - posList[num]) <= offsetX)
                {
                    index = num;
                }
                else
                {
                    index = num + 1;
                    if (index >= posList.Count)
                    {
                        index = posList.Count - 1;
                    }
                }
            }

            int startIndex = (int)Math.Round(startDragHorizontal / unitLength);
            //如果翻页超过2个 则强制缩减到1个
            int diffIndex = Mathf.Abs(index - startIndex);
            if (diffIndex >= 2)
            {
                if (index > startIndex)
                {
                    index = startIndex + 1;
                }
                else
                {
                    index = startIndex - 1;
                }
            }
            if (index >= posList.Count)
            {
                index = posList.Count - 1;
            }
            else if (index < 0)
            {
                index = 0;
            }
            return index;
        }

        public void Update()
        {
            if (this.autoScroll)
            {
                if (this.autoScrollParam.y <= 0f)
                {
                    this.SetContainerPos(this.autoScrollParam.z);
                    this.ShowContentAt(this.autoScrollParam.z);
                    this.autoScroll = false;
                }
                else
                {
                    float num = this.GetContainerPos();
                    num += this.autoScrollParam.x * Time.deltaTime;
                    this.autoScrollParam.y = this.autoScrollParam.y - Time.deltaTime;
                    if ((this.autoScrollParam.z > this.autoScrollParam.w && num > this.autoScrollParam.z) ||
                        (this.autoScrollParam.z < this.autoScrollParam.w && num < this.autoScrollParam.z))
                    {
                        num = autoScrollParam.z;
                        autoScroll = false;
                    }
                    this.SetContainerPos(num);
                    this.ShowContentAt(num);
                }
            }
            else
            {
                if (!isDrag && !stopMove)
                {
                    startTime += Time.deltaTime;
                    float t = startTime * smooting;
                    if (isVertical)
                    {
                        rect.verticalNormalizedPosition = Mathf.Lerp(rect.verticalNormalizedPosition, targethorizontal, t);
                    }
                    else
                    {
                        rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, t);
                    }
                    if (t >= 1)
                        stopMove = true;
                }
            }
        }
       
    }
}