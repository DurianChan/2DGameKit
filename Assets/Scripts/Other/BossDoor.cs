using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 控制Boss出场的门
/// </summary>
public class BossDoor : MonoBehaviour,Openable
{
    #region  字段

    private Animator door, boss;
    /// <summary>
    /// boss
    /// </summary>
    public GameObject bossObj;
    /// <summary>
    /// boss面板
    /// </summary>
    public BossPanel bossPanel;

    /// <summary>
    /// 相机聚焦的位置
    /// </summary>
    private Transform followTarget;

    #endregion

    #region  Unity回调

    private void Start()
    {
        door = transform.GetComponent<Animator>();
        boss = transform.Find("Boss").GetComponent<Animator>();
        followTarget = transform.Find("followTarget").transform;
    }

    #endregion


    #region  方法

    /// <summary>
    /// 播放boss出场动画
    /// </summary>
    public void PlayBossAnim()
    {
        door.Play("boss_door");
        boss.Play("boss_spawn");
    }

    /// <summary>
    /// 设置boss出场
    /// </summary>
    public void OpenDoor()
    {
        //取消人物的控制
        PlayerInput.instance.SetEnable(false);
        //把相机聚焦到boss身上
        Camera.main.GetComponent<CameraFollowTarget>().SetFollowTarget(followTarget, 40, 2);
        Invoke("PlayBossAnim", 2f);
    }

    /// <summary>
    /// 当开门动画播放完毕之后的回调
    /// </summary>
    public void OnOpenDoorOver()
    {
        boss.gameObject.SetActive(false);
        door.gameObject.SetActive(false);
        //显示boss
        bossObj.SetActive(true);
        //显示bossPanel面板
        bossPanel.Show();
        PlayerInput.instance.SetEnable(true);
        //把相机聚焦到玩家身上
        Camera.main.GetComponent<CameraFollowTarget>().SetFollowTarget(GameObject.Find("Player").transform,46,1);
        GameObject.Find("BossSwitch").GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Open()
    {
        OpenDoor();
    }

    public void Close()
    {
        
    }

    #endregion


}
