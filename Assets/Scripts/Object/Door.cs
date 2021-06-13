using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制门的开关状态
/// </summary>
public class Door : MonoBehaviour,Openable
{

    private Animator animator;

    private void Start()
    {
        animator = transform.Find("sprite").GetComponent<Animator>();
    }

    public void Open()
    {
        animator.SetBool("isOpen",true);
    }

    public void Close()
    {
        animator.SetBool("isOpen", false);
    }

}
