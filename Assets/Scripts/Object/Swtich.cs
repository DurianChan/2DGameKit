using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于控制开门的控制器
/// </summary>
public class Swtich : SwitchBase
{
    #region  字段

    /// <summary>
    /// 打开状态的灯光
    /// </summary>
    private GameObject on_light;

    #endregion

    protected override void Start()
    {
        base.Start();
        on_light = transform.Find("on_light").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == TagConst.Bullet || collision.tag == TagConst.Pushable || collision.tag == TagConst.MovingPlatform)
        {
            //开门
            Open();
        }
    }

    public override void OnOpen()
    {
        base.OnOpen();
        //将灯光打开
        on_light.SetActive(true);
        //把碰撞体检测关闭
        transform.GetComponent<BoxCollider2D>().enabled = false;
    }


}
