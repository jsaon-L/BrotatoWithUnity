
using Sirenix.OdinInspector;

/// <summary>
/// 用来表示这个物品是正面效果还是负面效果
/// </summary>
public enum EffectSign
{
    [LabelText("积极")]
    POSITIVE,
    [LabelText("消极")]
    NEGATIVE,
    [LabelText("中立")]
    NEUTRAL,
    [LabelText("从值推导")]
    FROM_VALUE,
    [LabelText("从参数推导")]
    FROM_ARG
}