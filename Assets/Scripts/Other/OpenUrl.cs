using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///点击跳转链接
///</summary>
public class OpenUrl : MonoBehaviour
{
    public string url;

    public void OpenURL()
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }
        else
        {
            Application.OpenURL(url);
        }
    }
}
