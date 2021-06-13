using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动平台开关
/// </summary>
public class PressurePad : SwitchBase
{
    private GameObject on_light;
    /// <summary>
    /// 触碰到的物体数量
    /// </summary>
    private int objCount;
    /// <summary>
    /// 控制开关的目标数组
    /// </summary>
    public GameObject[] openableTargets;


    protected override void Start()
    {
        base.Start();
        on_light = transform.Find("on_light").gameObject;
        if(on_light!=null)
            on_light.SetActive(false);
        //数组长度为0，则说明没有数据，将以前的数据设置上去
        if (openableTargets.Length == 0)
        {
            openableTargets = new GameObject[] { openableTarget };
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != TagConst.Ground && collision.name != "attackRange")
        {
            Open();
            objCount++;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objCount--;
        if(objCount<=0)
            Close();
    }

    public override void Open()
    {
        if (openableTarget == null)
        {
            base.Open();
        }
        else
        {
            switchStatus = SwitchStatus.Open;
            spriteRenderer.sprite = statusSprites[(int)switchStatus]; //把开关设置为打开状态的精灵
            //触发对应的事件
            OnOpen();
            for(int i = 0; i < openableTargets.Length; i++)
            {
                if (openableTargets[i].GetComponent<Openable>() != null)
                {
                    openableTargets[i].GetComponent<Openable>().Open();
                }
            }
        }
    }

    public override void Close()
    {
        if (openableTarget == null)
        {
            base.Close();
        }
        else
        {
            switchStatus = SwitchStatus.Close;
            spriteRenderer.sprite = statusSprites[(int)switchStatus]; //把开关设置为打开状态的精灵
            //触发对应的事件
            OnClose();
            for (int i = 0; i < openableTargets.Length; i++)
            {
                if (openableTargets[i].GetComponent<Openable>() != null)
                {
                    openableTargets[i].GetComponent<Openable>().Close();
                }
            }
        }
    }

    public override void OnOpen()
    {
        base.OnOpen();
        on_light.SetActive(true);
    }

    public override void OnClose()
    {
        base.OnClose();
        on_light.SetActive(false);
    }
}
