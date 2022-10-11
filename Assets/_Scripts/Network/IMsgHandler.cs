using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMsgHandler
{
    public void OnRecvMsg(IAsyncResult result);
}
