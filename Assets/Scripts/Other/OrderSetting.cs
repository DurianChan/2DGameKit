using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///改变3D物体的渲染层级
///</summary>
public class OrderSetting : MonoBehaviour
{


    private MeshRenderer meshRenderer;
    /// <summary>
    /// 需要改变为的层级
    /// </summary>
    public int orderInLayer;

    private void Start()
    {
        meshRenderer = transform.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = orderInLayer;
    }

}
