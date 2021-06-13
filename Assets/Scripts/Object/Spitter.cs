using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spitter敌人控制脚本
/// </summary>
public class Spitter : EnemyBase
{
    #region  字段

    /// <summary>
    /// 子弹预制体
    /// </summary>
    private GameObject bulletPrefab;

    /// <summary>
    /// 生成子弹的位置
    /// </summary>
    public Transform bulletSpawnPos;

    #endregion

    #region  Unity回调

    protected override void Update()
    {
        base.Update();
        UpdateDirection();
    }

    /// <summary>
    /// 触碰对玩家造成伤害
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            damage.onDamage(collision.gameObject);
        }
    }


    #endregion

    #region  方法

    /// <summary>
    /// 更新敌人状态
    /// </summary>
    public override void UpdateStatus()
    {
        base.UpdateStatus();

        switch (enemyStatus)
        {
            case EnemyStatus.Idle:

                break;
            case EnemyStatus.Attack:
                animator.SetBool("isAttack", true);
                break;
            case EnemyStatus.Dead:
                animator.SetBool("isDead", true);
                break;
        }
        if (enemyStatus != EnemyStatus.Attack)
        {
            animator.SetBool("isAttack", false);
        }
    }

    /// <summary>
    /// 敌人监听状态
    /// </summary>
    public override void UpdateListener()
    {
        base.UpdateListener();
        if (Vector3.Distance(transform.position, attackTarget.position) > attackRange)
        {
            enemyStatus = EnemyStatus.Idle;
        }
    }

    /// <summary>
    /// 更新敌人的方向
    /// </summary>
    public void UpdateDirection()
    {
        if (attackTarget.position.x - transform.position.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (attackTarget.position.x - transform.position.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    /// <summary>
    /// 敌人攻击方法
    /// </summary>
    public override void OnAttack()
    {
        //创建子弹
        if(bulletPrefab == null)
        {
            bulletPrefab = Resources.Load<GameObject>("Prefabs/Object/AcidBubbles");
        }
        GameObject bullet =  GameObject.Instantiate(bulletPrefab);
        bullet.transform.position = bulletSpawnPos.position;

        //将生成的子弹向上抛一段距离，并通过自由落体运动的位移公式计算出时间，再通过 y = v * t 公式算出速度 然后将速度赋值给子弹
        //环境重力加速度 加上子弹本身的重力
        float g = Mathf.Abs(Physics2D.gravity.y) * bullet.transform.GetComponent<Rigidbody2D>().gravityScale;

        //竖直向上的初速度
        float v0 = 8;
        float t0 = v0 / g;
        float y0 = 0.5f * g * Mathf.Pow(t0, 2);
        //水平方向的速度
        float v = 0;

        //子弹水平位移加上随机值
        float x = attackTarget.position.x - transform.position.x + Random.Range(-1.5f, 1.5f);

        //当攻击目标位置的y轴小于敌人的y轴
        if (transform.position.y + y0 > attackTarget.position.y)
        {
            //计算子弹需要的初速度
            //y = 0.5 * a * t * t
            float y = transform.position.y - attackTarget.position.y + y0;
            //子弹运动总时间
            float t = Mathf.Sqrt((y * 2) / Mathf.Abs(Physics2D.gravity.y)) + t0;
            v = x / t;
        }//当攻击目标位置的y轴大于敌人的y轴  求出可到达目标需要的初速度
        else if (transform.position.y + y0 < attackTarget.position.y)
        {
            float y = attackTarget.position.y - transform.position.y;
            float t = Mathf.Sqrt((y*2)/g);

            v0 = g * t;
            v = x / t;
        }

        bullet.GetComponent<AcidBubbles>().SetSpeed(new Vector2(v, v0));

    }

    #endregion
}
