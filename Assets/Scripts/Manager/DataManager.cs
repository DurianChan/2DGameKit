using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 需要保存的数据类型
/// </summary>
public class Data
{

}

public class Data<T> : Data
{
    public T value1;
}

public class Data<T, T1> : Data
{
    public T value1;
    public T1 value2;
}

public class Data<T,T1,T2> : Data
{
    public T value1;
    public T1 value2;
    public T2 value3;
}

///<summary>
///用于保存数据到内存中
///</summary>
public class DataManager : Singleton<DataManager>
{

    Dictionary<string, Data> datas = new Dictionary<string, Data>();

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SaveData(string key, Data value)
    {
        datas[key] = value;
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <returns></returns>
    public Data GetData(string key)
    {
        //数据是否存在
        if (datas.ContainsKey(key))
            return datas[key];
        return null;
    }

    /// <summary>
    /// 是否包含某条数据
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsContainsData(string key)
    {
        return datas.ContainsKey(key);
    }

}
