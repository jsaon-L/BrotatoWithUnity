using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LiteNetLibServer : MonoBehaviour, ILiteNetLib
{

    public int Port = 9050;
    /// <summary>
    /// 相同key的客户端和服务端才会连接到一起
    /// </summary>
    public string ConnectKey = "key";


    public event UnityAction<string> OnReceiveMsg;
    NetDataWriter writer = new NetDataWriter();
    NetManager server;

    private bool _isConnected;
    public bool IsConnected { get { return _isConnected; } }

    void Start()
    {
        //Server
        EventBasedNetListener listener = new EventBasedNetListener();
        server = new NetManager(listener)
        {
            BroadcastReceiveEnabled = true,
            IPv6Enabled = true
        };
        listener.NetworkReceiveUnconnectedEvent += Listener_NetworkReceiveUnconnectedEvent;
        listener.ConnectionRequestEvent += Listener_ConnectionRequestEvent;
        listener.PeerConnectedEvent += Listener_PeerConnectedEvent;
        listener.NetworkReceiveEvent += Listener_NetworkReceiveEvent;

        server.Start(Port);
    }

    private void Listener_PeerConnectedEvent(NetPeer peer)
    {
        _isConnected = true;
        ServerDebug("连接成功:" + peer.EndPoint.ToString());
    }

    private void Listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        string msg = reader.GetString();
        reader.Recycle();

        try
        {
            OnReceiveMsg?.Invoke(msg);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
}

    private void Listener_ConnectionRequestEvent(ConnectionRequest request)
    {
        request.AcceptIfKey(ConnectKey);
    }

    private void Listener_NetworkReceiveUnconnectedEvent(System.Net.IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        writer.Reset();
        writer.Put(ConnectKey);
        server.SendUnconnectedMessage(writer, remoteEndPoint);
    }

    public void Send(string msg)
    {
        writer.Reset();
        writer.Put(msg);
        server.SendToAll(writer,DeliveryMethod.ReliableOrdered);
    }

    private void Update()
    {
        if (server != null)
        {
            server.PollEvents();
        }
    }
    private void OnDestroy()
    {
        if (server!=null)
        {
            server.Stop();
        }
    }

    public void ServerDebug(string msg)
    {
        Debug.Log($"[Server]: {msg}");
    }
}
