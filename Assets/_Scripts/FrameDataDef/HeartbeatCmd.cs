using MessagePack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MessagePackObject]
public class HeartbeatCmd
{
    [Key(0)]
    public int Index;

    [Key(1)]
    public float SendTime;

    [Key(2)]
    public float RecvTime;
}
