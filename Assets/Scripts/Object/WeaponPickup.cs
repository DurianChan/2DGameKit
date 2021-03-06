using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///玩家拾取武器控制脚本
///</summary>
public class WeaponPickup : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// 有武器和无武器状态
    /// </summary>
    public Sprite haveWeapon, noWeapon;



    private void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        InitData();
    }

    /// <summary>
    /// 初始化武器数据
    /// </summary>
    public void InitData()
    {
        //获取当前玩家是否有武器
        Data<bool> data = (Data<bool>)DataManager.Instance.GetData(DataConst.is_have_weapon);
        //如果玩家有武器，则拾取武器地点设置为无武器状态
        if (data != null && data.value1)
        {
            spriteRenderer.sprite = noWeapon;
        }
        else
        {
            spriteRenderer.sprite = haveWeapon;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConst.Player)
        {
            spriteRenderer.sprite = noWeapon;
            //对数据进行保存
            Data<bool> data = new Data<bool>();
            data.value1 = true;
            DataManager.Instance.SaveData(DataConst.is_have_weapon, data);

            TipMessagePanel.Instance.ShowTip("恭喜您获得武器，可以按 K 或 O 进行攻击!",TipStyle.Style1);
            Invoke("HideTip",2f);

            this.GetComponent<BoxCollider2D>().enabled = false;
        }

    }

    /// <summary>
    /// 隐藏提示
    /// </summary>
    public void HideTip()
    {
        TipMessagePanel.Instance.HideTip(TipStyle.Style1);
    }

}
