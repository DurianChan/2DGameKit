using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Openable
{
    //打开调用
    void Open();
    //关闭调用
    void Close();
}

/// <summary>
/// 开关状态
/// </summary>
public enum SwitchStatus
{
    Close = 0, //关闭
    Open = 1  //开启
}

/// <summary>
/// 开关基础类
/// </summary>
public class SwitchBase : MonoBehaviour
{
    #region  字段

    protected SwitchStatus switchStatus = SwitchStatus.Close;
    public Sprite[] statusSprites;
    protected SpriteRenderer spriteRenderer;
    /// <summary>
    /// 打开的目标
    /// </summary>
    public GameObject openableTarget;

    #endregion

    #region  Unity回调

    protected virtual void Start()
    {
        //初始化开关状态
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = statusSprites[(int)switchStatus];
    }

    #endregion

    #region  方法

    /// <summary>
    /// 开关打开
    /// </summary>
    public virtual void Open()
    {
        switchStatus = SwitchStatus.Open;
        spriteRenderer.sprite = statusSprites[(int)switchStatus]; //把开关设置为打开状态的精灵
        //触发对应的事件
        OnOpen();
        if (openableTarget == null) return;
        if (openableTarget.GetComponent<Openable>() != null)
        {
            openableTarget.GetComponent<Openable>().Open();
        }

    }

    /// <summary>
    /// 开关关闭
    /// </summary>
    public virtual void Close()
    {
        switchStatus = SwitchStatus.Close;
        spriteRenderer.sprite = statusSprites[(int)switchStatus]; //把开关设置为打开状态的精灵
        //触发对应的事件
        OnClose();
        if (openableTarget == null) return;
        if (openableTarget.GetComponent<Openable>() != null)
        {
            openableTarget.GetComponent<Openable>().Close();
        }

    }

    /// <summary>
    /// 打开的回调
    /// </summary>
    public virtual void OnOpen()
    {

    }

    /// <summary>
    /// 关闭的回调
    /// </summary>
    public virtual void OnClose()
    {

    }

    #endregion

}
