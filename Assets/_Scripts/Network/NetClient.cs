using LitJson;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetClient : MonoBehaviour, IMsgHandler
{
    public static bool ClientStarted = false;

    UdpClient _socket;

    Dictionary<int, HeartbeatCmd> _cmdMap = new Dictionary<int, HeartbeatCmd>();

    int _index = 0;

    void OnEnable()
    {
        StartClient();
    }

    public void StartClient()
    {
        if (ClientStarted)
            return;
        ClientStarted = true;

        _cmdMap.Clear();

        _socket = new UdpClient(NetConfig.ClientPort);
        _socket.BeginReceive(OnRecvMsg, _socket);

        Debug.Log("Client Start.");
    }

    public void OnRecvMsg(IAsyncResult result)
    {
        IPEndPoint epFrom = new IPEndPoint(0, 0);

        #region 处理网络消息
        UdpClient socket = result.AsyncState as UdpClient;

        byte[] msg = socket.EndReceive(result, ref epFrom);

        string json = Encoding.ASCII.GetString(msg);
        HeartbeatCmd cmd = JsonMapper.ToObject<HeartbeatCmd>(json);

        if (!_cmdMap.ContainsKey(cmd.Index))
        {
            Debug.Log($"client recv data, from: {epFrom}, index not found: {cmd.Index}");
        }
        else
        {
            float time = Time.realtimeSinceStartup;
            cmd.RecvTime = time;
            Debug.Log($"client recv data, from: {epFrom}, deltaTime: {cmd.RecvTime - cmd.SendTime}");
        }
        #endregion

        _socket.BeginReceive(OnRecvMsg, _socket);
    }

    public void SendTo(byte[] data, IPEndPoint ep)
    {
        _socket.Send(data, data.Length, ep);
    }

    public void SendTestMsg()
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse(NetConfig.ServerAddr), NetConfig.ServerPort);

        _index++;

        float time = Time.realtimeSinceStartup;

        HeartbeatCmd cmd = new HeartbeatCmd();
        cmd.Index = _index;
        cmd.SendTime = time;

        string json = JsonMapper.ToJson(cmd);
        byte[] data = Encoding.ASCII.GetBytes(json);

        _cmdMap[_index] = cmd;

        SendTo(data, ep);
    }
}
