using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MyUnityEvent:UnityEvent
{

}

/// <summary>
/// 箱子的状态
/// </summary>
public enum PushableStatus
{
    Idle,
    Fall
}

/// <summary>
/// 控制推的移动脚本
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Pushable : MonoBehaviour
{
    #region  字段

    private new Rigidbody2D rigidbody2D;
    /// <summary>
    /// 物体当前状态
    /// </summary>
    public PushableStatus currentStatus = PushableStatus.Idle;
    /// <summary>
    /// 存放检测点位置数组
    /// </summary>
    public Transform[] startCheckPos;
    /// <summary>
    /// 是否在地面上
    /// </summary>
    public bool isGround;
    /// <summary>
    /// Unity事件
    /// </summary>
    public UnityEvent<GameObject,int> OnStartFall;
    /// <summary>
    /// 箱子下落时相机聚焦的物体
    /// </summary>
    public GameObject cameraFollow;
    /// <summary>
    /// 相机聚焦时间
    /// </summary>
    public int time;
    /// <summary>
    /// 是否能够下落
    /// </summary>
    private bool isCanFall = true;
    /// <summary>
    /// 是不是玩家在推
    /// </summary>
    private bool isPush = false;

    #endregion

    #region  Unity回调

    private void Start()
    {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //CheckGround();

        //如果不是推的状态 x方向为0
        if (!isPush)
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }
    }

    #endregion

    #region  方法

    public void Move(Vector2 velocity)
    {
        rigidbody2D.velocity = velocity;
        Invoke("ResetIsPush", 0.1f);
    }

    public void Move(float x)
    {
        isPush = true;
        Move(new Vector2(x, rigidbody2D.velocity.y));
    }

    /// <summary>
    /// 检查是否在地面上
    /// </summary>
    public void CheckGround()
    {
        for(int i = 0; i < startCheckPos.Length; i++)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(startCheckPos[i].position, Vector3.down, 0.3f, 1 << 8);
            Debug.DrawLine(startCheckPos[i].position, startCheckPos[i].position+Vector3.down * 0.3f,Color.red);
            isGround = raycastHit2D;
            if (isGround==true)
            {
                currentStatus = PushableStatus.Idle;
                break;
            }
        }

        //当箱子状态为Idle，但不在地面上，则为开始下落状态
        if (!isGround && currentStatus == PushableStatus.Idle)
        {          
            if (OnStartFall != null || isCanFall)
            {
                //Debug.Log("开始下落");
                OnStartFall.Invoke(cameraFollow, time);
                isCanFall = false;
            }
                
            currentStatus = PushableStatus.Fall;
        }
    }

    /// <summary>
    /// 重制可推的状态
    /// </summary>
    public void ResetIsPush()
    {
        isPush = false;
    }

    #endregion

}
