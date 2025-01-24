using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GASAttribute 
{

    public float Base;
    public float Modifier;

    public UnityEvent<float, GASAttributeModification> OnAttributeChanged;

    public GASAttribute()
    {
        Base = 0;
        Modifier = 0;
    }
    public GASAttribute(float inBase)
    {
        Base = inBase;
        Modifier = 0;
    }
    public float GetValue()
    {
        return Mathf.Max(Base+Modifier, 0);
    }
}
