using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平台移动类型
/// </summary>
public enum MovingType
{
    Horizontal, //水平
    Vertical  //竖直
}


/// <summary>
/// 移动平台控制基础类
/// </summary>
public class MovingPlatformBase : MonoBehaviour
{
    #region  字段

    public MovingType movingType = MovingType.Horizontal;

    public Vector3 startPos;//开始的位置
    public Vector3 endPos;//终点的位置

    private bool isMoveToEnd = true;   //是不是向结束位置移动

    public float moveSpeed; //平台移动速度

    //获取平台上的物体
    private new Rigidbody2D rigidbody2D;

    public ContactFilter2D contactFilter;
    //存放获取的游戏物体
    private ContactPoint2D[] contactPoint2D = new ContactPoint2D[10];

    /// <summary>
    /// 存放连接的物体的刚体
    /// </summary>
    private List<Rigidbody2D> contacts = new List<Rigidbody2D>();

    #endregion

    #region  方法

    public void FollowObjects()
    {
        //清空集合
        contacts.Clear();
        //count为接触到平台游戏物体的数量
        int count = rigidbody2D.GetContacts(contactFilter, contactPoint2D);
        //将获取到的游戏物体加上移动平台的速度
        for (int i = 0; i < count; i++)
        {
            //若没有重复的则添加进集合中
            if (!contacts.Contains(contactPoint2D[i].rigidbody))
                contacts.Add(contactPoint2D[i].rigidbody);
        }
        if (movingType == MovingType.Horizontal)
        {
            foreach (Rigidbody2D rigid in contacts)
            {
                if (startPos.x < endPos.x)  //开始向右移动
                {
                    rigid.velocity += new Vector2(isMoveToEnd ? moveSpeed : -moveSpeed, 0);
                }
                else  //开始向左移动
                {
                    rigid.velocity += new Vector2(isMoveToEnd ? -moveSpeed : moveSpeed, 0);
                }
            }
        }
    }

    #endregion


    #region  Unity回调

    protected virtual void Start()
    {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();

    }

    protected virtual void Update()
    {
        if (isMoveToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
            if (transform.position == endPos)
                isMoveToEnd = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
            if (transform.position == startPos)
                isMoveToEnd = true;
        }

    }

    protected virtual void LateUpdate()
    {
        //因检测玩家按键改变刚体速度，因此在Update之后执行
        FollowObjects();
    }


    #endregion


}
