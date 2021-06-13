using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制boss第三种攻击方法
/// </summary>
public class BulletLighting : MonoBehaviour
{
    private Damage damage;

    private void Start()
    {
        damage = transform.GetComponent<Damage>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Invoke("Hide", 2f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == TagConst.Player)
        {
            damage.onDamage(collision.gameObject);
        }
    }

}
