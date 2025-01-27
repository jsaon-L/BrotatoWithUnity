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
    protected AttributeSet AttributeSet = new AttributeSet();

    //Action

    public UnityEvent<ActionComponent, GASAction> OnActionStarted;
    public UnityEvent<ActionComponent, GASAction> OnActionStopped;
    [SerializeField]
    protected List<GASAction> Actions;



    public void AddAction(GameObject instigator, GASAction action)
    {
        if (action!=null)
        {
            action.Initialize(this);
            Actions.Add(action);

            Debug.Log("AddAction");
            Debug.Log("IsAutoStart" + action.IsAutoStart());
            Debug.Log("CanStart" + action.CanStart(instigator));

            if (action.IsAutoStart() && action.CanStart(instigator))
            {
                Debug.Log("AddAction start");
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

    public GASAttribute GetAttribute(GameplayTag attributeTag)
    {
        return AttributeSet.GetAttribute(attributeTag);
    }
    public void AddAttribute(GameplayTag attributeTag)
    {
        AttributeSet.AddAttribute(attributeTag);
    }

    public bool ApplyAttributeChange(GameplayTag attributeTag, EAttributeModifyType ModifyType, float Magnitude,
        ActionComponent target,
        ActionComponent instigator)
    {
       var modification = ScriptableObject.CreateInstance<GASAttributeModification>();

        modification.AttributeTag = attributeTag;
        modification.ModifyType = ModifyType;
        modification.Magnitude = Magnitude;
        modification.Target = target;
        modification.Instigator = instigator;
        return ApplyAttributeChange(modification);

    }
    public bool ApplyAttributeChange(GASAttributeModification modification)
    {
        if (modification == null)
        {
            Debug.LogError($"modification is Null!");
            return false;
        }

        var attribute = GetAttribute(modification.AttributeTag);
        if(attribute == null)
        {
            Debug.LogError($"attribute {modification.AttributeTag.Name} Not found on GameObject!");
            return false;
        }

        Debug.Log("ApplyAttributeChange");

        float originalValue = attribute.GetValue();

        switch (modification.ModifyType)
        {
            case EAttributeModifyType.AddBase:
                attribute.Base += modification.Magnitude;
                break;
            case EAttributeModifyType.MultipliBase:
                attribute.Base *= modification.Magnitude;
                break;
            case EAttributeModifyType.OverrideBase:
                attribute.Base = modification.Magnitude;
                break;
            default:
                break;
        }

        if (originalValue != attribute.GetValue()) 
        {
            attribute.OnAttributeChanged?.Invoke(originalValue, modification);
            return true;
        }

        return false;
    }

}
