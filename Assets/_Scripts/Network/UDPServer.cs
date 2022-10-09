using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class UDPServer : MonoBehaviour, IUDPSocketMsgHandler
{
    public static bool ServerStarted = false;

    UDPSocket _udpSocket;

    void OnEnable()
    {
        if (ServerStarted)
            return;
        ServerStarted = true;

        _udpSocket = new UDPSocket();
        _udpSocket.Server(UDPNetPort.ServerPort, this);
    }

    public void OnRecvMsg(EndPoint endPointFrom, byte[] msgData, int dataLength)
    {
        Debug.Log($"server recv data, from: {endPointFrom}, dataLength: {dataLength}");
        SendTo(Encoding.ASCII.GetBytes("server res: hello world."), endPointFrom);
    }

    public void SendTo(byte[] data, EndPoint dstEndPoint)
    {
        _udpSocket.SendTo(data, dstEndPoint);
    }
}
