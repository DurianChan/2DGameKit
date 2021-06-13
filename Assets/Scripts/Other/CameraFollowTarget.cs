using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

///<summary>
///相机跟随人物移动而移动
///</summary>
public class CameraFollowTarget : MonoBehaviour
{
    #region  字段

    /// <summary>
    /// 要跟随的游戏物体
    /// </summary>
    public Transform target;

    public Vector2 rangeMin;    //跟随的最小范围
    public Vector2 rangeMax;    //跟随的最大范围

    private Vector3 targetPos;

    /// <summary>
    /// 相机跟随使用一定的时间移动
    /// </summary>
    private bool isFollowWithTime = false;
    /// <summary>
    /// 延迟的时间
    /// </summary>
    private float delaytime;
    /// <summary>
    /// 计时
    /// </summary>
    private float timer;
    /// <summary>
    /// 开始的位置
    /// </summary>
    private Vector3 startPos;
    /// <summary>
    /// 初始相机缩放值
    /// </summary>
    public float defaultView;
    /// <summary>
    /// 当前相机缩放值
    /// </summary>
    private float currentView;
    /// <summary>
    /// 需要缩放的值
    /// </summary>
    private float targetView;

    public bool isChangeView = false;//是否修改view

    private new Camera camera;

    private Transform defaultTarget;

    #endregion

    #region  方法

    public void Follow()
    {
        if (target == null)
        {
            Debug.LogWarning(transform.name + "要跟随的目标为空");
            return;
        }

       //是否使用延迟时间跟随
        if (isFollowWithTime)
        {
            timer += Time.deltaTime;
            targetPos = Vector3.Lerp(startPos,target.position,timer/delaytime);
            targetPos.z = transform.position.z;
            //是否缩放
            if (isChangeView)
            {
                camera.fieldOfView = Mathf.Lerp(currentView,targetView,timer/delaytime);
            }
            //重置
            if (timer / delaytime > 1)
            {
                isFollowWithTime = false;
                isChangeView = false;
            }
        }
        else
        {
            targetPos = new Vector3(target.position.x, target.position.y + Vector3.up.y, transform.position.z);
        }
 
        transform.position = LimitPos(targetPos);

    }

    /// <summary>
    /// 限制跟随目标的相机位置
    /// </summary>
    /// <param name="targetPos">跟随目标的位置</param>
    /// <returns></returns>
    public Vector3 LimitPos(Vector3 targetPos)
    {
        if(targetPos.x < rangeMin.x)
        {
            targetPos.x = rangeMin.x;
        }
        if(targetPos.y < rangeMin.y)
        {
            targetPos.y = rangeMin.y;
        }
        if (targetPos.x > rangeMax.x)
        {
            targetPos.x = rangeMax.x;
        }
        if (targetPos.y > rangeMax.y)
        {
            targetPos.y = rangeMax.y;
        }
        return targetPos;
    }


    public void SetFollowTarget(Transform target)
    {
        isFollowWithTime = false;
        this.target = target;
    }

    public void SetFollowTarget(Transform target,float time)
    {
        this.target = target;
        isFollowWithTime = true;
        timer = 0;
        delaytime = time;
        startPos = transform.position;
    }

    public void SetFollowTarget(Transform target,float view,float time)
    {
        isChangeView = true;
        targetView = view;
        currentView = camera.fieldOfView;
        SetFollowTarget(target, time);
    }

    /// <summary>
    /// 当物体下落时调用，过一段时间后恢复相机
    /// </summary>
    /// <param name="gameObject">需要聚焦的物体</param>
    /// <param name="time">聚焦时间</param>
    public void SetFollowTarget(GameObject target,int time)
    {
        defaultTarget = target.transform;
        SetFollowTarget(target.transform, time);
        
        Invoke("ResetFollowTarget",3);

    }

    public void ResetFollowTarget()
    {
        SetFollowTarget(defaultTarget, 1);
    }

    #endregion

    #region Unity回调

    private void Start()
    {
        camera = transform.GetComponent<Camera>();
        defaultView = camera.fieldOfView;
    }
    private void Update()
    {
        Follow();
    }


    #endregion

}
