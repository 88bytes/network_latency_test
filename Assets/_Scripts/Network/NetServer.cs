using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NetServer : MonoBehaviour, IMsgHandler
{
    public static bool ServerStarted = false;

    UdpClient _socket;

    void OnEnable()
    {
        StartServer();
    }

    void StartServer()
    {
        if (ServerStarted)
            return;
        ServerStarted = true;

        _socket = new UdpClient(NetConfig.ServerPort);
        _socket.BeginReceive(OnRecvMsg, _socket);

        Debug.Log("Server Start.");
    }

    public void OnRecvMsg(IAsyncResult result)
    {
        IPEndPoint epFrom = new IPEndPoint(0, 0);

        #region 处理网络消息
        UdpClient socket = result.AsyncState as UdpClient;
        byte[] msg = socket.EndReceive(result, ref epFrom);
        Debug.Log($"server recv data, from: {epFrom}, data length: {msg.Length}.");

        // 把消息直接返回去
        SendTo(msg, epFrom);
        #endregion

        _socket.BeginReceive(OnRecvMsg, _socket);
    }

    public void SendTo(byte[] data, IPEndPoint ep)
    {
        _socket.Send(data, data.Length, ep);
    }
}
