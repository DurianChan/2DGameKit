using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人发射子弹类
/// </summary>
public class AcidBubbles : BulletBase
{
    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    public void SetSpeed(Vector2 velocity)
    {
        rigidbody2D.velocity = velocity;
    }

}
