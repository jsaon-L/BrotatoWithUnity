
using GameFramework;

/// <summary>
/// ����ui close��ʱ���Ƿ�ˢ����Ӱ��ui
/// </summary>
public sealed class CloseUIFormParam : IReference
{

    public bool RefreshUI;
    public object UserData;
    public static CloseUIFormParam Create(object userData,bool refreshUI)
    {
        // ʹ�����óؼ��g�������l���ȴ����
        CloseUIFormParam e = ReferencePool.Acquire<CloseUIFormParam>();
        //�����¼������������ ׃����Ҫ�xֵ �o�@�������Ӆ��� ���@���xֵ
        e.RefreshUI = refreshUI;
        e.UserData = userData;
        return e;
    }


    public void Clear()
    {
        RefreshUI = false;
        UserData = null;
    }
}


