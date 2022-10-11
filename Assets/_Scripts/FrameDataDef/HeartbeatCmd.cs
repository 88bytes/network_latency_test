using MessagePack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MessagePackObject]
public class HeartbeatCmd
{
    public int Index;
    public float SendTime;
    public float RecvTime;
}
