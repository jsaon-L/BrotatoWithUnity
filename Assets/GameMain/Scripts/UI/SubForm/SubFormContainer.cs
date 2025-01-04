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
    /// ��prefab�ڵľ�̬��ui
    /// ��̬��ҳ�����ܸ���ҳ����ͬ���������ڣ����Ҳ����Ƴ�
    /// </summary>
    /// 
    [UnityEngine.SerializeField]
    private List<UIForm> _staticSubUIForms = new List<UIForm>();
    /// <summary>
    /// �����ж�̬��ӵ���ui
    /// ��̬��ҳ��ֻ���������update ������������ֻ�����onInit onOpen onClose
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
        //�������о�̬����ҳ��
        List<UIFormLogic> subUIFormLogiss = new List<UIFormLogic>(owner.GetComponentsInChildren<UIFormLogic>());
        subUIFormLogiss.Remove(owner.Logic);
        HashSet<UIFormLogic> subUIFormLogisHashSet = new HashSet<UIFormLogic>(subUIFormLogiss);

        //�������嶼ɾ�� ʣ�µĶ��Ǹ��ڵ���
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
    /// ������ա�
    /// </summary>
    public void OnRecycle()
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnRecycle();
        }
    }

    /// <summary>
    /// ����رա�
    /// </summary>
    /// <param name="isShutdown">�Ƿ��ǹرս��������ʱ������</param>
    /// <param name="userData">�û��Զ������ݡ�</param>
    public void OnClose(bool isShutdown, object userData)
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnClose(isShutdown, userData);
        }
    }

    /// <summary>
    /// ������ͣ��
    /// </summary>
    public void OnPause()
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnPause();
        }
    }

    /// <summary>
    /// ������ͣ�ָ���
    /// </summary>
    public void OnResume()
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnResume();
        }
    }

    /// <summary>
    /// �����ڵ���
    /// </summary>
    public void OnCover()
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnCover();
        }
    }

    /// <summary>
    /// �����ڵ��ָ���
    /// </summary>
    public void OnReveal()
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnReveal();
        }
    }

    /// <summary>
    /// ���漤�
    /// </summary>
    /// <param name="userData">�û��Զ������ݡ�</param>
    public void OnRefocus(object userData)
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnRefocus(userData);
        }
    }

    /// <summary>
    /// ������ѯ��
    /// </summary>
    /// <param name="elapseSeconds">�߼�����ʱ�䣬����Ϊ��λ��</param>
    /// <param name="realElapseSeconds">��ʵ����ʱ�䣬����Ϊ��λ��</param>
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
    /// ������ȸı䡣
    /// </summary>
    /// <param name="uiGroupDepth">��������ȡ�</param>
    /// <param name="depthInUIGroup">�����ڽ������е���ȡ�</param>
    public void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
    {
        foreach (var uiWidget in _staticSubUIForms)
        {
            uiWidget.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        }
    }

    
}

