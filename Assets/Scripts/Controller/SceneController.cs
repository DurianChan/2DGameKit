using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

///<summary>
///控制场景加载类
///</summary>
public class SceneController : Singleton<SceneController>
{
    /// <summary>
    /// 加载场景的信息
    /// </summary>
    public AsyncOperation currentLoadOperation;

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="target">加载场景的序列</param>
    public void LoadScence(int target)
    {
        currentLoadOperation = SceneManager.LoadSceneAsync(target);
        //显示一个加载界面
        SceneLoadPanel.Instance.UpdateProcess(currentLoadOperation);
    }


    public void LoadScence(int target, Action<AsyncOperation> onComplete)
    {
        //加载
        currentLoadOperation = SceneManager.LoadSceneAsync(target);
        //显示一个加载界面
        SceneLoadPanel.Instance.UpdateProcess(currentLoadOperation);
        currentLoadOperation.completed += onComplete;
    }

    /// <summary>
    /// 将游戏物体加载到指定的位置
    /// </summary>
    /// <param name="target"></param>
    /// <param name="objName"></param>
    /// <param name="posName"></param>
    public void LoadScence(int target, string objName, string posName,bool isFlipX = false)
    {
        LoadScence(target, (AsyncOperation) =>
        {
            GameObject gameObject = GameObject.Find(objName);
            GameObject posObject = GameObject.Find(posName);

            gameObject.transform.position = posObject.transform.position;

            if(gameObject.GetComponent<SpriteRenderer>() != null)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = isFlipX;
            }

        });
    }

}
