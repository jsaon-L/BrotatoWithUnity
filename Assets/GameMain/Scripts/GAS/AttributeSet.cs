using BandoWare.GameplayTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeSet
{

    protected Dictionary<GameplayTag,GASAttribute> _attributes = new Dictionary<GameplayTag, GASAttribute>();

    public virtual GASAttribute GetAttribute(GameplayTag attributeTag)
    {
        if (_attributes.TryGetValue(attributeTag, out var attr))
        {
            return attr;
        }
        return null;
    }

    public virtual void AddAttribute(GameplayTag attributeTag)
    {

        _attributes[attributeTag] = new GASAttribute();

    }

}

