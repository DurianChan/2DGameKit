using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Bridge : MonoBehaviour, Openable
{
    private Animator animator;

    private void Start()
    {
        animator = transform.GetComponentInChildren<Animator>();
    }

    public void Close()
    {
        animator.SetBool("isOpen", false);
    }

    public void Open()
    {
        animator.SetBool("isOpen", true);
    }
}
