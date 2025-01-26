using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GASActionEffect : GASAction
{
    //TODO:����ѡ������ �޸���ʾ�ֶ�
    //˲ʱ����ӵ�ʱ��ִ��,�������Ǹ�������ִ��
    public EffectType EffectType;

    public float Duration;
    public float Period;

    public List<GASAttributeModification> AttributeModification;


    protected Timer _durationTimer;
    protected Timer _periodTimer;

    //TODO:Effect �ѵ�,������һ��������,�������˺ü���,һ�·���ü���

    public GASActionEffect()
    {
        AutoStart = true;
    }

  

    public override void StartAction(GameObject instigator)
    {
        base.StartAction(instigator);


        if (EffectType != EffectType.Instant)
        {
            if (Duration > 0)
            {
                //TODO:�������� EffectType �ж��Ƿ����ȡ����ʱ
                //���ö�ʱ��ȡ��effect
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
        //TODO:����ǿ�������������,������һ���ǲ��Ǹպÿ������һ������,Ҫִ��һ��

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
       //������Է�����Ʒ,���߸ɵ�ɶ

    }


    public virtual void ExecuteAttributeModification()
    {
        foreach (var modification in AttributeModification)
        {
            ActionComp.ApplyAttributeChange(modification);
        }
    }

}
