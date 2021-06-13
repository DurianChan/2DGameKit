using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///背景随着鼠标移动而移动
///</summary>
public class SpriteOffsetWithMouse : MonoBehaviour
{
    /// <summary>
    /// 初始位置
    /// </summary>
    private Vector3 startPosition;

    /// <summary>
    /// 累加每帧的间隔时间
    /// </summary>
    private float totalTime;

    /// <summary>
    ///偏移系数 数字越小偏移幅度越小
    /// </summary>
    public float spriteScaler;

    private void Start()
    {
        spriteScaler = 0.001f;
        startPosition = transform.position;
    }

    private void Update()
    {
        //地图位置随鼠标位置偏移
        transform.position = startPosition + Input.mousePosition * spriteScaler;
        //累加每帧的间隔直到5秒后恢复原来的位置
        totalTime += Time.deltaTime;
        if (totalTime >= 5)
        {
            transform.position = startPosition;
            totalTime = 0;
        }
    }
}
