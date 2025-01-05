using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LiteNetLibClient : MonoBehaviour, ILiteNetLib
{

    public int Port = 9050;

    /// <summary>
    /// ��ͬkey�Ŀͻ��˺ͷ���˲Ż����ӵ�һ��
    /// </summary>
    public string ConnectKey = "key";

    public event UnityAction<string> OnReceiveMsg;
    NetManager client;
    NetDataWriter writer = new NetDataWriter();

    private bool _isConnected;
    public bool IsConnected { get { return _isConnected; } }
    void Start()
    {
        EventBasedNetListener listener = new EventBasedNetListener();

        client = new NetManager(listener)
        {
            UnconnectedMessagesEnabled = true,
            IPv6Enabled = true
        };

        listener.NetworkReceiveUnconnectedEvent += Listener_NetworkReceiveUnconnectedEvent;
        listener.NetworkReceiveEvent += Listener_NetworkReceiveEvent;
        listener.PeerConnectedEvent += Listener_PeerConnectedEvent;

        client.Start();

        //Send broadcast
        StartCoroutine(SendBroadcastFindServer());
        
    }

    private void Listener_PeerConnectedEvent(NetPeer peer)
    {
        _isConnected = true;
        ClientDebug("���ӳɹ�");
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

    private void Listener_NetworkReceiveUnconnectedEvent(System.Net.IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        var text = reader.GetString();
        if (messageType == UnconnectedMessageType.BasicMessage && text == ConnectKey)
        {
            client.Connect(remoteEndPoint, ConnectKey);
        }
    }

    IEnumerator SendBroadcastFindServer()
    {
        while (true)
        {
            if (client != null && client.ConnectedPeersCount <= 0)
            {
                SendBroadcast(ConnectKey);
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void SendBroadcast(string msg)
    {
        writer.Reset();
        writer.Put(msg);
        client.SendBroadcast(writer, Port);
    }


    public void Send(string msg)
    {
        writer.Reset();
        writer.Put(msg);
        client.SendToAll(writer, DeliveryMethod.ReliableOrdered);
    }

    // Update is called once per frame
    void Update()
    {
        if (client != null)
        {
            client.PollEvents();
        }
    }

    private void OnDestroy()
    {
        if (client!=null)
        {
            client.Stop();
        }
    }

    public void ClientDebug(string msg)
    {
        Debug.Log($"[Client]: {msg}");
    }
}
