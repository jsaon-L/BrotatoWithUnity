using GameFramework;
using GameFramework.Fsm;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FSMExtension 
{

    public static void Change<TState,T>(this IFsm<T> fsm) where TState : FsmState<T> where T : class
    {
        Fsm<T> fsmImplement = (Fsm<T>)fsm;
        if (fsmImplement == null)
        {
            Debug.LogError("FSM is invalid.");
            return;
        }
        fsmImplement.ChangeState<TState>();
    }
    //public static void Change<T, TState>(this IFsm<T> fsm) where TState : FsmState<T> where T : class
    //{
    //    Fsm<T> fsmImplement = (Fsm<T>)fsm;
    //    if (fsmImplement == null)
    //    {
    //        Debug.LogError("FSM is invalid.");
    //        return;
    //    }
    //    fsmImplement.ChangeState<TState>();
    //}

    public static void Change<T>(this IFsm<T> fsm,Type state) where T : class
    {
        Fsm<T> fsmImplement = (Fsm<T>)fsm;

        fsmImplement.ChangeState(state);
    }
}
