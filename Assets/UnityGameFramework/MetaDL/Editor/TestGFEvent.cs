using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGFEvent : GameEventArgs
{

    public static readonly int EventId = typeof(TestGFEvent).GetHashCode();

    public override int Id
    {
        get { return EventId; }
    }

    //事件内数据示例
    //public string NewPlayerName = "God name";

    public static TestGFEvent Create()
    {
        // 使用引用池技术，避免频繁内存分配
        TestGFEvent e = ReferencePool.Acquire<TestGFEvent>();
        //生成事件实例后如果有 变量需要赋值 给这个函数加参数 在这里赋值
        //e.NewPlayerName = newPlayerName;

        return e;
    }

    public override void Clear()
    {
        //清理变量 简单来说就是把这里所有变量全赋值为null
        //NewPlayerName = null;
    }
}
