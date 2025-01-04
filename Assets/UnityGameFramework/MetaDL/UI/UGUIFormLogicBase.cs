using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarForce;
public class UGUIFormLogicBase : UGuiForm
{
    /// <summary>
    /// 页面是横屏还是竖屏,当页面打开时会自动设置屏幕旋转<see cref="InternalSetVisible"/>
    /// </summary>
    public UIDirection UIDirection;
    public bool CanBack = true;


    private List<int> _webRequests = new List<int>();

    private RectTransform _rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        RectTransform.Fill();
    }
    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
    }
    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
    }

    protected override void OnCover()
    {
        base.OnCover();
    }

    protected override void OnReveal()
    {
        base.OnReveal();
    }



    protected override void InternalSetVisible(bool visible)
    {
        gameObject.SetActive(visible);
        
        if (visible)
        {
            switch (UIDirection)
            {
                case UIDirection.None:
                    break;
                case UIDirection.Horizontal:
                    GameEntry.UIStack.HorizontalScreen();
                    break;
                case UIDirection.Vertical:
                    GameEntry.UIStack.VerticalScreen();
                    break;
                default:
                    break;
            }
        }
    }



   

    //protected override void OnClose(bool isShutdown, object userData)
    //{
    //    base.OnClose(isShutdown, userData);

    //    foreach (int i in _webRequests)
    //    {
    //        GameEntry.WebRequest.RemoveWebRequest(i);
    //    }
    //    ClearBindWebRequest();
    //}

    //public void BindWebRequest(int webRequestId)
    //{
    //    _webRequests.Add(webRequestId);
    //}
    //public void ClearBindWebRequest()
    //{
    //    _webRequests.Clear();
    //}

}
