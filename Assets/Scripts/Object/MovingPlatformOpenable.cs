using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可开关控制的移动平台
/// </summary>
public class MovingPlatformOpenable : MovingPlatformBase, Openable
{
    /// <summary>
    /// 判断开关是否为打开状态
    /// </summary>
    private bool isOpen;

    public void Close()
    {
        isOpen = false;
    }

    public void Open()
    {
        isOpen = true;
    }

    protected override void Update()
    {
        if (isOpen)
        {
            base.Update();
        }
    }

    protected override void LateUpdate()
    {
        if (isOpen)
        {
            base.LateUpdate();
        }
    }

}
