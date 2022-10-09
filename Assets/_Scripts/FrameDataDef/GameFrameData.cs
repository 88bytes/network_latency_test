using MessagePack;
using System.Collections.Generic;

// 玩家 index 在这一帧所有的操作命令
[MessagePackObject]
public class PlayerFrameData
{
    [Key(0)]
    public List<GameCmd> CmdList = null;
}

// 按照每秒 30 帧来 tick
[MessagePackObject]
public class GameFrameData
{
    [Key(0)]
    public uint FrameIndex;

    [Key(1)]
    public List<PlayerFrameData> PlayerFrameDataList = new List<PlayerFrameData>(100);

    public void Clear()
    {
        FrameIndex = 0;
        PlayerFrameDataList.Clear();
    }
}
