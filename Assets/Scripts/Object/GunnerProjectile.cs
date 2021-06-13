using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制boss第一种攻击方法发射的子弹
/// </summary>
public class GunnerProjectile : BulletBase
{

    public float speed = 10f;

    protected override void Awake()
    {
        base.Awake();
        Destroy(gameObject, 5f);
    }

    public void SetDirection(Vector3 direction)
    {
        rigidbody2D.velocity = direction * speed;
    }


}
