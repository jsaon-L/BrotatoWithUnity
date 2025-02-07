using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;


[ExecuteAlways]
[RequireComponent(typeof(TextMeshProUGUI))]
public class BrotatoLanguageText : MonoBehaviour
{
    [OnValueChanged("OnValueChange")]
    public string Key;

    private TextMeshProUGUI _textMeshProUGUI;
    
    private void Awake()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        if (!Application.IsPlaying(gameObject))
        {
            SyncKeyToText();
        }
        
    }
    private void Start()
    {
        if (Application.IsPlaying(gameObject))
        {
            SetLocalizationText(Key);
        }
    }

    public void SetKey(string key)
    {
        Key = key;
        SetLocalizationText(key);
    }


#if UNITY_EDITOR
    public void OnValueChange()
    {
        SyncKeyToText();
    }
#endif

    private void SyncKeyToText()
    {
        GetComponent<TextMeshProUGUI>().text = Key;
    }

    public void SetLocalizationText(string key)
    {
        Key = key;
        _textMeshProUGUI.text = GameEntry.Localization.GetString(key);
    }

}
