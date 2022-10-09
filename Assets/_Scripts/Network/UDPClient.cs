using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UDPClient : MonoBehaviour, IUDPSocketMsgHandler
{
    public static bool ClientStarted = false;

    UDPSocket _udpSocket;

    const string SERVER_IP = "127.0.0.1";

    //const string SERVER_IP = "9.134.41.53";

    void OnEnable()
    {
        if (ClientStarted)
            return;
        ClientStarted = true;

        _udpSocket = new UDPSocket();
        _udpSocket.Client(SERVER_IP, UDPNetPort.ServerPort, this);
    }

    public void OnRecvMsg(EndPoint endPointFrom, byte[] msgData, int dataLength)
    {
        Debug.Log($"client recv data, from: {endPointFrom}, dataLength: {dataLength}");
    }

    public void Send(byte[] data)
    {
        _udpSocket.Send(data);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Send(Encoding.ASCII.GetBytes("hello world."));
        }
    }
}
