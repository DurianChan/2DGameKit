using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 提示的类型枚举
/// </summary>
public enum TipStyle
{
    Style1,  //文字提示演示
    Style2,  //黑屏样式
    Style3   //游戏结束页面
}

///<summary>
///控制提示信息类
///</summary>
public class TipMessagePanel : SingletonView<TipMessagePanel>
{

    #region  字段

    private GameObject style1Obj;
    private GameObject style2Obj;
    private GameObject style3Obj;

    #endregion

    #region  Unity回调

    protected override void Awake()
    {
        base.Awake();
        style1Obj = transform.Find("Style1").gameObject;
        style1Obj.SetActive(false);
        style2Obj = transform.Find("Style2").gameObject;
        style2Obj.SetActive(false);
        style3Obj = transform.Find("Style3").gameObject;
        style3Obj.SetActive(false);
    }


    #endregion

    #region  方法

    public void ShowTip(string content,TipStyle tipStyle)
    {
        switch (tipStyle)
        {
            case TipStyle.Style1:
                style1Obj.SetActive(true);
                style1Obj.transform.Find("Content").GetComponent<Text>().text = content;
                break;
            case TipStyle.Style2:
                style2Obj.SetActive(true);
                Invoke("HideTipStyle2", 2f);
                break;
            case TipStyle.Style3:
                style3Obj.SetActive(true);
                Invoke("HideTipStyle3", 2f);
                break;
        }
    }

    public void HideTipStyle2()
    {
        HideTip(TipStyle.Style2);
    }

    public void HideTipStyle3()
    {
        HideTip(TipStyle.Style3);
    }

    public void HideTip(TipStyle tipStyle)
    {
        switch (tipStyle)
        {
            case TipStyle.Style1:
                style1Obj.SetActive(false);
                break;
            case TipStyle.Style2:
                style2Obj.SetActive(false);
                break;
            case TipStyle.Style3:
                style3Obj.SetActive(false);
                break;
        }
    }

    #endregion
}
