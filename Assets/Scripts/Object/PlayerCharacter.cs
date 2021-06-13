using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家行为状态
/// </summary>
public enum PlayerStatus
{
    Idle = 0,
    Jump = 1,
    Run = 2,
    Crouch = 3
}

/// <summary>
/// 玩家攻击的类型
/// </summary>
public enum AttackType
{
    Attack = 0, //普通攻击
    Shoot = 1   //射击
}

///<summary>
///控制玩家行为核心脚本
///</summary>
public class PlayerCharacter : MonoBehaviour
{

    #region  字段

    /// <summary>
    /// 获取角色刚体组件
    /// </summary>
    private new Rigidbody2D rigidbody2D;
    /// <summary>
    /// 渲染人物
    /// </summary>
    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// 人物在x轴上移动的速度
    /// </summary>
    public float speedX;
    /// <summary>
    /// 人物在y轴上移动的速度
    /// </summary>
    public float speedY;
    /// <summary>
    /// 角色动画控制机
    /// </summary>
    private Animator animator;
    /// <summary>
    /// Y轴跳跃计时器
    /// </summary>
    private float timeY;
    /// <summary>
    /// 检测人物是否在地面上
    /// </summary>
    private bool isGround;
    /// <summary>
    /// 人物跳跃状态
    /// </summary>
    private bool jump;
    /// <summary>
    /// 玩家当前状态
    /// </summary>
    public PlayerStatus currentStatus = PlayerStatus.Idle;
    /// <summary>
    /// 相机跟随的目标
    /// </summary>
    private Transform followTarget;
    /// <summary>
    /// 跟随偏移
    /// </summary>
    public Vector3 followTargetOffset;
    /// <summary>
    /// 当前玩家所处在的平台
    /// </summary>
    private PassPlatform currentPlatform;
    /// <summary>
    /// 跳跃射线检测的位置
    /// </summary>
    public Transform startCheckPos;

    /// <summary>
    /// 玩家受伤出生的位置
    /// </summary>
    private string ResetPos;
    //攻击的冷却时间
    private float attackTimer = 0.4f;
    //是否已经结束冷却
    private bool attackIsReady = true;

    /// <summary>
    /// 设置X方向速度的时间
    /// </summary>
    private float setSpeedXTime;
    /// <summary>
    /// 设置X方向速度的计时器
    /// </summary>
    private float setSpeedXTimer;
    /// <summary>
    /// 设置X方向的速度大小
    /// </summary>
    private float setSpeedX;

    /// <summary>
    /// 人物近战攻击范围
    /// </summary>
    private AttackRange attackRange;
    /// <summary>
    /// 人物可受伤组件
    /// </summary>
    private Damageable playerDamageable;
    /// <summary>
    /// 造成伤害组件
    /// </summary>
    private Damage playerDamage;

    /// <summary>
    /// 子弹生成的位置
    /// </summary>
    private Transform bulletSpawnPos;
    /// <summary>
    /// 子弹的预制体
    /// </summary>
    private GameObject bulletPrefab;
    /// <summary>
    /// 默认是否有武器
    /// </summary>
    public bool defaultHaveWeapon;

    /// <summary>
    /// 当前正在推的游戏物体
    /// </summary>
    private Pushable currentPushable;

    /// <summary>
    /// 菜单
    /// </summary>
    public GameObject PlayerMenu;

    //推动物体相关的变量
    public ContactFilter2D pushContactFilter;
    public ContactPoint2D[] pushContactPoint = new ContactPoint2D[5];

    #endregion

    #region  Unity回调

