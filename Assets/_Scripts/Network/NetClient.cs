using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetClient : MonoBehaviour, IMsgHandler
{
    public static bool ClientStarted = false;

    UdpClient _socket;

    void OnEnable()
    {
        StartClient();
    }

    public void StartClient()
    {
        if (ClientStarted)
            return;
        ClientStarted = true;

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
        Debug.Log($"client recv data, from: {epFrom}, dataLength: {msg.Length}");
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
        SendTo(Encoding.ASCII.GetBytes("hello world."), ep);
    }
}
