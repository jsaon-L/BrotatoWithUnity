
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


            int id = GameEntry.UIStack.OpenDialogWithTwoBtn("���", "��ϲ����", "ȷ��", "ȡ��", (o) => { Debug.Log("ȷ��" + o); }, (o) => { Debug.Log("ȡ��" + o); }, "�û�����");

            DialogParams<int>  dialogParams = new DialogParams<int>();
            dialogParams.Mode = 2;
            dialogParams.Title = "��ʾ";
            dialogParams.Message = "��ϲ������һ�ټ�";
            dialogParams.CancelText = "ȡ����";
            dialogParams.ConfirmText = "֪����";
            dialogParams.UserData = "����userData";
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
        //    GameFramework.GameFrameworkLog.Debug("����");
        //    timer = GameEntry.Timer.Register(1, () => { GameFramework.GameFrameworkLog.Debug("��ʱ��"); }, null, true);

            //}

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    timer.Cancel();
            //}
    }
}
