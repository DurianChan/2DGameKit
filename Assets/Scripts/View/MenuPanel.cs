using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///主界面
///</summary>
public class MenuPanel : ViewBase
{
    #region  字段

    /// <summary>
    /// 选项界面
    /// </summary>
    public OptionPanel optionPanel;
    /// <summary>
    /// 开始游戏
    /// </summary>
    public Button btn_start;
    /// <summary>
    /// 返回游戏
    /// </summary>
    public Button btn_back;
    /// <summary>
    /// 选项按钮
    /// </summary>
    private Button btn_option;
    /// <summary>
    /// 退出按钮
    /// </summary>
    private Button btn_exit;
    /// <summary>
    /// 需要切换的场景
    /// </summary>
    public int targetScene;
    /// <summary>
    /// 是否是玩家菜单
    /// </summary>
    public bool isPlayerMenu = false;

    #endregion

    #region  点击事件

    /// <summary>
    /// 初始化按钮
    /// </summary>
    public void InitializeButton()
    {
        //初始化按钮
        btn_option = transform.Find("bg/btn_option").GetComponent<Button>();
        btn_option.onClick.AddListener(OnOptionClick);
        btn_exit = transform.Find("bg/btn_exit").GetComponent<Button>();
        btn_exit.onClick.AddListener(OnExitClick);

    }

    public void StartButton()
    {
        btn_start = transform.Find("bg/btn_start").GetComponent<Button>();
        btn_start.onClick.AddListener(OnStartGameClick);
    }

    public void BackButton()
    {
        btn_back = transform.Find("bg/btn_back").GetComponent<Button>();
        btn_back.onClick.AddListener(OnBackGameClick);
    }

    /// <summary>
    /// 开始游戏按钮
    /// </summary>
    public void OnStartGameClick()
    {
        SceneController.Instance.LoadScence(targetScene);
        AudioManager.Instance.PlaySound("Audio/UI/MenuButton01");
    }

    public void OnBackGameClick()
    {
        GameObject.Find("PlayerMenu").SetActive(false);
    }

    /// <summary>
    /// 选项按钮
    /// </summary>
    public void OnOptionClick()
    {
        //隐藏自己
        this.Hide();
        //显示选项界面
        optionPanel.Show();
        AudioManager.Instance.PlaySound("Audio/UI/MenuButton02");
    }

    /// <summary>
    /// 退出按钮
    /// </summary>
    public void OnExitClick()
    {
        //if (Application.isEditor)
        //    EditorApplication.isPlaying = false;
        //else
        //    Application.Quit();
        AudioManager.Instance.PlaySound("Audio/UI/MenuButtonBack");

#if UNITY_EDITOR    //判断是不是在编译器模式下
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion

    #region  Unity回调

    private void Start()
    {
        //窗口化游戏
        //Screen.fullScreen = false;

        AudioManager.Instance.PlayMusic("Audio/Music/MusicGameplay");
        InitializeButton();
        if (!isPlayerMenu)
        {
            StartButton();
        }
        else
        {
            BackButton();
        }
    }

    #endregion

}
