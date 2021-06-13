using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///门的状态变换
///</summary>

//门的状态枚举
public enum HubDoorStatus
{
    zero = 0,
    one = 1,
    two = 2,
    three = 3
}


public class HubDoor : MonoBehaviour
{
    /// <summary>
    /// 存放门的不同状态
    /// </summary>
    public Sprite[] status;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }


    /// <summary>
    /// 设置状态
    /// </summary>
    public void SetSattus(HubDoorStatus s)
    {
        spriteRenderer.sprite = status[(int)s];
    }

}
