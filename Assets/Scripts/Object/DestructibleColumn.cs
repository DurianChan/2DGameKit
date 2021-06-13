using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可被打碎的石柱
/// </summary>
public class DestructibleColumn : MonoBehaviour
{

    private Damageable damageable;
    private GameObject destoryObj;


    private void Start()
    {
        damageable = transform.GetComponent<Damageable>();
        destoryObj = transform.Find("Destrucible").gameObject;
        damageable.OnDead += this.OnDead;
    }


    public void OnDead(string resetPos)
    {
        destoryObj.SetActive(true);
        transform.GetComponent<BoxCollider2D>().enabled = false;
        transform.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 3f);
    }



}
