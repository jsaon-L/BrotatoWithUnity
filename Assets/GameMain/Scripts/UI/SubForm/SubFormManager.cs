using GameFramework;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using GameFramework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class SubUIFormManager
{
    public const string ObjectPoolName = "SubUIFormSingleSpawnObjectPool";

    private int _serialId = 0;
    private LoadAssetCallbacks _LoadAssetCallbacks;
    private Dictionary<int, string> _uiFormsBeingLoaded;
    private HashSet<int> _uiFormsToReleaseOnLoad;

    private IObjectPool<SubUIFormInstanceObject> _instancePool;
    private GameObject _instancePoolRootGameObject;

    private Dictionary<int, IUIForm> _openedSubForms;
    public SubUIFormManager()
    {
        _LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetFailureCallback);
        _uiFormsBeingLoaded = new Dictionary<int, string>();
        _uiFormsToReleaseOnLoad = new HashSet<int>();

        _instancePool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<SubUIFormInstanceObject>(ObjectPoolName);
        _instancePoolRootGameObject = new GameObject(ObjectPoolName);
        _instancePoolRootGameObject.transform.SetParent(GameEntry.ObjectPool.transform);

        _openedSubForms = new Dictionary<int, IUIForm>();
    }
    public int OpenSubUIForm(string assetName, Transform parent, SubFormContainer subFormContainer, object userData)
    {
        if (string.IsNullOrEmpty(assetName))
        {
            throw new System.NullReferenceException("Sub UI form asset name is null.");
        }
        if (subFormContainer == null)
        {
            throw new System.NullReferenceException("Sub UI form  SubFormContainer is null.");
        }

        int serialId = ++_serialId;
        var subUIFormInstance = _instancePool.Spawn(assetName);
        if (subUIFormInstance != null)
        {
            InternalOpenUIForm(serialId, assetName, parent, subFormContainer, subUIFormInstance.Target, false, 0, userData);
        }
        else
        {
            _uiFormsBeingLoaded.Add(serialId, assetName);
            GameEntry.Resource.LoadAsset(assetName, _LoadAssetCallbacks, OpenSubUIFormInfo.Create(serialId, userData, parent, subFormContainer));
        }
        return serialId;
    }
    /// <summary>
    /// �Ƿ����ڼ��ؽ��档
    /// </summary>
    /// <param name="serialId">�������б�š�</param>
    /// <returns>�Ƿ����ڼ��ؽ��档</returns>
    public bool IsLoadingUIForm(int serialId)
    {
        return _uiFormsBeingLoaded.ContainsKey(serialId);
    }

    /// <summary>
    /// ��ȡ���档
    /// </summary>
    /// <param name="serialId">�������б�š�</param>
    /// <returns>Ҫ��ȡ�Ľ��档</returns>
    public IUIForm GetUIForm(int serialId)
    {
        if (_openedSubForms.ContainsKey(serialId))
        {
            return _openedSubForms[serialId];
        }

        return null;
    }
    public void CloseSubUIForm(int serialId, SubFormContainer subFormContainer, object userData)
    {
        if (IsLoadingUIForm(serialId))
        {
            _uiFormsToReleaseOnLoad.Add(serialId);
            _uiFormsBeingLoaded.Remove(serialId);
            return;
        }

        CloseSubUIForm(GetUIForm(serialId), subFormContainer, userData);
    }
    public void CloseSubUIForm(IUIForm uiForm, SubFormContainer subFormContainer, object userData)
    {
        _openedSubForms.Remove(uiForm.SerialId);
        subFormContainer.RemoveDynamicSubUIForm((UIForm)uiForm);
        uiForm.OnCover();
        uiForm.OnPause();
        uiForm.OnClose(false, userData);
        _instancePool.Unspawn(uiForm.Handle);
        uiForm.OnRecycle();
    }


    private void LoadAssetSuccessCallback(string assetName, object asset, float duration, object userData)
    {
        OpenSubUIFormInfo openUIFormInfo = (OpenSubUIFormInfo)userData;

        //ҳ�滹û����ɣ��ͱ��ر���
        if (_uiFormsToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
        {
            _uiFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
            GameEntry.Resource.UnloadAsset(assetName);
        }
        else
        {
            //������Դ��ɣ�ʵ����GameObject����ҳ��
            _uiFormsBeingLoaded.Remove(openUIFormInfo.SerialId);

            SubUIFormInstanceObject uiFormInstanceObject = SubUIFormInstanceObject.Create(assetName, asset, InstantiateUIForm(asset), _instancePoolRootGameObject);
            _instancePool.Register(uiFormInstanceObject, true);

            InternalOpenUIForm(openUIFormInfo.SerialId, assetName, openUIFormInfo.Parent, openUIFormInfo.SubFormContainer, uiFormInstanceObject.Target, true, duration, openUIFormInfo.UserData);
        }

        ReferencePool.Release(openUIFormInfo);
    }

    private void LoadAssetFailureCallback(string assetName, LoadResourceStatus status, string errorMessage, object userData)
    {
        OpenSubUIFormInfo openUIFormInfo = (OpenSubUIFormInfo)userData;

        if (_uiFormsToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
        {
            _uiFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
        }
        _uiFormsBeingLoaded.Remove(openUIFormInfo.SerialId);

        string appendErrorMessage = Utility.Text.Format("Load UI form failure, asset name '{0}', status '{1}', error message '{2}'.", assetName, status, errorMessage);
        Debug.LogError(appendErrorMessage);
    }


    private void InternalOpenUIForm(int serialId, string uiFormAssetName, Transform parent, SubFormContainer subFormContainer, object uiFormInstance, bool isNewInstance, float duration, object userData)
    {
        try
        {
            IUIForm uiForm = CreateUIForm(uiFormInstance, parent, subFormContainer);
            if (uiForm == null)
            {
                throw new GameFrameworkException("Can not create UI form in UI form helper.");
            }
            _openedSubForms.Add(serialId, uiForm);
            uiForm.OnInit(serialId, uiFormAssetName, null, false, isNewInstance, userData);
            uiForm.OnOpen(userData);
            uiForm.OnResume();
            uiForm.OnReveal();
            subFormContainer.AddDynamicSubUIForm((UIForm)uiForm);
        }
        catch (System.Exception e)
        {
            throw e;
        }
    }


    /// <summary>
    /// ʵ�������档
    /// </summary>
    /// <param name="uiFormAsset">Ҫʵ�����Ľ�����Դ��</param>
    /// <returns>ʵ������Ľ��档</returns>
    public object InstantiateUIForm(object uiFormAsset)
    {
        return GameObject.Instantiate((UnityEngine.Object)uiFormAsset);
    }

    /// <summary>
    /// �������档
    /// </summary>
    /// <param name="uiFormInstance">����ʵ����</param>
    /// <param name="uiGroup">���������Ľ����顣</param>
    /// <param name="userData">�û��Զ������ݡ�</param>
    /// <returns>���档</returns>
    public IUIForm CreateUIForm(object uiFormInstance, Transform parent, SubFormContainer subFormContainer)
    {
        GameObject gameObject = uiFormInstance as GameObject;
        if (gameObject == null)
        {
            Log.Error("UI form instance is invalid.");
            return null;
        }

        Transform transform = gameObject.transform;
        transform.SetParent(parent);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        return gameObject.GetOrAddComponent<UIForm>();
    }


}