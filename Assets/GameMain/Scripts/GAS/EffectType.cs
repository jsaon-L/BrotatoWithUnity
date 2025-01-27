using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    /// <summary>
    /// 瞬时(Instant) 就是没有周期执行,只启动一次
    /// </summary>
    [LabelText("瞬时", SdfIconType.LightningCharge)]
    Instant = 1,
    /// <summary>
    /// 永久(Infinite)
    /// </summary>
    [LabelText("永久", SdfIconType.Infinity)]
    Infinite,
    /// <summary>
    /// 限时(Duration)
    /// </summary>
    [LabelText("限时", SdfIconType.HourglassSplit)]
    Duration
}