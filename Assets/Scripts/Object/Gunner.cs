using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// boss的状态
/// </summary>
public enum GunnerStatus
{
    Idle,
    Attack,
    Disable,
    Dead
}

/// <summary>
/// 控制boss的逻辑脚本
/// </summary>
public class Gunner : MonoBehaviour
{
    #region  字段

    private Animator animator;

    private Damage damage;
    /// <summary>
    /// Boss生命值
    /// </summary>
    private Damageable bossAble;
    /// <summary>
    /// 能量罩生命值
    /// </summary>
    private Damageable defenceAble;
    /// <summary>
    /// boss当前状态
    /// </summary>
    private GunnerStatus currentStatus = GunnerStatus.Idle;
    /// <summary>
    /// boss默认血量
    /// </summary>
    private int defaultBossHp;
    /// <summary>
    /// 防御罩默认血量
    /// </summary>
    private int defaultDefenceHp;
    /// <summary>
    /// boss攻击间隔
    /// </summary>
    private float attackTime = 3f;

    /// <summary>
    /// boss第一种子弹预制体
    /// </summary>
    public GameObject bossBullet1;
    /// <summary>
    /// 第一种子弹发射的位置
    /// </summary>
    private Transform bullet1Pos;
    /// <summary>
    /// 第一种攻击的线
    /// </summary>
    private LineRenderer attack1Line;
    /// <summary>
    /// 攻击1的线的位置
    /// </summary>
    private List<Vector3> attack1LinePosition = new List<Vector3>();

    /// <summary>
    /// boss第二种攻击的子弹
    /// </summary>
    public GameObject bossBullet2;
    /// <summary>
    /// 第二种子弹的位置
    /// </summary>
    private Transform bullet2Pos;

    /// <summary>
    /// 第三种攻击的方法
    /// </summary>
    public BulletLighting bullet3;

    /// <summary>
    /// 攻击目标
    /// </summary>
    private GameObject attackTarget;

    /// <summary>
    /// boss血条面板
    /// </summary>
    public BossPanel bossPanel;

    #endregion

    #region  Unity回调

    private void Start()
    {
        //初始化组件
        animator = transform.GetComponent<Animator>();
        damage = transform.GetComponent<Damage>();
        bossAble = transform.GetComponent<Damageable>();
        bossAble.OnHurt += this.OnBossHurt;
        bossAble.OnDead += this.OnBossDead;
        defenceAble = transform.Find("GunnerShield").GetComponent<Damageable>();
        defenceAble.OnHurt += this.OnDefenceHurt;
        defenceAble.OnDead += this.OnDefenceDead;

        bullet1Pos = transform.Find("bullet1Pos").transform;
        attackTarget = GameObject.Find("Player");
        attack1Line = transform.Find("attack1Line").GetComponent<LineRenderer>();

        bullet2Pos = transform.Find("bullet2Pos").transform;

        defaultBossHp = bossAble.health;
        defaultDefenceHp = defenceAble.health;

        StartAttack();
    }

    private void Update()
    {
        //更新boss状态
        OnUpdateStatus();
        //更新攻击1的线
        UpdateAttack1Line();
    }



    #endregion

    #region  方法

    /// <summary>
    /// 更新boss状态
    /// </summary>
    public void OnUpdateStatus()
    {
        switch (currentStatus)
        {
            case GunnerStatus.Idle:

                break;
            case GunnerStatus.Attack:

                break;
            case GunnerStatus.Disable:
                animator.SetBool("isDisable", true);
                break;
            case GunnerStatus.Dead:
                animator.SetBool("isDead", true);
                break;
        }

        if (currentStatus != GunnerStatus.Disable)
        {
            animator.SetBool("isDisable", false);
        }
        if (currentStatus != GunnerStatus.Dead)
        {
            animator.SetBool("isDead", false);
        }

    }

    /// <summary>
    /// 开始攻击状态
    /// </summary>
    public void StartAttack()
    {
        InvokeRepeating("Attack", 1f, attackTime);
    }

    /// <summary>
    /// 攻击方法
    /// </summary>
    public void Attack()
    {
        int attackType = Random.Range(1, 4);

        if (attackType == 1)
        {
            attack1Line.transform.gameObject.SetActive(true);
        }
        else if (attackType != 1)
        {
            attack1Line.transform.gameObject.SetActive(false);
        }

        animator.SetFloat("attackType", attackType);
        animator.SetTrigger("attack");
    }

