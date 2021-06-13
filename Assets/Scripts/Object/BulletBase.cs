using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹基本类
/// </summary>
public class BulletBase : MonoBehaviour
{
    #region  字段

    protected new Rigidbody2D rigidbody2D;

    protected SpriteRenderer spriteRenderer;

    protected Damage damage;

    protected Animator animator;

    protected BoxCollider2D boxCollider2D;

    #endregion


    #region  Unity回调

    protected virtual void Awake()
    {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        damage = transform.GetComponent<Damage>();
        animator = transform.GetComponent<Animator>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //对游戏物体造成伤害
        damage.onDamage(collision.gameObject);
        //播放碰撞动画
        if(animator!=null)
            animator.SetBool("isBoom", true);
        //将速度设置为0，取消检测碰撞体
        rigidbody2D.velocity = Vector2.zero;
        transform.GetComponent<Collider2D>().enabled = false;
        //销毁自己
        Destroy(gameObject, 0.15f);
    }

    #endregion




}
