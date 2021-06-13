using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///游戏人物血条控制
///</summary>
public class GamePanel : SingletonView<GamePanel>
{
    /// <summary>
    /// 血量的预制体
    /// </summary>
    public GameObject hp_item_prefab;
    /// <summary>
    /// 血条的父物体
    /// </summary>
    private Transform hp_parent;
    /// <summary>
    /// 存储血条的数组
    /// </summary>
    private GameObject[] hp_items;

    protected override void Awake()
    {
        base.Awake();
        hp_parent = transform.Find("hp");
    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic("Audio/Music/MusicGameplay");
    }

    /// <summary>
    /// 初始化血量
    /// </summary>
    /// <param name="count"></param>
    public void InitHP(int hp,int currentHp)
    {
        hp_items = new GameObject[hp];
        for (int i = 0; i < hp; i++)
        {
            hp_items[i] = GameObject.Instantiate(hp_item_prefab, hp_parent);
        }
        UpdateHp(currentHp);
    }

    /// <summary>
    /// 更新血量
    /// </summary>
    /// <param name="hp"></param>
    public void UpdateHp(int hp)
    {
        for (int i = hp; i < hp_items.Length; i++)
        {
            if (hp_items[i].GetComponent<Toggle>().isOn)
            {
                hp_items[i].GetComponent<Toggle>().isOn = false;
            }
        }
    }

    public void ResetHP()
    {
        for (int i = 0; i < hp_items.Length; i++)
        {
            hp_items[i].GetComponent<Toggle>().isOn = true;
        }
    }


}
