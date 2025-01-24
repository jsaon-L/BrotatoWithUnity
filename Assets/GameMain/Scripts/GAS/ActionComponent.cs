using BandoWare.GameplayTags;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionComponent : MonoBehaviour
{


    //Tag
    public GameplayTagContainer ActiveGameplayTags;


    //Attribute
    protected AttributeSetHealth AttributeSet;

    //Action

    public UnityEvent<ActionComponent, GASAction> OnActionStarted;
    public UnityEvent<ActionComponent, GASAction> OnActionStopped;

    protected List<GASAction> Actions;







    public void AddAction(GameObject instigator,Type actionType)
    {
        //TODO:判断type是否是 GASAction 子类
        GASAction action = Activator.CreateInstance(actionType) as GASAction;

        if (action!=null)
        {
            action.Initialize(this);
            Actions.Add(action);
            if (action.IsAutoStart()&&action.CanStart(instigator))
            {
                action.StartAction(instigator);
            }
        }

    }
    public void RemoveAction(GASAction action)
    {
        //TODO:

        if (action == null)
        {
            return;
        }

        if (action.IsRunning())
        {
            Debug.LogError("不能移除正在运行中的技能："+ action.ActivationTag.Name);
            return;
        }

        Actions.Remove(action);
    }



    public bool StartActionByName(GameObject instigator,GameplayTag actionName)
    {
        foreach (var action in Actions)
        {
            if (action.ActivationTag == actionName)
            {
                if (!action.CanStart(instigator))
                {
                    Debug.LogError("不能启动技能："+ actionName.Name);
                    continue;
                }

               
                action.StartAction(instigator);

                return true;
            }
            
        }
        return false;
    }


    public bool StopActionByName(GameObject instigator, GameplayTag actionName)
    {
        foreach (var action in Actions)
        {
            if (action.ActivationTag == actionName)
            {
                if (action.IsRunning())
                {
                    action.StopAction(instigator);
                    return true;
                }

            }

        }
        return false;
    }

    public void StopAllActions()
    {
        foreach (var action in Actions)
        {
            if (action.IsRunning())
            {
                action.StopAction(gameObject);
            }
        }
    }

}
