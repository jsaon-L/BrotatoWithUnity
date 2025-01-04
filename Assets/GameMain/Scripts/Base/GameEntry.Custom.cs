//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using StarForce;


/// <summary>
/// 游戏入口。
/// </summary>
public partial class GameEntry : MonoBehaviour
{
    public static BuiltinDataComponent BuiltinData
    {
        get;
        private set;
    }


    public static UIStackComponent UIStack
    {
        get;
        private set;
    }
    
    public static SQLiteComponent SQLite
    {
        get;
        private set;
    }
    public static TimerComponent Timer
    {
        get;
        private set;
    }
    private static void InitCustomComponents()
    {
        BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
        UIStack = UnityGameFramework.Runtime.GameEntry.GetComponent<UIStackComponent>();
        SQLite = UnityGameFramework.Runtime.GameEntry.GetComponent<SQLiteComponent>();
        Timer = UnityGameFramework.Runtime.GameEntry.GetComponent<TimerComponent>();
    }
}
