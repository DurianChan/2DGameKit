using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class PassPlatform : MonoBehaviour
{
    /// <summary>
    /// 下落的层级
    /// </summary>
    private int targetLayer;
    /// <summary>
    /// 半圆碰撞检测组件
    /// </summary>
    private PlatformEffector2D platformEffector; 

    private void Start()
    {
        platformEffector = transform.GetComponent<PlatformEffector2D>();
    }

    /// <summary>
    /// 穿过平台
    /// </summary>
    /// <param name="target">穿过目标</param>
    public void Fall(GameObject target)
    {
        //获取目标的层级
        targetLayer = 1 << target.layer;
        //把这个目标的碰撞的层取消掉
        platformEffector.colliderMask &= ~targetLayer;
        //恢复原来的碰撞层级
        Invoke("ResetLayer", 0.5f);
    }

    /// <summary>
    /// 将原来的碰撞层级附加上
    /// </summary>
    public void ResetLayer()
    {
        platformEffector.colliderMask |= targetLayer;
    }

}
