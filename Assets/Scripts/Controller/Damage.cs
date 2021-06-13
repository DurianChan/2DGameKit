using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 受伤类型
/// </summary>
public enum HurtType
{
    Normal,
    Dead
}

///<summary>
///控制造成伤害的脚本
///</summary>
public class Damage : MonoBehaviour
{

    #region  字段

    /// <summary>
    /// 对人物造成的伤害的值
    /// </summary>
    public int damage;
    /// <summary>
    /// 受伤类型
    /// </summary>
    public HurtType hurtType;
    /// <summary>
    /// 重置位置
    /// </summary>
    public string ResetPos;

    #endregion

    #region  方法

    /// <summary>
    /// 对人物造成伤害
    /// </summary>
    public void onDamage(GameObject gameObject)
    {
        Damageable damageable = gameObject.GetComponent<Damageable>();
        //如果物体上没有可受伤组件，则该物体不可受伤直接返回
        if(damageable == null)
            return;

        damageable.TakeDamage(this.damage,hurtType,ResetPos);

    }

    /// <summary>
    /// 对多个物体造成伤害
    /// </summary>
    /// <param name="gameObjects"></param>
    public void onDamage(GameObject[] gameObjects)
    {
        for(int i = 0; i < gameObjects.Length; i++)
        {
            onDamage(gameObjects[i]);
        }
    }

    #endregion

}
