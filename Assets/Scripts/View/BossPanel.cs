using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// boss血条面板
/// </summary>
public class BossPanel : ViewBase
{
    /// <summary>
    /// boss血条和防御罩血条
    /// </summary>
    private Slider slider_boss;
    private Slider slider_defence;

    private void Start()
    {
        slider_boss = transform.Find("BossHp").GetComponent<Slider>();
        slider_defence = transform.Find("BossShield").GetComponent<Slider>();
    }

    public void UpdateBossHp(float hp)
    {
        slider_boss.value = hp;
    }

    public void UpdateDefenceHp(float hp)
    {
        slider_defence.value = hp;
    }


}
