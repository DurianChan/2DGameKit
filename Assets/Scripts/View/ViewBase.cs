using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///界面类
///</summary>
public class ViewBase : MonoBehaviour
{
    /// <summary>
    /// 显示界面
    /// </summary>
    public virtual void Show()
    {
        transform.gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏界面
    /// </summary>
    public virtual void Hide()
    {
        transform.gameObject.SetActive(false);
    }

    /// <summary>
    /// 判断是否显示
    /// </summary>
    /// <returns></returns>
    public bool IsShow()
    {
        return gameObject.activeSelf;
    }

}
