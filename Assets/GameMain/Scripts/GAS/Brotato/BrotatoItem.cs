using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Brotato/Item")]
public class BrotatoItem :ScriptableObject
{
    public string ID;
    public bool UnlockedByDefault = false;
    public Sprite Icon;
    public string ItemName;
    public Tier Tier = Tier.COMMON;
    /// <summary>
    /// 价格
    /// </summary>
    public int Value = 1;
    public GASActionEffect Effect;
    public string TrackingText;
    public Category Category = Category.ITEM;

    /// <summary>
    /// 最大获取数量
    /// TODO:实现物品最多数量功能
    /// </summary>
    public int MaxNB = -1;

    //TODO:获得物品后,挂件显示在玩家身上的图片
    //public item_appearances;

    public List<string> Tags = new List<string>();

    public string GetEffectsText()
    {
        //TODO:实现获取所有效果描述文字
        return "";
    }

}
