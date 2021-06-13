using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 敌人状态的枚举类
/// </summary>
public enum EnemyStatus
{
    Idle,
    Walk,
    Run,
    Attack,
    Dead
}

/// <summary>
/// 控制敌人行为基本类
/// </summary>
public class EnemyBase : MonoBehaviour
{

    #region  字段

    /// <summary>
    /// 敌人刚体组件
    /// </summary>
    protected new Rigidbody2D rigidbody2D;
    /// <summary>
    /// 敌人当前状态
    /// </summary>
    protected EnemyStatus enemyStatus;
    /// <summary>
    /// 敌人精灵渲染组件
    /// </summary>
    protected SpriteRenderer spriteRenderer;
    /// <summary>
    /// 敌人动画状态机
    /// </summary>
    protected Animator animator;
    /// <summary>
    /// 敌人攻击组件
    /// </summary>
    protected Damage damage;
    /// <summary>
    /// 敌人受伤组件
    /// </summary>
    protected Damageable damageable;
    /// <summary>
    /// 敌人攻击范围
    /// </summary>
    public float attackRange;
    /// <summary>
    /// 敌人监听范围
    /// </summary>
    public float listenRange;
    /// <summary>
    /// 需要攻击的目标
    /// </summary>
    protected Transform attackTarget;

    #endregion

    #region  Unity回调

    protected virtual void Start()
    {
        //初始化组件
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        damage = transform.GetComponent<Damage>();
        damageable = transform.GetComponent<Damageable>();
        damageable.OnDead += OnDead;
        //初始化敌人最初状态
        enemyStatus = EnemyStatus.Idle;
        //初始化需要攻击的目标
        attackTarget = GameObject.Find("Player").transform;
    }

    protected virtual void Update()
    {
        //控制敌人行为
        UpdateStatus();
        //监听玩家是否进入攻击范围
        UpdateListener();
    }

    //protected virtual void OnDrawGizmosSelected()
    //{
    //    Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.2f);
    //    Handles.DrawSolidDisc(transform.position, Vector3.forward, attackRange);
    //    Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.2f);
    //    Handles.DrawSolidDisc(transform.position, Vector3.forward, listenRange);
    //}

    #endregion

    #region  方法

    /// <summary>
    /// 敌人死亡方法
    /// </summary>
    /// <param name="resetPos"></param>
    public virtual void OnDead(string resetPos)
    {
        enemyStatus = EnemyStatus.Dead;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        rigidbody2D.gravityScale = 0;
        Destroy(gameObject, 1f);
    }

    /// <summary>
    /// 敌人攻击方法
    /// </summary>
    public virtual void OnAttack()
    {
        damage.onDamage(attackTarget.gameObject);
    }

    /// <summary>
    /// 设置敌人移动速度
    /// </summary>
    /// <param name="speedX"></param>
    public virtual void SetSpeedX(float speedX)
    {
        if (speedX > 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
        rigidbody2D.velocity = new Vector2(speedX, rigidbody2D.velocity.y);
    }

    /// <summary>
    /// 更新敌人状态
    /// </summary>
    public virtual void UpdateStatus()
    {
       
    }

    /// <summary>
    /// 监听要攻击的目标
    /// </summary>
    public virtual void UpdateListener()
    {
        if (attackTarget == null)
        {
            Debug.LogError("要攻击的目标为空");
            return;
        }

        //攻击范围
        if (Vector3.Distance(transform.position, attackTarget.position) <= attackRange)
        {
            //若小于等于监听范围，则发现敌人
            //播放攻击动画
            enemyStatus = EnemyStatus.Attack;

            return;
        }

        //监听范围
        if (Vector3.Distance(transform.position, attackTarget.position) <= listenRange)
        {
            //若小于等于监听范围，则发现敌人
            //播放奔跑动画
            enemyStatus = EnemyStatus.Run;
        }


    }


    #endregion

}