    private void Start()
    {
        //初始化刚体、人物精灵、动画状态机
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        //相机跟随物体位置
        followTarget = transform.Find("followTarget");
        followTarget.position = transform.position + followTargetOffset;
        //初始化受伤功能
        playerDamageable = transform.GetComponent<Damageable>();
        playerDamageable.OnHurt += this.OnHurt;  //注册受伤事件
        playerDamageable.OnDead += this.OnDead;  //注册死亡事件、
        //初始化造成伤害
        playerDamage = transform.GetComponent<Damage>();
        //初始化和保存血量

        //当有数据时初始化不再更新，游戏中只更新一次
        if (DataManager.Instance.IsContainsData(DataConst.hp) == false)
        {
            //保存最大的血量和当前血量
            SaveMaxHp(playerDamageable.health);
            UpdateHp(playerDamageable.health);
        }
        else
        {
            playerDamageable.health = GetHpFromData();
        }

        GamePanel.Instance.InitHP(GetMaxHp(), GetHpFromData());
        //初始化攻击范围物体
        attackRange = transform.Find("attackRange").GetComponent<AttackRange>();
        //初始化子弹位置
        bulletSpawnPos = transform.Find("bulletSpawnPos");

        //判断默认是否有武器(初始化武器)
        if (defaultHaveWeapon)
        {
            Data<bool> data = new Data<bool>();
            data.value1 = true;
            DataManager.Instance.SaveData(DataConst.is_have_weapon, data);
        }
    }

