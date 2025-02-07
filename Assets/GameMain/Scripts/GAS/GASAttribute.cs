using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using R3;

public class GASAttribute 
{

    public float Base;
    public float Modifier;


    private float _minValue = float.MinValue;
    private float _maxValue = float.MaxValue;

    /// <summary>
    /// originalValue, modification
    /// </summary>
    public UnityEvent<float, GASAttributeModification> OnAttributeChanged;

    //ReactiveProperty<float> FinalValue = new ReactiveProperty<float>();

    public GASAttribute()
    {
        Base = 0;
        Modifier = 0;
        OnAttributeChanged = new UnityEvent<float, GASAttributeModification>();
    }
    public GASAttribute(float inBase):this()
    {
        Base = inBase;
    }
    public GASAttribute(float inBase,float minValue,float maxValue): this(inBase)
    {
        _minValue = minValue;
        _maxValue = maxValue;
    }
    public float GetValue()
    {
        return Mathf.Clamp(Base + Modifier, _minValue, _maxValue);
    }
}
