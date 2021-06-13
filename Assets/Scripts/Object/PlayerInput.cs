using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///控制玩家输入行为状态脚本
///</summary>
public class PlayerInput : MonoBehaviour
{

    #region  字段

    /// <summary>
    /// 当前人物是否处于可操作状态
    /// </summary>
    public bool isEnable = true;
    /// <summary>
    /// 设置单例对象
    /// </summary>
    public static PlayerInput instance;

    #endregion



    #region 输入事件

    //PC上为该版本 发布其他平台版本时，需更改变量
    public InputButton Pause = new InputButton(KeyCode.Escape); //暂停
    public InputButton Attack = new InputButton(KeyCode.K);     //攻击 
    public InputButton Shoot = new InputButton(KeyCode.O);      //射击
    public InputButton Jump = new InputButton(KeyCode.Space);   //跳跃

    public InputAxis Horizontal = new InputAxis(KeyCode.A, KeyCode.D);   //水平移动
    public InputAxis Vertical = new InputAxis(KeyCode.S, KeyCode.W);    //竖直移动


    #endregion

    #region  方法

    /// <summary>
    /// 设置人物是否可操控状态
    /// </summary>
    /// <param name="isCanUse"></param>
    public void SetEnable(bool isCanUse)
    {
        this.isEnable = isCanUse;
    }

    #endregion

    #region  Unity回调

    private void Awake()
    {
        if (instance != null)
            throw new System.Exception("PlayerInput存在多个对象！");

        instance = this;
    }

    private void Update()
    {
        if (isEnable==true)
        {
            Pause.Get();
            Attack.Get();
            Shoot.Get();
            Jump.Get();

            Horizontal.Get();
            Vertical.Get();
        }
        else
        {
            Horizontal.value = 0;
            Horizontal.value = 0;
        }
    }

    #endregion





}