    private void Update()
    {
        //更新速度
        UpdateVelocity();
        //更新攻击移动速度
        UpdateSetSpeedWithTime();
        //下蹲/跳跃 动画设置
        UpdateAnimator();
        //检测是否处于空中状态
        CheckGround();
        //更新人物行为状态
        UpdateStatus();
        //跟新跟随目标物体的位置
        UpdateFollowTargetPos();
        //显示菜单
        ShowPlayerMenu();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentPlatform = collision.gameObject.GetComponent<PassPlatform>();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag==TagConst.PassPlatform)
            currentPlatform = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConst.Pushable)
        {
            currentPushable = collision.transform.GetComponent<Pushable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagConst.Pushable)
        {
            currentPushable = null;
        }
    }

    #endregion

    #region  方法

    /// <summary>
    /// 设置x轴上的移动
    /// </summary>
    /// <param name="x"></param>
    public void SetSpeedX(float x)
    {
        //当人物速度不等于0的时候设置为奔跑状态动画
        animator.SetBool("isRun", x != 0);

        //当x小于0时玩家向左移动，将玩家渲染设置朝向左
        //当x大于0时玩家向右移动，将玩家渲染设置朝向右

        if (x < 0)
        {
            spriteRenderer.flipX = true;
            attackRange.transform.localPosition = new Vector3(-1.63f, attackRange.transform.localPosition.y, 0);
        }
        else if (x > 0)
        {
            spriteRenderer.flipX = false;
            attackRange.transform.localPosition = new Vector3(1.63f, attackRange.transform.localPosition.y, 0);
        }
        //如果玩家处于下蹲状态不能移动
        if (currentStatus == PlayerStatus.Crouch)
            x = 0;
        rigidbody2D.velocity = new Vector2(x, rigidbody2D.velocity.y);
    }

    /// <summary>
    /// 更新攻击移动速度
    /// </summary>
    public void UpdateSetSpeedWithTime()
    {
        setSpeedXTimer += Time.deltaTime;
        if (setSpeedXTimer < setSpeedXTime)
        {
            rigidbody2D.velocity = new Vector2(setSpeedX, rigidbody2D.velocity.y);
        }
    }

    /// <summary>
    /// 设置普通攻击时伴随的位移
    /// </summary>
    /// <param name="x"></param>
    /// <param name="time"></param>
    public void SetSpeedXWithTime(float x, float time)
    {
        setSpeedXTime = time;
        setSpeedXTimer = 0;
        setSpeedX = x;
    }

    /// <summary>
    /// 设置y轴上的移动
    /// </summary>
    /// <param name="y"></param>
    public void SetSpeedY(float y)
    {
        //如果玩家处于下蹲状态不能跳跃
        if (currentStatus == PlayerStatus.Crouch)
            y = 0;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, y);
    }

    /// <summary>
    /// 控制玩家跳跃
    /// </summary>
    public bool UpdateJump()
    {
        if (PlayerInput.instance.Jump.Down && isGround)
        {
            timeY = 0;
            jump = true;
        }

        if (PlayerInput.instance.Jump.Hold && jump)
        {
            timeY += Time.deltaTime;
            if (timeY < 0.2f)
                jump = true;
            else
                jump = false;
        }

        if (PlayerInput.instance.Jump.Up)
        {
            jump = false;
        }

        return jump;
    }

    /// <summary>
    /// 发射射线检测是否离开地面在空中
    /// </summary>
    public void CheckGround()
    {
        //物体当前位置  检测方向  检测距离  检测第八层级的物体
        RaycastHit2D raycastHit2D = Physics2D.Raycast(startCheckPos.position, Vector3.down, 0.6f, 1 << 8);
        isGround = raycastHit2D;

        ////判断游戏物体是否触碰到地面
        //if (raycastHit2D)
        //{
        //    //判断是否触碰到的是空中的地面
        //    if (raycastHit2D.collider.tag == TagConst.SkyGround)
        //    {
        //        //碰撞点是否小于碰撞物体的高度 如果是则还没到上面的地面
        //        if(raycastHit2D.point.y < raycastHit2D.transform.position.y)
        //        {
        //            isGround = false;
        //        }
        //        else
        //        {
        //            isGround = true;
        //        }
        //    }
        //    else
        //    {
        //        isGround = true;
        //    }
        //}
        //else
        //{
        //    isGround = false;
        //}

        //Debug.DrawLine(startCheckPos.position, startCheckPos.position + Vector3.down * 0.6f, Color.red);
        //if (raycastHit2D)
        //{
        //    Debug.Log(raycastHit2D.collider.gameObject.name);

        //}
    }

    /// <summary>
    /// 人物跳跃/下蹲动画设置
    /// </summary>
    public void UpdateAnimator()
    {
        //当isGround为true的时候即在地面上，不设置跳跃动画，否则设置
        animator.SetBool("isJump", !isGround);
        animator.SetFloat("speedY", this.rigidbody2D.velocity.y);
        animator.SetBool("isCrouch", PlayerInput.instance.Vertical.value == -1);
    }

    /// <summary>
    /// 更新玩家状态
    /// </summary>
    public void UpdateStatus()
    {
        currentStatus = PlayerStatus.Idle;
        if (rigidbody2D.velocity.x != 0)
            currentStatus = PlayerStatus.Run;
        if (!isGround)
            currentStatus = PlayerStatus.Jump;
        if (PlayerInput.instance.Vertical.value == -1 && isGround)
            currentStatus = PlayerStatus.Crouch;

        //判断下落
        if (PlayerInput.instance.Vertical.value == -1 && isGround && PlayerInput.instance.Jump.Down)
        {
            //当处于可下落的平台时，下蹲状态按下跳跃可进行下落
            if (currentPlatform != null)
            {
                currentPlatform.Fall(gameObject);
                animator.SetTrigger("fall");
            }
        }

        //普通攻击
        if (PlayerInput.instance.Attack.Down || PlayerInput.instance.Attack.Hold)
        {
            Attack(AttackType.Attack);
        }

        //射击
        if (PlayerInput.instance.Shoot.Down || PlayerInput.instance.Shoot.Hold)
        {
            Attack(AttackType.Shoot);
        }
    }

    /// <summary>
    /// 更新移动速度
    /// </summary>
    public void UpdateVelocity()
    {
        //更新x方向速度
        SetSpeedX(PlayerInput.instance.Horizontal.value * speedX);
        //更新y方向速度
        if (UpdateJump())
            SetSpeedY(speedY);
        //更新当前推动的物体的移动速度
        if (currentPushable != null)
        {
            //播放推箱子的动画
            animator.SetBool("isPush", true && PlayerInput.instance.Horizontal.value != 0);

            //判断游戏物体在人物左边还是右边
            bool isLeft = transform.position.x - currentPushable.transform.position.x > 0;
            float speed = PlayerInput.instance.Horizontal.value * speedX * 0.5f;
            //当isLeft为true时，物体在人物的右边
            if (isLeft)
            {
                if (speed > 0)
                {
                    speed = 0;
                }
            }

            bool isRight = transform.position.x - currentPushable.transform.position.x < 0;
            if (isRight)
            {
                if (speed < 0)
                {
                    speed = 0;
                }
            }

            currentPushable.Move(speed);
        }
        else
        {
            animator.SetBool("isPush", false);
        }
            
    }

    /// <summary>
    /// 跟新跟随目标的位置
    /// </summary>
    public void UpdateFollowTargetPos()
    {
        if (spriteRenderer.flipX)
        {
            followTarget.position = Vector3.MoveTowards(followTarget.position, transform.position - followTargetOffset, 0.1f);
        }
        else
        {
            followTarget.position = Vector3.MoveTowards(followTarget.position, transform.position + followTargetOffset, 0.1f);
        }
    }

    /// <summary>
    /// 人物攻击方法
    /// </summary>
    public void Attack(AttackType attackType)
    {
        //如果返回false，则玩家还没有武器不能进行攻击
        if (!IsHaveWeapon()) return;
        //如果attackIsReady为false，则处于冷却时间
        if (!attackIsReady) { return; }

        //设置攻击的动画
        animator.SetTrigger("attack");
        animator.SetInteger("attackType", (int)attackType);

        //设置普通攻击时伴随的位移
        if (attackType == AttackType.Attack)
        {
            SetSpeedXWithTime(spriteRenderer.flipX ? -5 : 5, 0.2f);
        }
        else
        {
            //将射击状态设置为1
            animator.SetFloat("shoot", 1);
            //创建子弹
            Invoke("CreateBullet", 0.2f);
        }


        attackIsReady = false;
        Invoke("ResetAttackIsReady", attackTimer);
    }

    /// <summary>
    /// 解除攻击冷却
    /// </summary>
    public void ResetAttackIsReady()
    {
        attackIsReady = true;
        //将射击状态设置重制
        animator.SetFloat("shoot", 0);
    }

    /// <summary>
    /// 创建子弹
    /// </summary>
    public void CreateBullet()
    {
        if (bulletPrefab == null)
        {
            bulletPrefab = Resources.Load<GameObject>("Prefabs/Object/Bullet");
        }
        GameObject gameObject = GameObject.Instantiate(bulletPrefab);

        //如果翻转了，则改变子弹生成位置反向
        if (spriteRenderer.flipX)
        {
            bulletSpawnPos.localPosition = new Vector3(-bulletSpawnPos.localPosition.x, bulletSpawnPos.localPosition.y, bulletSpawnPos.localPosition.z);
        }

        gameObject.transform.position = bulletSpawnPos.position;
        gameObject.GetComponent<Bullet>().SetDirection(!spriteRenderer.flipX);

        //播放音效
        AudioManager.Instance.PlaySound("Audio/Ellen/Attacks/Ranged/EllenRangedAttack01");
    }

    /// <summary>
    /// 判断玩家是否有武器
    /// </summary>
    /// <returns></returns>
    public bool IsHaveWeapon()
    {
        Data data = DataManager.Instance.GetData(DataConst.is_have_weapon);
        if (data != null && ((Data<bool>)data).value1)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检查可以推动的物体
    /// </summary>
    //public void CheckPushableObj()
    //{
    //    //count为接触到平台游戏物体的数量
    //    int count = rigidbody2D.GetContacts(pushContactFilter, pushContactPoint);
    //    //当不接触时不推动物体
    //    if (count == 0)
    //    {
    //        currentPushable = null;
    //        return;
    //    }
    //    //将获取到的游戏物体加上移动平台的速度
    //    for (int i = 0; i < count; i++)
    //    {
    //        if (pushContactPoint[i].rigidbody != null)
    //        {
    //            if(pushContactPoint[i].rigidbody.gameObject.tag == TagConst.Pushable)
    //            {
    //                currentPushable = pushContactPoint[i].rigidbody.transform.GetComponent<Pushable>();
    //                break;
    //            }
    //            else
    //            {
    //                currentPushable = null;
    //            }
    //        }
    //    }
    //}

    public void PlayAudio(string audioName)
    {
        AudioManager.Instance.PlaySound("Audio/Ellen/"+audioName);
    }

    public void ShowPlayerMenu()
    {
        if (PlayerInput.instance.Pause.Down || PlayerInput.instance.Pause.Hold)
        {
            PlayerMenu.SetActive(true);
        }
    }

    #endregion

    #region  人物受伤方法

    /// <summary>
    /// 更新玩家血量
    /// </summary>
    /// <param name="hp"></param>
    public void UpdateHp(int hp)
    {
        Data data = DataManager.Instance.GetData(DataConst.hp);
        if (data == null)
        {
            data = new Data<int>();
            ((Data<int>)data).value1 = hp;
            DataManager.Instance.SaveData(DataConst.hp, data);
        }
        else
        {
            ((Data<int>)data).value1 = hp;
        }
    }

    /// <summary>
    /// 获取玩家血量
    /// </summary>
    /// <returns></returns>
    public int GetHpFromData()
    {
        Data<int> data = (Data<int>)DataManager.Instance.GetData(DataConst.hp);
        return data.value1;
    }

    /// <summary>
    /// 保存血量上限
    /// </summary>
    public void SaveMaxHp(int maxHp)
    {
        Data<int> data = new Data<int>();
        data.value1 = maxHp;
        DataManager.Instance.SaveData(DataConst.maxHp, data);
    }

    /// <summary>
    /// 获取血量上限
    /// </summary>
    public int GetMaxHp()
    {
        Data<int> data = (Data<int>)DataManager.Instance.GetData(DataConst.maxHp);
        return data.value1;
    }


    /// <summary>
    /// 设置玩家无敌状态的方法
    /// </summary>
    /// <param name="time">无敌状态的时间</param>
    public void SetIsinvincible(int time)
    {
        animator.SetBool("Isinvincible", true);
        playerDamageable.Disable();
        Invoke("ResetDamageable", time);

        //不会与敌人层碰撞
        //Physics2D.SetLayerCollisionMask(LayerMask.NameToLayer("player"),~LayerMask.GetMask("enemy", "ignorePlayer"));
    }

    /// <summary>
    /// 人物受伤方法
    /// </summary>
    public void OnHurt(HurtType hurtType, string ResetPos)
    {
        //更新玩家血量
        UpdateHp(playerDamageable.health);

        this.ResetPos = ResetPos;
        switch (hurtType)
        {
            case HurtType.Normal:
                //播放受伤动画
                animator.SetTrigger("hurt");
                //设置无敌状态  //角色受伤之后一秒内无敌
                SetIsinvincible(1);
                break;
            case HurtType.Dead:
                SetDead();
                //重制玩家位置
                Invoke("ResetDead", 1f);
                break;
        }

        //更新血条
        GamePanel._instance.UpdateHp(GetHpFromData());
    }

    private void SetDead()
    {
        //播放死亡动画
        animator.SetBool("isdead", true);
        animator.SetTrigger("isDead");

        PlayerInput.instance.SetEnable(false);

        //显示提示
        TipMessagePanel.Instance.ShowTip(null, TipStyle.Style2);

    }

    /// <summary>
    /// 解除无敌，恢复可受伤状态
    /// </summary>
    public void ResetDamageable()
    {
        playerDamageable.Enable();
        animator.SetBool("Isinvincible", false);
        //恢复与敌人层碰撞
        //Physics2D.SetLayerCollisionMask(LayerMask.NameToLayer("player"), LayerMask.GetMask("enemy"));
    }

    /// <summary>
    /// 从死亡状态中恢复
    /// </summary>
    public void ResetDead()
    {
        animator.SetBool("isdead", false);
        //rigidbody2D.gravityScale = 5;
        PlayerInput.instance.SetEnable(true);
        //设置无敌状态  
        SetIsinvincible(1);
        //设置位置
        transform.position = GameObject.Find(ResetPos).transform.position;
        GamePanel._instance.UpdateHp(GetHpFromData());
    }

    /// <summary>
    /// 人物死亡方法
    /// </summary>
    public void OnDead(string resetPos)
    {
        //设置死亡后出生地点
        ResetPos = resetPos;
        //设置成死亡状态
        SetDead();
        //延迟显示游戏结束页面
        Invoke("DelayShowGameOverPanel", 2f);
        //更新血条
        GamePanel._instance.UpdateHp(GetHpFromData());
    }

    public void DelayShowGameOverPanel()
    {
        //显示游戏结束的界面
        TipMessagePanel.Instance.ShowTip(null, TipStyle.Style3);
        //重制死亡状态
        ResetDead();
        //重制HP
        GamePanel.Instance.ResetHP();
        playerDamageable.ResetHealth();
    }

    /// <summary>
    /// 人物攻击造成伤害
    /// </summary>
    public void AttackDamage()
    {
        //获取到所有可以攻击的游戏物体
        GameObject[] damageables = attackRange.GetDamageableGameObjects();
        if (damageables != null && damageables.Length != 0)
        {
            //对范围内所有物体造成伤害
            playerDamage.onDamage(damageables);
        }
    }

    #endregion

}
