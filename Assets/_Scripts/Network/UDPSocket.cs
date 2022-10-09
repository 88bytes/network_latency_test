using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public interface IUDPSocketMsgHandler
{
    void OnRecvMsg(EndPoint endPointFrom, byte[] msgData, int dataLength);
}

public class UDPSocket
{
    private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    private const int BUFFER_SIZE = 8 * 1024;
    private State _state = new State();
    private EndPoint _endPointFrom = new IPEndPoint(IPAddress.Any, 0);
    private AsyncCallback _recvCallback = null;

    IUDPSocketMsgHandler _msgHandler;

    public class State
    {
        public byte[] buffer = new byte[BUFFER_SIZE];
    }

    public void Server(int port, IUDPSocketMsgHandler msgHandler)
    {
        _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
        _socket.Bind(new IPEndPoint(IPAddress.Any, port));
        _msgHandler = msgHandler;
        Receive();
    }

    public void Client(string address, int port, IUDPSocketMsgHandler msgHandler)
    {
        _socket.Connect(IPAddress.Parse(address), port);
        _msgHandler = msgHandler;
        Receive();
    }

    public void Send(byte[] data)
    {
        _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (result) =>
        {
            State asyncState = (State)result.AsyncState;
            int bytes = _socket.EndSend(result);

            // Console.WriteLine("SEND: {0}, {1}", bytes, text);
        },
        _state);
    }

    public void SendTo(byte[] data, EndPoint dstEndPoint)
    {
        _socket.BeginSendTo(data, 0, data.Length, SocketFlags.None, dstEndPoint, (result) =>
        {
            State asyncState = (State)result.AsyncState;
            int bytes = _socket.EndSend(result);

            // Console.WriteLine("SEND: {0}, {1}", bytes, text);
        },
        _state);
    }

    private void Receive()
    {
        _socket.BeginReceiveFrom(_state.buffer, 0, BUFFER_SIZE, SocketFlags.None, ref _endPointFrom, _recvCallback = (result) =>
        {
            State asyncState = (State)result.AsyncState;
            int dataLength = _socket.EndReceiveFrom(result, ref _endPointFrom);
            _socket.BeginReceiveFrom(asyncState.buffer, 0, BUFFER_SIZE, SocketFlags.None, ref _endPointFrom, _recvCallback, asyncState);

            _msgHandler.OnRecvMsg(_endPointFrom, asyncState.buffer, dataLength);

            // Console.WriteLine("RECV: {0}: {1}, {2}", _endPointFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes));
        },
        _state);
    }
}