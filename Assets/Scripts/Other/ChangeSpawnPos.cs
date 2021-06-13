using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 改变出生点
/// </summary>
public class ChangeSpawnPos : MonoBehaviour
{
    /// <summary>
    /// 需要改变的点
    /// </summary>
    public Damage[] needChangeDamage;
    /// <summary>
    /// 目标出生点
    /// </summary>
    public string TargetSpwan;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == TagConst.Player)
        {
            for(int i = 0; i < needChangeDamage.Length; i++)
            {
                needChangeDamage[i].ResetPos = TargetSpwan;
            }
        }
    }

}
