using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///当检测到人物碰到时，弹出提示
///</summary>
public class InfoSign : MonoBehaviour
{
    #region  字段

    /// <summary>
    /// 正常状态下的精灵和高亮状态下的精灵
    /// </summary>
    public Sprite normalSprite, lightSprite;

    private SpriteRenderer render;

    /// <summary>
    /// 提示内容
    /// </summary>
    public string tipContent;

    /// <summary>
    /// 提示音效
    /// </summary>
    public AudioClip speekClick;

    #endregion

    private void Start()
    {
        render = transform.GetComponent<SpriteRenderer>();  
    }

    /// <summary>
    /// 碰撞触发
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            render.sprite = lightSprite;
            //显示提示
            TipMessagePanel.Instance.ShowTip(tipContent, TipStyle.Style1);
            //播放说话的音乐
            AudioManager.Instance.PlaySound(speekClick);
        }
    }

    /// <summary>
    /// 离开触发
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            render.sprite = normalSprite;
            //隐藏提示
            TipMessagePanel.Instance.HideTip(TipStyle.Style1);
        }
    }

}
