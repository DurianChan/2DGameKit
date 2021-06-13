using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///玩家获取钥匙检测
///</summary>
public class Key : MonoBehaviour
{

    public static int KeyCount = 0;

    private HubDoor hubDoor;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConst.Player)
        {
            //获取一个钥匙
            KeyCount++;
            
            if (hubDoor == null)
            {
                throw new System.Exception("未在场景中查询到门");
            }

            //把我们的人物设置为不可操作状态
            PlayerInput.instance.SetEnable(false);
            //隐藏钥匙
            gameObject.SetActive(false);
            //聚焦到门
            Camera.main.GetComponent<CameraFollowTarget>().SetFollowTarget(hubDoor.transform,33,1);
            //修改门的状态
            Invoke("ChangeHubDoorStatus", 1.5f);
            //恢复正常
            Invoke("ResetToNormal", 2f);
            //设置人物可操作状态
            PlayerInput.instance.SetEnable(true);
        }
    }

    public void ChangeHubDoorStatus()
    {
        hubDoor.SetSattus((HubDoorStatus)KeyCount);
    }

    /// <summary>
    /// 恢复正常
    /// </summary>
    public void ResetToNormal()
    {
        CameraFollowTarget cameraFollowTarget = Camera.main.GetComponent<CameraFollowTarget>();
        cameraFollowTarget.SetFollowTarget(GameObject.Find("Player/followTarget").transform,cameraFollowTarget.defaultView,1);
        //销毁钥匙
        Destroy(gameObject);
    }

    private void Start()
    {   
        //获取门
        hubDoor = GameObject.Find("HubDoor").GetComponent<HubDoor>();
    }



}
