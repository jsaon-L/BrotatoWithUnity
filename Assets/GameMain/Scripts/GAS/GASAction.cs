using BandoWare.GameplayTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GASAction 
{
    protected ActionComponent ActionComp;
    protected GameObject _instigator;
    protected bool _isRunning;
    protected float TimeStarted;
    


    public bool AutoStart;

    //就是这个action的名字
    public GameplayTag ActivationTag;

    //激活时添加到所属角色的标签，动作停止时移除。
    public GameplayTagContainer GrantsTags;
    //仅当所属角色未应用这些标签中的任何一个时，动作才能开始。
    public GameplayTagContainer BlockedTags;

    public void Initialize(ActionComponent owner)
    {
        ActionComp = owner;
    }

    public bool IsRunning()
    {
        return _isRunning;
    }
    public bool IsAutoStart()
    {
        return AutoStart;
    }
    public bool CanStart(GameObject instigator)
    {
        if (_isRunning)
        {
            return false;
        }

        //判断当前主角是否拥有某些tag，如果有这些tag这个技能就不生效
        if (ActionComp.ActiveGameplayTags.HasAny(BlockedTags))
        {
            return false;
        }

        return true;
    }

    public void StartAction(GameObject instigator)
    {
        //TODO: UE使用函数 Comp->ActiveGameplayTags.AppendTags(GrantsTags); 需要确认他与AddTags 有什么区别
        
        ActionComp.ActiveGameplayTags.AddTags(GrantsTags);

        _isRunning = true;
        _instigator = instigator;

        TimeStarted = Time.time;
        ActionComp.OnActionStarted.Invoke(ActionComp, this); 

    }
    public void StopAction(GameObject instigator)
    {
        ActionComp.ActiveGameplayTags.RemoveTags(GrantsTags);

        _isRunning = false;
        _instigator = instigator;

        ActionComp.OnActionStopped.Invoke(ActionComp, this);

    }

}
