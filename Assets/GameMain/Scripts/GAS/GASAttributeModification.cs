using BandoWare.GameplayTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttributeModification",menuName = "GAS/AttributeModification")]
public class GASAttributeModification : ScriptableObject
{

    public string AttributeKey;
    public EAttributeModifyType ModifyType;
    public float Value;
    public EffectSign EffectSign = EffectSign.FROM_VALUE;

    /// <summary>
    /// 目标
    /// </summary>
    /// 
    [HideInInspector]
    public ActionComponent Target;
    /// <summary>
    /// 发起者
    /// </summary>
    /// 
    [HideInInspector]
    public ActionComponent Instigator;


    public GASAttributeModification(
        string attributeKey,
        float magnitude, 
        ActionComponent target, 
        ActionComponent instigator,
        EAttributeModifyType modifyType
        )
    {
        AttributeKey = attributeKey;
        Value = magnitude;
        ModifyType = modifyType;
        Target = target;
        Instigator = instigator;
        
    }


    //TODO:添加获取标签函数
}

[System.Serializable]
public class AttributeModificationData
{
    public string AttributeKey;
    public EAttributeModifyType ModifyType;
    public float Value;
    public EffectSign EffectSign = EffectSign.FROM_VALUE;
}