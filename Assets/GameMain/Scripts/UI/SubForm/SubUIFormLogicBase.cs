using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityGameFramework.Runtime;
using Sirenix;
using Sirenix.OdinInspector;

public class SubUIFormLogicBase : UGUIFormLogicBase
{
    [SerializeField]
    private SubFormContainer _subFormContainer;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        InitSubUIForm();
        _subFormContainer.OnInit(userData);
        _subFormContainer.DisableCanvasOverrideSorting();
    }


    private void InitSubUIForm()
    {
        if (_subFormContainer == null)
        {
            _subFormContainer = new SubFormContainer();
        }

        _subFormContainer.Initialize(UIForm);
    }


    #region SubForm

    public int OpenSubForm(string assetName, Transform parent, object userData = null)
    {
        return GameEntry.UIStack.OpenSubForm(assetName, parent != null ? parent : transform, _subFormContainer, userData);
    }

    public void CloseSubForm(int subSerialId,object userData = null)
    {
        GameEntry.UIStack.CloseSubForm(subSerialId, _subFormContainer,userData);
    }

    #endregion

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        //Debug.LogError("OnOpen",gameObject);
        _subFormContainer.OnOpen(userData);
    }

    protected override void InternalSetVisible(bool visible)
    {
        base.InternalSetVisible(visible);
        //Debug.LogError("InternalSetVisible", gameObject);
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        //Debug.LogError("OnClose", gameObject);
        _subFormContainer.OnClose(isShutdown, userData);
    }
    protected override void OnPause()
    {
        base.OnPause();
        //Debug.LogError("OnPause", gameObject);
        _subFormContainer.OnPause();
    }
    protected override void OnCover()
    {
        base.OnCover();
        //Debug.LogError("OnCover", gameObject);
        _subFormContainer.OnCover();
    }
    protected override void OnRecycle()
    {
        base.OnRecycle();
        //Debug.LogError("OnRecycle", gameObject);
        _subFormContainer.OnRecycle();
    }
    protected override void OnRefocus(object userData)
    {
        base.OnRefocus(userData);
        //Debug.LogError("OnRefocus", gameObject);
        _subFormContainer.OnRefocus(userData);
    }
    protected override void OnReveal()
    {
        base.OnReveal();
        //Debug.LogError("OnReveal", gameObject);
        _subFormContainer.OnReveal();
    }
    protected override void OnResume()
    {
        base.OnResume();
        //Debug.LogError("OnResume", gameObject);
        _subFormContainer.OnResume();
    }
    protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
    {
        base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        //Debug.LogError("OnDepthChanged", gameObject);
        _subFormContainer.OnDepthChanged(uiGroupDepth, depthInUIGroup);
    }
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
         //Debug.LogError("OnUpdate");
        _subFormContainer.OnUpdate(elapseSeconds, realElapseSeconds);
    }
}
