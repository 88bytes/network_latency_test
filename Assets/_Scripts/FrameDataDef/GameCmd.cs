using MessagePack;
using UnityEngine;

[MessagePackObject]
public class GameCmd
{
    public enum ECmdType
    {
        None,
        AddNewPlayer,
        SetMoveDir,
    }

    [Key(0)]
    public byte PlayerID; // 指令来自哪一个 Player

    [Key(1)]
    public ECmdType Cmd;  // 指令类型

    [Key(2)]
    public short Param1;  // 参数1 - 不同的指令类型会有不同的解读

    [Key(3)]
    public short Param2;  // 参数2

    public static GameCmd FromMoveDir(Vector3 moveDir)
    {
        return null;
    }
}
