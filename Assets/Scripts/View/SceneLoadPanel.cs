using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///加载界面
///</summary>
public class SceneLoadPanel : SingletonView<SceneLoadPanel>
{

    #region  字段

    /// <summary>
    /// 进度条
    /// </summary>
    private Slider slider_process;
    /// <summary>
    /// 当前加载的场景
    /// </summary>
    private AsyncOperation currentLoadScene;


    #endregion


    #region  Unity回调

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        slider_process = transform.Find("bg/Slider").GetComponent<Slider>();
    }

    private void Update()
    {
        if(currentLoadScene != null)
        {
            UpdateProcess(currentLoadScene.progress);
        }
    }

    public override void Hide()
    {
        base.Hide();
        currentLoadScene = null;
    }

    #endregion

    #region  方法

    /// <summary>
    /// 更新进度
    /// </summary>
    /// <param name="process">进度条状态</param>
    public void UpdateProcess(float process)
    {
        this.Show();
        this.slider_process.value = process;

        if(process >= 1)    //  说明加载完成
        {
            //this.Hide();
            //延迟1秒隐藏
            Invoke("Hide",1);
        }

    }

    /// <summary>
    /// 更新加载场景的进度
    /// </summary>
    /// <param name="asyncOperation"></param>
    public void UpdateProcess(AsyncOperation asyncOperation)
    {
        this.Show();
        currentLoadScene = asyncOperation;
    }

    #endregion


}
