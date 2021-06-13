using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///菜单选中后显示图片
///</summary>
public class MyButtons : MonoBehaviour
{
    #region 字段

    /// <summary>
    /// 按钮正常状态下的精灵
    /// </summary>
    private Sprite btn_normal_sprite;
    /// <summary>
    /// 按钮图标
    /// </summary>
    private Transform btn_logo;
    /// <summary>
    /// 按钮背景
    /// </summary>
    private Image btn_bg;
    /// <summary>
    /// 当选中显示高亮状态的精灵
    /// </summary>
    public Sprite HightedSprite;

    #endregion

    #region Unity回调
    private void Start()
    {
        btn_logo = transform.Find("btn_logo");
        btn_logo.gameObject.SetActive(false);
        btn_bg = transform.GetComponent<Image>();
        btn_normal_sprite = btn_bg.sprite;
    }

    #endregion


    #region 事件监听

    public void OnPointerEnter()
    {
        SetHighlight(true);
    }

    public void OnPointerExit()
    {
        SetHighlight(false);
    }

    public void OnPointerUp()
    {
        SetHighlight(false);
    }

    #endregion

    #region  方法

    //设置高亮
    public void SetHighlight(bool isLight)
    {
        btn_logo.gameObject.SetActive(isLight);
        btn_bg.sprite = isLight? HightedSprite : btn_normal_sprite;
    }

    #endregion

}
