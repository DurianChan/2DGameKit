using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///攻击范围类,获取所有攻击范围内可攻击的物体
///</summary>
public class AttackRange : MonoBehaviour
{
    //获取攻击范围内可攻击的物体
    List<GameObject> damageables = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.transform.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageables.Add(damageable.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Damageable damageable = collision.transform.GetComponent<Damageable>();
        if (damageable != null)
        {
            if(!damageables.Contains(damageable.gameObject))
                damageables.Add(damageable.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Damageable damageable = collision.transform.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageables.Remove(damageable.gameObject);
        }
    }

    public GameObject[] GetDamageableGameObjects()
    {
        return damageables.ToArray();
    }

}
