using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///角色输入跳跃、下蹲、攻击、射击状态
///</summary>
public class InputButton
{

    public KeyCode keyCode;
    /// <summary>
    /// 按下状态
    /// </summary>
    public bool Down;
    /// <summary>
    /// 抬起状态
    /// </summary>
    public bool Up;
    /// <summary>
    /// 按住状态
    /// </summary>
    public bool Hold;

    public InputButton(KeyCode keyCode)
    {
        this.keyCode = keyCode;
    }

    /// <summary>
    /// 获取事件状态
    /// </summary>
    public void Get()
    {
        Down = Input.GetKeyDown(this.keyCode);
        Up = Input.GetKeyUp(this.keyCode);
        Hold = Input.GetKey(this.keyCode);
    }

}
