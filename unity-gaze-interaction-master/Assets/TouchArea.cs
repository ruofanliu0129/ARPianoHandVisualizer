using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchArea : MonoBehaviour
{
    /// <summary>
    /// 锁定手指触摸的区域 UI对象作为区域判断条件   （*上图说的红框）
    /// </summary>
    [Header("锁定手指触摸的区域 UI对象作为区域判断条件")]
    public Transform _map;
    float _mapWidth;
    float _mapHight;

    /// <summary>
    /// 获取ui的屏幕坐标
    /// </summary>
    /// <param name="trans">UI物体</param>
    /// <returns></returns>
    private Vector2 GetUiToScreenPos(Transform trans)
    {
        _mapWidth = trans.GetComponent<RectTransform>().rect.width;//获取ui的实际宽度
        _mapHight = trans.GetComponent<RectTransform>().rect.height;//长度
        Vector2 pos2D = trans.position;
        return pos2D;
    }

    /// <summary>
    /// 判断是否在ui上
    /// </summary>
    /// <param name="pos">输入的坐标信息(触摸点数据)</param>
    /// <returns></returns>
    public bool IsTouchInUi(Vector3 pos)
    {
        bool isInRect = false;
        Vector3 newPos = GetUiToScreenPos(_map);
        if (pos.x < (newPos.x + _mapWidth) && pos.x > newPos.x &&
            pos.y < (newPos.y + _mapHight) && pos.y > newPos.y)
        {
            isInRect = true;
        }
        return isInRect;
    }


    //调用 rawPosition这个很关键，用于触摸的原始位置。
    //如果使用的是position，那么意味着触摸区域一旦离开指定区域将会无效；
    //deltaPosition这个我没整明白打印出来的值是什么，也可能是之前写错了代码导致它打印的结果是乱的
    //IsTouchInUi(Input.GetTouch(0).rawPosition);
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
