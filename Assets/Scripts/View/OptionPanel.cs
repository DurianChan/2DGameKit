using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///选项界面
///</summary>
public class OptionPanel : ViewBase
{

    #region  字段

    /// <summary>
    /// 菜单界面
    /// </summary>
    public MenuPanel menuPanel;
    /// <summary>
    /// 音量界面
    /// </summary>
    public OptionAudioSetPanel optionAudioSetPanel;
    /// <summary>
    /// 操作界面
    /// </summary>
    public OptionOperatorPanel optionOperatorPanel;
    /// <summary>
    /// 信息板
    /// </summary>
    private Transform messagePanel;
    /// <summary>
    /// 返回按钮
    /// </summary>
    private Button btn_back;
    /// <summary>
    /// 音量按钮
    /// </summary>
    private Button btn_audio;
    /// <summary>
    /// 操作按钮
    /// </summary>
    private Button btn_controller;
    
    #endregion

    #region  点击事件

    /// <summary>
    /// 初始化按钮
    /// </summary>
    public void InitializeButton()
    {
        //初始化选项按钮
        btn_back = transform.Find("bg/btn_back").GetComponent<Button>();
        btn_back.onClick.AddListener(OnBackClick);
        btn_audio = transform.Find("bg/btn_audio").GetComponent<Button>();
        btn_audio.onClick.AddListener(OnAudioClick);
        btn_controller = transform.Find("bg/btn_controller").GetComponent<Button>();
        btn_controller.onClick.AddListener(OnOperatorClick);
        //初始化信息板
        messagePanel = transform.Find("bg/MessagePanel");
    }

    /// <summary>
    /// 音量按钮
    /// </summary>
    public void OnAudioClick()
    {
        //隐藏按钮和信息板
        HideOrShowOptionPanel(false);

        optionAudioSetPanel.Show();
    }

    /// <summary>
    /// 操作按钮
    /// </summary>
    public void OnOperatorClick()
    {
        //隐藏按钮和信息板
        HideOrShowOptionPanel(false);

        optionOperatorPanel.Show();
    }

    /// <summary>
    /// 返回按钮
    /// </summary>
    public void OnBackClick()
    {
        //当这两个界面显示时隐藏它们，否则隐藏自己
        if(optionAudioSetPanel.IsShow() || optionOperatorPanel.IsShow())
        {
            //显示按钮和信息板
            HideOrShowOptionPanel(true);

            optionAudioSetPanel.Hide();
            optionOperatorPanel.Hide();
        }
        else
        {
            //隐藏自己，显示菜单界面
            this.Hide();
            menuPanel.Show();
        }
    }

    #endregion

    #region  Unity回调

    private void Start()
    {
        InitializeButton();
    }

    #endregion

    #region  方法

    /// <summary>
    /// 是否显示和隐藏按钮和信息板
    /// </summary>
    /// <param name="isShow">true显示，false隐藏</param>
    private void HideOrShowOptionPanel(bool isShow)
    {
        btn_audio.gameObject.SetActive(isShow);
        btn_controller.gameObject.SetActive(isShow);
        messagePanel.gameObject.SetActive(isShow);
    }

    #endregion

}
