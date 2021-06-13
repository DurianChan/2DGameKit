using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制子弹的脚本
/// </summary>
public class Bullet : BulletBase
{
    #region  字段

    public float speed;

    #endregion


    #region  Unity回调

    protected override void Awake()
    {
        base.Awake();
    }

    #endregion


    #region  方法

    //判断是否反转
    public void SetDirection(bool isRight)
    {
        spriteRenderer.flipX = !isRight;
        rigidbody2D.velocity = new Vector2(isRight ? speed : -speed, 0);
    }

    #endregion

}
