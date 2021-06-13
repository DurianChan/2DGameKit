using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

///<summary>
///触发器被触发后调用加载场景
///</summary>
public class ChangeScene : MonoBehaviour
{
    /// <summary>
    /// 需要切换的场景序列
    /// </summary>
    public int targetScene;

    public string posName; //游戏物体位置的名称
    public string objName; //需要设置位置的游戏物体
    public bool isFlip; //是否需要翻转

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (string.IsNullOrEmpty(posName) || string.IsNullOrEmpty(objName))
            {
                SceneController.Instance.LoadScence(targetScene);
            }
            else
            {
                SceneController.Instance.LoadScence(targetScene,objName,posName,isFlip);
            }
        }
    }

}
