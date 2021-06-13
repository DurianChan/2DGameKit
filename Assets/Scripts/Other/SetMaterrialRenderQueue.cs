using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 改变材质的层级
/// </summary>
public class SetMaterrialRenderQueue : MonoBehaviour
{

    public Material material;

    public int renderQueue;

    private void Start()
    {
        material.renderQueue= renderQueue;
    }

}
