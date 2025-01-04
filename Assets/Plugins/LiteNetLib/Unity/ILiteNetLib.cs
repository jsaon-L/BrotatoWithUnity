using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ILiteNetLib 
{
    public bool IsConnected { get; }
    public event UnityAction<string> OnReceiveMsg;

    public void Send(string msg);
}
