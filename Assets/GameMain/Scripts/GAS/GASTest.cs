using BandoWare.GameplayTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GASTest : MonoBehaviour
{
    ActionComponent _actionComponent;

    public GASAction GasAction;
    // Start is called before the first frame update
    void Start()
    {
        string tagName = "Attribute.HP";
        _actionComponent = GetComponent<ActionComponent>();
        
        _actionComponent.AddAttribute(tagName);


        Debug.Log(_actionComponent.GetAttribute(tagName));

        _actionComponent.GetAttribute(tagName).OnAttributeChanged.AddListener((o,m) => { Debug.Log("属性修改了原来的值为:"+o); });


        _actionComponent.AddAction(gameObject, GasAction);

        Debug.Log(_actionComponent.GetAttribute(tagName).GetValue());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
