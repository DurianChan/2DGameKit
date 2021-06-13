using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制boss第二种攻击方法发射的子弹
/// </summary>
public class Gunner2Bullet : BulletBase
{
    protected override void Awake()
    {
        base.Awake();
        Destroy(gameObject, 5f);
    }

    public void SetSpeed(Vector2 velocity)
    {
        rigidbody2D.velocity = velocity;
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.tag != TagConst.Player)
            return;
        base.OnCollisionEnter2D(collision);
    }

}
