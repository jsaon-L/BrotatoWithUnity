using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    /// <summary>
    /// ˲ʱ(Instant)
    /// </summary>
    [LabelText("˲ʱ(Instant)", SdfIconType.LightningCharge)]
    Instant = 1,
    /// <summary>
    /// ����(Infinite)
    /// </summary>
    [LabelText("����(Infinite)", SdfIconType.Infinity)]
    Infinite,
    /// <summary>
    /// ��ʱ(Duration)
    /// </summary>
    [LabelText("��ʱ(Duration)", SdfIconType.HourglassSplit)]
    Duration
}