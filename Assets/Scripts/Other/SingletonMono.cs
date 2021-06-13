using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承mono的单例
/// </summary>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //加载界面
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Object/" + typeof(T).Name));
            }
            return _instance;
        }

    }

    protected virtual void Awake()
    {
        _instance = this as T;
    }
}
