using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///控制有毒的水
///</summary>
public class Water : MonoBehaviour
{

    private Damage damage;

    private void Start()
    {
        damage = transform.GetComponent<Damage>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        damage.onDamage(collision.gameObject);
    }

}
