using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    /// <summary>
    /// 瞬时(Instant)
    /// </summary>
    [LabelText("瞬时(Instant)", SdfIconType.LightningCharge)]
    Instant = 1,
    /// <summary>
    /// 永久(Infinite)
    /// </summary>
    [LabelText("永久(Infinite)", SdfIconType.Infinity)]
    Infinite,
    /// <summary>
    /// 限时(Duration)
    /// </summary>
    [LabelText("限时(Duration)", SdfIconType.HourglassSplit)]
    Duration
}