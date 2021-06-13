using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///控制能够被伤害的物体
///</summary>
public class Damageable : MonoBehaviour
{
    #region  字段

    public int health;  //生命值

    private int defaultHealth;

    public Action<HurtType,string> OnHurt; //受伤事件

    public Action<string> OnDead; //死亡事件

    private bool isEnable = true; //是否可用

    #endregion

    #region  Unity回调

    private void Start()
    {
        this.defaultHealth = health;
    }

    #endregion 

    #region  方法

    /// <summary>
    /// 承受伤害
    /// </summary>
    public void TakeDamage(int damage,HurtType hurtType,string ResetPos)
    {
        //当受伤状态不可用时直接返回
        if (isEnable == false)
            return;

        //已经死亡直接返回
        if (health <= 0)
            return;

        //血量减少
        health--;
        if(health == 0)
        {
            //死亡
            if(OnDead != null)
            {
                OnDead(ResetPos);
            }
        }
        else
        {
            //受伤
            if(OnHurt != null)
            {
                OnHurt(hurtType,ResetPos);
            }
        }
    }

    /// <summary>
    /// 将受伤状态设置为可用
    /// </summary>
    public void Enable()
    {
        isEnable = true;
    }

    /// <summary>
    /// 将受伤状态设置为不可用
    /// </summary>
    public void Disable()
    {
        isEnable = false;
    }

    /// <summary>
    /// 重置血量
    /// </summary>
    /// <param name="health"></param>
    public void ResetHealth()
    {
        this.health = defaultHealth;
        Enable();
    }

    #endregion
}
