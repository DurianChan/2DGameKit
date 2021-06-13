using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制地刺陷阱脚本
/// </summary>
public class Spikes : MonoBehaviour
{

    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private Damage damage;

    public GameObject attackObject;

    private void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        damage = transform.GetComponent<Damage>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //damage.onDamage(collision.gameObject);
        attackObject = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        attackObject = null;
    }

    private void Update()
    {
        if(attackObject!=null)
            damage.onDamage(attackObject);
    }

}
