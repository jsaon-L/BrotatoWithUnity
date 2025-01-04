using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class UIDefaultValue : MonoBehaviour
{
    [Header("保存后缩放为0")]
    [SerializeField]
    public bool SetScaleOneInEditMode;

    protected Vector3 realScale = Vector3.zero;
    protected Vector3 editScale = Vector3.one;

    [Header("保存后透明度为0")]
    [SerializeField]
    public bool SetCanvasGroupInEditMode;

    protected float originAlpha = 0f;
    protected float editAlpha = 1f;
#if UNITY_EDITOR


    [InitializeOnLoadMethod]
    static void StartInitializeOnLoadMethod()
    {
        PrefabUtility.prefabInstanceUpdated += OnPrefabUpdate;

        PrefabStage.prefabStageClosing += OnPrefabStageClosing;
    }



    static public void OnPrefabUpdate(GameObject instance)
    {
        if(PrefabStageUtility.GetCurrentPrefabStage()!=null)
        {
            return;
        }
        Object parentPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(instance);
        UIDefaultValue[] value = instance.GetComponentsInChildren<UIDefaultValue>(true);
        if (value != null && value.Length > 0)
        {
            bool change = false;
            foreach (var element in value)
            {
                change |= element.OnSaveGameObject();
            }
            if(change)
            {
                PrefabUtility.ReplacePrefab(instance, parentPrefab, ReplacePrefabOptions.ConnectToPrefab);
                foreach (var element in value)
                {
                    element.Start();
                }
            }
        }
    }

    static public void OnPrefabStageClosing(PrefabStage stage)
    {
        UIDefaultValue[] value = stage.prefabContentsRoot.GetComponentsInChildren<UIDefaultValue>(true);
        if (value != null && value.Length > 0)
        {
            foreach (var element in value)
            {
                element.OnSaveGameObject();
            }
            PrefabUtility.SaveAsPrefabAsset(stage.prefabContentsRoot,stage.prefabAssetPath);
        }
    }

    public bool OnSaveGameObject()
    {
        bool change = false;
        if (SetScaleOneInEditMode)
        {
            if(this.gameObject.transform.localScale!=realScale)
            {
                this.gameObject.transform.localScale = realScale;
                change = true;
            }
        }

        if(SetCanvasGroupInEditMode)
        {
            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            if(canvasGroup&&canvasGroup.alpha!= originAlpha)
            {
                canvasGroup.alpha = originAlpha;
                change = true;
            }
        }

        return change;
    }

    [ExecuteInEditMode]
    public void Start()
    {
        if(Application.isPlaying)
        {
            return;
        }
        if (SetScaleOneInEditMode)
        {
            this.gameObject.transform.localScale = editScale;
        }

        if(SetCanvasGroupInEditMode)
        {
            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            if(canvasGroup)
            {
                canvasGroup.alpha = editAlpha;
            }
        }
    }

#endif
}
