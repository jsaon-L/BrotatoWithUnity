using GameFramework;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using GameFramework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;


/// <summary>
/// 界面实例对象。
/// </summary>
public sealed class SubUIFormInstanceObject : ObjectBase
{
    private object m_UIFormAsset;
    private GameObject _ObjectPoolRoot;

    public static SubUIFormInstanceObject Create(string name, object uiFormAsset, object uiFormInstance, GameObject objectPoolRoot)
    {
        if (uiFormAsset == null)
        {
            throw new GameFrameworkException("UI form asset is invalid.");
        }

        SubUIFormInstanceObject uiFormInstanceObject = ReferencePool.Acquire<SubUIFormInstanceObject>();
        uiFormInstanceObject.Initialize(name, uiFormInstance);
        uiFormInstanceObject.m_UIFormAsset = uiFormAsset;
        uiFormInstanceObject._ObjectPoolRoot = objectPoolRoot;
        return uiFormInstanceObject;
    }

    public override void Clear()
    {
        base.Clear();
        m_UIFormAsset = null;
        _ObjectPoolRoot = null;
    }
    protected override void OnUnspawn()
    {
        base.OnUnspawn();

        ((GameObject)Target).transform.SetParent(_ObjectPoolRoot.transform);
    }

    protected override void Release(bool isShutdown)
    {
        ReleaseSubUIForm(m_UIFormAsset, Target);
    }

    /// <summary>
    /// 释放界面。
    /// </summary>
    /// <param name="uiFormAsset">要释放的界面资源。</param>
    /// <param name="uiFormInstance">要释放的界面实例。</param>
    public void ReleaseSubUIForm(object uiFormAsset, object uiFormInstance)
    {
        GameEntry.Resource.UnloadAsset(uiFormAsset);
        Object.Destroy((Object)uiFormInstance);
    }
}
