using BandoWare.GameplayTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GASAttributeModification 
{

    public GameplayTag AttributeTag;
    public EAttributeModifyType ModifyType;
    public float Magnitude;

    /// <summary>
    /// 目标
    /// </summary>
    public ActionComponent Target;
    /// <summary>
    /// 发起者
    /// </summary>
    public ActionComponent Instigator;


    public GASAttributeModification(
        GameplayTag attributeTag,
        float magnitude, 
        ActionComponent target, 
        ActionComponent instigator,
        EAttributeModifyType modifyType
        )
    {
        AttributeTag = attributeTag;
        Magnitude = magnitude;
        Target = target;
        Instigator = instigator;
        ModifyType = modifyType;
    }

}
