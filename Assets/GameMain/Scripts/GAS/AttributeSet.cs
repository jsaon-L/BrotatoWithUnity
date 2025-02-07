using BandoWare.GameplayTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeSet
{

    protected Dictionary<string,GASAttribute> _attributes = new Dictionary<string, GASAttribute>();

    public virtual GASAttribute GetAttribute(string attributeKey)
    {
        if (_attributes.TryGetValue(attributeKey, out var attr))
        {
            return attr;
        }
        return null;
    }

    public virtual GASAttribute AddAttribute(string attributeKey)
    {
        if (_attributes.ContainsKey(attributeKey))
        {
            return _attributes[attributeKey];
        }

        return _attributes[attributeKey] = new GASAttribute();

    }

}