    /// <summary>
    /// 执行攻击
    /// </summary>
    public void AttackExc(int type)
    {
        switch (type)
        {
            case 1://向玩家发射一个子弹
                GameObject bulletObj = GameObject.Instantiate(bossBullet1);
                bulletObj.transform.position = bullet1Pos.position;
                bulletObj.GetComponent<GunnerProjectile>().SetDirection((attackTarget.transform.position + Vector3.up - bullet1Pos.position).normalized);
                break;
            case 2://想玩家抛一个子弹
                GameObject bullet2 = GameObject.Instantiate(bossBullet2);
                bullet2.transform.position = bullet2Pos.position;

                //将生成的子弹向上抛一段距离，并通过自由落体运动的位移公式计算出时间，再通过 y = v * t 公式算出速度 然后将速度赋值给子弹
                //环境重力加速度 加上子弹本身的重力
                float g = Mathf.Abs(Physics2D.gravity.y) * bullet2.transform.GetComponent<Rigidbody2D>().gravityScale;

                //竖直向上的初速度
                float v0 = 8;
                float t0 = v0 / g;
                float y0 = 0.5f * g * Mathf.Pow(t0, 2);
                //水平方向的速度
                float v = 0;

                //子弹水平位移加上随机值
                float x = attackTarget.transform.position.x - transform.position.x + Random.Range(-1.5f, 1.5f);

                //当攻击目标位置的y轴小于敌人的y轴
                if (transform.position.y + y0 > attackTarget.transform.position.y)
                {
                    //计算子弹需要的初速度
                    //y = 0.5 * a * t * t
                    float y = transform.position.y - attackTarget.transform.position.y + y0;
                    //子弹运动总时间
                    float t = Mathf.Sqrt((y * 2) / Mathf.Abs(Physics2D.gravity.y)) + t0;
                    v = x / t;
                }//当攻击目标位置的y轴大于敌人的y轴  求出可到达目标需要的初速度
                else if (transform.position.y + y0 < attackTarget.transform.position.y)
                {
                    float y = attackTarget.transform.position.y - transform.position.y;
                    float t = Mathf.Sqrt((y * 2) / g);

                    v0 = g * t;
                    v = x / t;
                }

                bullet2.GetComponent<Gunner2Bullet>().SetSpeed(new Vector2(v,v0));
                break;
            case 3:
                bullet3.Show();
                break;
        }
    }

    /// <summary>
    /// 停止所有调用攻击Invoke
    /// </summary>
    public void StopAttack()
    {
        CancelInvoke("Attack");
    }

    /// <summary>
    /// 更新攻击1的线
    /// </summary>
    public void UpdateAttack1Line()
    {
        if (!attack1Line.gameObject.activeSelf) return;
        attack1LinePosition.Clear();
        attack1LinePosition.Add(bullet1Pos.position);
        attack1LinePosition.Add(bullet1Pos.position+(attackTarget.transform.position + Vector3.up - bullet1Pos.position).normalized * 20);
        attack1Line.SetPositions(attack1LinePosition.ToArray());
        attack1Line.positionCount = attack1LinePosition.Count;
    }

    #endregion

    #region  受伤相关方法

    public void OnBossHurt(HurtType hurtType, string resetPos)
    {
        if (defenceAble.health > 0)
        {
            defenceAble.TakeDamage(1, hurtType, resetPos);
            bossAble.health++;
            return;
        }
        //更新Boss的血量
        bossPanel.UpdateBossHp((float)bossAble.health/(float)defaultBossHp);
    }

    public void OnBossDead(string resetPos)
    {
        currentStatus = GunnerStatus.Dead;
        animator.SetTrigger("trigger");
        //更新Boss的血量
        bossPanel.UpdateBossHp((float)bossAble.health / (float)defaultBossHp);
        //隐藏面板
        bossPanel.Hide();
        StopAttack();
        //隐藏碰撞体
        transform.GetComponent<Rigidbody2D>().gravityScale = 0;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 10f);
    }

    public void OnDefenceHurt(HurtType hurtType, string resetPos)
    {
        //更新防御罩的血量
        bossPanel.UpdateDefenceHp((float)defenceAble.health/(float)defaultDefenceHp);
    }

    public void OnDefenceDead(string resetPos)
    {
        currentStatus = GunnerStatus.Disable;
        animator.SetTrigger("trigger");
        //隐藏攻击红线1
        attack1Line.gameObject.SetActive(false);
        //停止攻击
        StopAttack();
        //更新防御罩的血量
        bossPanel.UpdateDefenceHp((float)defenceAble.health / (float)defaultDefenceHp);
        //恢复状态
        Invoke("ResetDefence", 5f);
    }

    public void ResetDefence()
    {
        //开始攻击
        attack1Line.gameObject.SetActive(true);
        //恢复防御罩的血量
        bossPanel.UpdateDefenceHp((float)defenceAble.health / (float)defaultDefenceHp);
        StartAttack();
        currentStatus = GunnerStatus.Idle;
        defenceAble.health = defaultDefenceHp;
    }

    #endregion


}
