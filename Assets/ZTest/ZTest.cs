
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        int a = 1;
        int b = 1;
        var c = a-- - b;

    }
    Timer timer;

    int i = 1;

    private int uiid;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {


            int id = GameEntry.UIStack.OpenDialogWithTwoBtn("你好", "恭喜升级", "确认", "取消", (o) => { Debug.Log("确认" + o); }, (o) => { Debug.Log("取消" + o); }, "用户数据");

            DialogParams<int>  dialogParams = new DialogParams<int>();
            dialogParams.Mode = 2;
            dialogParams.Title = "提示";
            dialogParams.Message = "恭喜你升到一百级";
            dialogParams.CancelText = "取消了";
            dialogParams.ConfirmText = "知道了";
            dialogParams.UserData = "我是userData";
            dialogParams.OnClickConfirm = (p) => { Debug.Log(p.UserData + " [ok] " + p.DialogValue); };
            dialogParams.OnClickCancel = (p) => { Debug.Log(p.UserData + " [cancel] " + p.DialogValue); };

            id = GameEntry.UIStack.OpenDialog<int>("Assets/GameMain/UI/UIDialogs/DialogFormTest.prefab", dialogParams);

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameEntry.UI.RefocusUIForm(GameEntry.UI.GetUIForm(uiid));
        }

        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    GameFramework.GameFrameworkLog.Debug("测试");
        //    timer = GameEntry.Timer.Register(1, () => { GameFramework.GameFrameworkLog.Debug("计时器"); }, null, true);

            //}

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    timer.Cancel();
            //}
    }
}
