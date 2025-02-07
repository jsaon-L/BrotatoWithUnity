using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Effect",menuName = "GAS/Effect")]
public class GASActionEffect : GASAction
{
    //TODO:根据选择类型 修改显示字段
    //瞬时是添加的时候执行,其他的是根据周期执行
    [Title("Effect")]
    [EnumToggleButtons]
    public EffectType EffectType = EffectType.Instant;


    [ShowIf("EffectType", EffectType.Duration)]
    public float Duration;
    [HideIf("EffectType", EffectType.Instant)]
    public float Period;

    public List<GASAttributeModification> AttributeModification;
    public List<AttributeModificationData> AttributeModificationData;


    protected Timer _durationTimer;
    protected Timer _periodTimer;

    //TODO:Effect 堆叠,比如我一个发射物,等我攒了好几个,一下发射好几个

    public GASActionEffect()
    {
        AutoStart = true;
    }

  


    public override void StartAction(GameObject instigator)
    {
        base.StartAction(instigator);

        Debug.Log("StartAction");

        if (EffectType != EffectType.Instant)
        {
            if (EffectType == EffectType.Duration && Duration > 0)
            {
                //TODO:根据类型 EffectType 判断是否进行取消计时
                //设置定时器取消effect
                _durationTimer = Timer.Register(Duration, () => { StopAction(instigator); });
            }

            if (Period > 0)
            {
                _periodTimer = Timer.Register(Period, () => { ExecutePeriodicEffect(instigator); }, isLooped: true);
            }
        }
        else
        {
            ExecutePeriodicEffect(instigator);
        }

        

    }
    public override void StopAction(GameObject instigator)
    {
        base.StopAction(instigator);
        //TODO:如果是可以是周期类型,这里检查一下是不是刚好卡在最后一次周期,要执行一下

        if (
            EffectType != EffectType.Instant
            && _periodTimer != null
            && !_periodTimer.isDone
            && Mathf.Approximately(_periodTimer.GetTimeRemaining(), 0)
            )
        {
            ExecutePeriodicEffect(instigator);
        }

        _durationTimer?.Stop();
        _periodTimer?.Stop();

       ActionComp.RemoveAction(this);
    }

    public virtual void ExecutePeriodicEffect(GameObject instigator)
    {
        Debug.Log("ExecutePeriodicEffect");
        //这里可以发射物品,或者干点啥

        ExecuteAttributeModification();
    }


    public virtual void ExecuteAttributeModification()
    {
        foreach (var modification in AttributeModification)
        {
            ActionComp.ApplyAttributeChange(modification);
        }

        foreach (var item in AttributeModificationData)
        {
            ActionComp.ApplyAttributeChange(item.AttributeKey, item.ModifyType, item.Value,null,null);
        }
    }

}


//TODO:效果