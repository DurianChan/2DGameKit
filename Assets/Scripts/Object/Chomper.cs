using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 控制敌人Chomper行为脚本
/// </summary>
public class Chomper : EnemyBase
{

    #region  字段


    /// <summary>
    /// 敌人移动速度
    /// </summary>
    public float speed;
    /// <summary>
    /// 检测前方是否可以移动的位置
    /// </summary>
    private Transform startCheckPos;
    /// <summary>
    /// 判断是否能移动
    /// </summary>
    private bool isCanMove = true;
    /// <summary>
    /// 敌人闲置计时器
    /// </summary>
    private float idleTimer;

    #endregion

    #region  Unity回调

    protected override void Start()
    {
        base.Start();
        //初始化射线检测位置
        startCheckPos = transform.Find("startCheckPos");
    }

    protected override void Update()
    {
        //检测是否可以移动
        CheckIsCanMove();
        base.Update();
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

    #region  控制行为方法

    /// <summary>
    /// 检查前方是否可以移动
    /// </summary>
    public void CheckIsCanMove()
    {
        //检查下方是否为地面可移动
        RaycastHit2D raycastHit2D = Physics2D.Raycast(startCheckPos.position, Vector2.down, 1f, 1 << 8);
        //检测前方是否可移动
        RaycastHit2D raycast_forward = Physics2D.Raycast(startCheckPos.position, spriteRenderer.flipX ? Vector2.left : Vector2.right, 0.4f, 1 << 8);
        //判断是否能移动的布尔值
        if (raycast_forward)
        {
            isCanMove = false;
        }
        else
        {
            isCanMove = raycastHit2D;
        }
        //Debug.DrawLine(startCheckPos.position,startCheckPos.position+Vector3.down,Color.red);
    }

    /// <summary>
    /// 更新敌人状态
    /// </summary>
    public override void UpdateStatus()
    {
        base.UpdateStatus();
        switch (enemyStatus)
        {
            case EnemyStatus.Idle:
                SetSpeedX(0);

                idleTimer += Time.deltaTime;
                if (idleTimer > 2)
                {
                    idleTimer = 0;
                    enemyStatus = EnemyStatus.Walk;
                }
                break;
            case EnemyStatus.Walk:
                SetSpeedX(speed);
                if (!isCanMove)
                {
                    speed = -speed;
                    startCheckPos.localPosition = new Vector3(-startCheckPos.localPosition.x, startCheckPos.localPosition.y, startCheckPos.localPosition.z);
                }
                animator.SetBool("isWalk", true);
                break;
            case EnemyStatus.Run:
                animator.SetBool("isRun", true);
                //跑向要攻击的目标(能够移动且在监听范围内)
                if (isCanMove && Vector3.Distance(transform.position, attackTarget.position) <= listenRange)
                {
                    if (attackTarget.position.x - transform.position.x > 0)
                    {
                        //往右边
                        speed = Mathf.Abs(speed);
                        startCheckPos.localPosition = new Vector3(Mathf.Abs(startCheckPos.localPosition.x), startCheckPos.localPosition.y, startCheckPos.localPosition.z);
                    }
                    else
                    {
                        //往左边
                        speed = -Mathf.Abs(speed);
                        startCheckPos.localPosition = new Vector3(-Mathf.Abs(startCheckPos.localPosition.x), startCheckPos.localPosition.y, startCheckPos.localPosition.z);
                    }
                    SetSpeedX(speed);
                }
                else
                {
                    SetSpeedX(0);
                    enemyStatus = EnemyStatus.Idle;
                }
                break;
            case EnemyStatus.Attack:
                animator.SetBool("isAttack", true);
                break;
            case EnemyStatus.Dead:
                animator.SetBool("isDead", true);
                break;
        }

        if (enemyStatus != EnemyStatus.Walk)
        {
            animator.SetBool("isWalk", false);
        }
        if (enemyStatus != EnemyStatus.Run)
        {
            animator.SetBool("isRun", false);
        }
        if (enemyStatus != EnemyStatus.Attack)
        {
            animator.SetBool("isAttack", false);
        }
    }

    /// <summary>
    /// 设置敌人移动速度
    /// </summary>
    /// <param name="speedX"></param>
    public override void SetSpeedX(float speedX)
    {
        base.SetSpeedX(speedX);
    }

    /// <summary>
    /// 监听要攻击的目标
    /// </summary>
    public override void UpdateListener()
    {
        base.UpdateListener();
    }

    #endregion

    #region  伤害相关方法

    /// <summary>
    /// 敌人攻击状态
    /// </summary>
    public override void OnAttack()
    {
        base.OnAttack();
    }

    /// <summary>
    /// 死亡状态
    /// </summary>
    /// <param name="resetPos"></param>
    public override void OnDead(string resetPos)
    {
        SetSpeedX(0);
        base.OnDead(resetPos);
    }

    #endregion

}
