using System.Collections.Generic;
using System.Linq;
using GameFramework;
using GameFramework.UI;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;

[System.Serializable]
public sealed class SubFormContainer
{

    /// <summary>
    /// 在prefab内的静态子ui
    /// 静态子页面享受跟主页面相同的生命周期，并且不可移除
    /// </summary>
    /// 
    [UnityEngine.SerializeField]
    private List<UIForm> _staticSubUIForms = new List<UIForm>();
    /// <summary>
    /// 运行中动态添加的子ui
    /// 动态子页面只在这里调用update 其他生命周期只会调用onInit onOpen onClose
    /// TODO:OnRecycle
    /// </summary>
    /// 
    [UnityEngine.SerializeField]
    private List<UIForm> _dynamicSubUIForms = new List<UIForm>();

    
    public UIForm Owner
    {
        get;
        private set;
    }


    public void Initialize(UIForm owner)
    {
        Owner = owner;

        InitStaticSubUIForm(owner);
    }

    private void InitStaticSubUIForm(UIForm owner)
    {
        //查找所有静态的子页面
        List<UIFormLogic> subUIFormLogiss = new List<UIFormLogic>(owner.GetComponentsInChildren<UIFormLogic>());
        subUIFormLogiss.Remove(owner.Logic);
        HashSet<UIFormLogic> subUIFormLogisHashSet = new HashSet<UIFormLogic>(subUIFormLogiss);

        //把子物体都删掉 剩下的都是根节点了
        //subUIFormLogiss[i]
        for (int i = 0; i < subUIFormLogiss.Count; i++)
        {
            if (!subUIFormLogisHashSet.Contains(subUIFormLogiss[i]))
            {
                continue;
            }
            foreach (var childForm in subUIFormLogiss[i].GetComponentsInChildren<UIFormLogic>())
            {
                if (childForm == subUIFormLogiss[i])
                {
                    continue;
                }
                subUIFormLogisHashSet.Remove(childForm);
            }
        }
        _staticSubUIForms.AddRange(subUIFormLogisHashSet.Select(uilogic => uilogic.GetOrAddComponent<UIForm>()));

  
    }

    public void DisableCanvasOverrideSorting()
    {
        foreach (var form in _staticSubUIForms)
        {
            var formCanvas = form.GetComponent<Canvas>();
            if (formCanvas)
            {
                formCanvas.overrideSorting = false;
            }
        }
    }

    public void AddDynamicSubUIForm(UIForm uiWidget)
    {
        if (uiWidget == null)
        {
            Log.Error("Can't add empty!");
            return;
        }
        if (_dynamicSubUIForms.Contains(uiWidget))
        {
            Log.Error(Utility.Text.Format("Can't duplicate add UIWidget : '{0}'!", uiWidget.name));
            return;
        }

        var formCanvas = uiWidget.GetComponent<Canvas>();
        if (formCanvas)
        {
            formCanvas.overrideSorting = false;
        }
        _dynamicSubUIForms.Add(uiWidget);
    }


    public void RemoveDynamicSubUIForm(UIForm uiWidget)
    {
        if (!_dynamicSubUIForms.Remove(uiWidget))
        {
            Log.Error(Utility.Text.Format("UIWidget : '{0}' not in container.", uiWidget.name));
        }
    }



    public void OnInit(object userData)
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnInit(0, string.Empty, null, false, true, userData);
        }
    }
    public void OnOpen(object userData)
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnOpen(userData);
        }
    }


    /// <summary>
    /// 界面回收。
    /// </summary>
    public void OnRecycle()
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnRecycle();
        }
    }

    /// <summary>
    /// 界面关闭。
    /// </summary>
    /// <param name="isShutdown">是否是关闭界面管理器时触发。</param>
    /// <param name="userData">用户自定义数据。</param>
    public void OnClose(bool isShutdown, object userData)
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnClose(isShutdown, userData);
        }
    }

    /// <summary>
    /// 界面暂停。
    /// </summary>
    public void OnPause()
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnPause();
        }
    }

    /// <summary>
    /// 界面暂停恢复。
    /// </summary>
    public void OnResume()
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnResume();
        }
    }

    /// <summary>
    /// 界面遮挡。
    /// </summary>
    public void OnCover()
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnCover();
        }
    }

    /// <summary>
    /// 界面遮挡恢复。
    /// </summary>
    public void OnReveal()
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnReveal();
        }
    }

    /// <summary>
    /// 界面激活。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    public void OnRefocus(object userData)
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnRefocus(userData);
        }
    }

    /// <summary>
    /// 界面轮询。
    /// </summary>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    public void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        foreach (var subUIForm in _dynamicSubUIForms)
        {
            subUIForm.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }

    /// <summary>
    /// 界面深度改变。
    /// </summary>
    /// <param name="uiGroupDepth">界面组深度。</param>
    /// <param name="depthInUIGroup">界面在界面组中的深度。</param>
    public void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        }
    }

    
}

