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
            Debug.LogError("不能移除正在运行中的技能："+ action.ID);
            return;
        }

        Actions.Remove(action);
    }



    public bool StartActionByName(GameObject instigator,string actionName)
    {
        foreach (var action in Actions)
        {
            if (action.ID == actionName)
            {
                if (!action.CanStart(instigator))
                {
                    Debug.LogError("不能启动技能："+ actionName);
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
            if (action.ID == actionName)
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

    public GASAttribute GetAttribute(string attributeKey)
    {
        return AttributeSet.GetAttribute(attributeKey);
    }
    public GASAttribute AddAttribute(string attributeKey)
    {
        return AttributeSet.AddAttribute(attributeKey);
    }

    public bool ApplyAttributeChange(string attributeKey, EAttributeModifyType ModifyType, float Magnitude,
        ActionComponent target,
        ActionComponent instigator)
    {
       var modification = ScriptableObject.CreateInstance<GASAttributeModification>();

        modification.AttributeKey = attributeKey;
        modification.ModifyType = ModifyType;
        modification.Value = Magnitude;
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

        if (modification.ModifyType == EAttributeModifyType.Add)
        {
            var attribute = GetAttribute(modification.AttributeKey);
            if (attribute == null)
            {
                Debug.LogError($"attribute {modification.AttributeKey} Not found on GameObject!");
                return false;
            }
            float originalValue = attribute.GetValue();
            attribute.Modifier += modification.Value;
            attribute.OnAttributeChanged?.Invoke(originalValue, modification);
            return true;

        }
        else if (modification.ModifyType == EAttributeModifyType.Override)
        {
            var attribute = GetAttribute(modification.AttributeKey);
            if (attribute == null)
            {
                Debug.LogError($"attribute {modification.AttributeKey} Not found on GameObject!");
                return false;
            }
            float originalValue = attribute.GetValue();
            attribute.Modifier = modification.Value;
            attribute.OnAttributeChanged?.Invoke(originalValue, modification);
            return true;

        }
        else if (modification.ModifyType == EAttributeModifyType.CustomKey)
        {
            var attribute = AddAttribute(modification.AttributeKey);
            float originalValue = attribute.GetValue();
            attribute.Modifier = modification.Value;
            attribute.OnAttributeChanged?.Invoke(originalValue, modification);
            return true;
        }
        else {

            Debug.LogError("没有实现类型的数据修改:"+modification.ModifyType);
        }

        return false;
    }

}
