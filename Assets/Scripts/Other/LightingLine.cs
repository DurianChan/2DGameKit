using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <summary>
/// boss第三种攻击的光线描绘方法
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class LightingLine : MonoBehaviour
{
    /// <summary>
    /// 线开始的位置
    /// </summary>
    public Transform startPoint;
    /// <summary>
    /// 线结束的位置
    /// </summary>
    public Transform endPoint;
    /// <summary>
    /// 偏移的点的数量
    /// </summary>
    public int pointCount;
    /// <summary>
    /// 连线组件
    /// </summary>
    private LineRenderer lineRenderer;
    /// <summary>
    /// 用于保存连线的点
    /// </summary>
    private List<Vector3> points = new List<Vector3>();
    /// <summary>
    /// 存放x的偏移值
    /// </summary>
    private List<float> pointX = new List<float>();
    /// <summary>
    /// y轴波动范围值
    /// </summary>
    private float range = 0.6f;
    /// <summary>
    /// 波动的频率
    /// </summary>
    public float f = 0.1f;
    /// <summary>
    /// 计时
    /// </summary>
    private float timer;

    //y = k * x + b  斜率和与y轴的截距
    // k = y / x
    //b = y - k * x
    private float k = 0;
    private float b = 0;

    private void Start()
    {
        lineRenderer = transform.GetComponent<LineRenderer>();
        k = (endPoint.position.y - startPoint.position.y) / (endPoint.position.x - startPoint.position.x);
        b = startPoint.position.y - k * startPoint.position.x;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > f)
        {
            //随机值+开始和结束的两条
            lineRenderer.positionCount = pointCount + 2;
            lineRenderer.SetPositions(GetPoints());
            timer = 0;
        }
    }

    public Vector3[] GetPoints()
    {
        points.Clear();
        pointX.Clear();

        points.Add(startPoint.position);
        points.Add(endPoint.position);
        pointX.Add(startPoint.position.x);
        pointX.Add(endPoint.position.x);

        for (int i = 0; i < pointCount; i++)
        {
            pointX.Add(Random.Range(startPoint.position.x, endPoint.position.x));
        }

        pointX.Sort();

        //根据随机的点求y值
        for (int i = 0; i < pointX.Count; i++)
        {
            float y = k * pointX[i] + b;
            //设置开头和结尾的线不进行偏移
            if(i==0 || i == pointX.Count - 1)
            {
                points.Add(new Vector3(pointX[i], y, 0));
            }
            else
            {
                points.Add(new Vector3(pointX[i], y + Random.Range(-range, range), 0));
            }

        }

        return points.ToArray();
    }

}
