
using GameFramework;

/// <summary>
/// 控制ui close的时候是否刷新受影响ui
/// </summary>
public sealed class CloseUIFormParam : IReference
{

    public bool RefreshUI;
    public object UserData;
    public static CloseUIFormParam Create(object userData,bool refreshUI)
    {
        // 使用引用池技術，避免頻繁內存分配
        CloseUIFormParam e = ReferencePool.Acquire<CloseUIFormParam>();
        //生成事件實例后如果有 變量需要賦值 給這個函數加參數 在這里賦值
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


