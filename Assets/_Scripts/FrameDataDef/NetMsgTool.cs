using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMsgTool
{
    public static uint BytesToUint(byte[] bytes)
    {
        uint value = 0;
        value += (uint)bytes[3] << 24;
        value += (uint)bytes[2] << 16;
        value += (uint)bytes[1] << 8;
        value += (uint)bytes[0] << 0;
        return value;
    }

    public static byte[] UintToBytes(uint value)
    {
        byte[] bytes = new byte[4];

        bytes[0] = (byte)(value & 0x000000FF);
        value = value >> 8;
        bytes[1] = (byte)(value & 0x000000FF);
        value = value >> 8;
        bytes[2] = (byte)(value & 0x000000FF);
        value = value >> 8;
        bytes[3] = (byte)(value & 0x000000FF);

        return bytes;
    }
}
