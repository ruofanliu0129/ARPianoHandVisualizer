using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TabletControl : MonoBehaviour
{
    public int oneOrTwoKey=1;
    Camera myCam;
    private Touch _OldTouch1;  //上次触摸点1(手指1)  
    private Touch _OldTouch2;  //上次触摸点2(手指2)  

    // 记录手指触屏的位置  
    Vector2 _M_Screenpos = new Vector2();

    //旋转还是移动布尔
    //public bool _bMoveOrRotation;

    //相机初始位置
    Vector3 _OldPosition;

    public float distance = 4F;

    private Vector2 oldPosition1;
    private Vector2 oldPosition2;

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
    


    void Start()
    {
        //记录开始摄像机的Position
        _OldPosition = Camera.main.transform.position;
        myCam = gameObject.GetComponent<Camera>();
        distance = myCam.fieldOfView;
        //print(myCam.fieldOfView);
        //print(IsTouchInUi(Input.GetTouch(0).rawPosition));
    }

    void Update()
    {
        //没有触摸  
        if (Input.touchCount <= 0)
        {
            return;
        }

        //1 touch  
        if (1 == Input.touchCount && (IsTouchInUi(Input.GetTouch(0).rawPosition)) == true)
        {
            //print(IsTouchInUi(Input.GetTouch(0).rawPosition));
            //true->rotate, false->move
                
            Touch _Touch = Input.GetTouch(0);
            Vector2 _DeltaPos = _Touch.deltaPosition/10;
            //(0, 1, 0).
            //transform.Rotate(Vector3.up * _DeltaPos.x , Space.World); //Rotate around the Y-axis
            if (oneOrTwoKey == 1)
            {
                //(1, 0, 0).
                transform.Rotate(Vector3.right * -_DeltaPos.y, Space.World); //Rotate around the X-axis
                                                                                //(0, 0, 1).
                transform.Rotate(Vector3.down * -_DeltaPos.x, Space.World); //Rotate around the X-axis
            }
            else
            {
                //(1, 0, 0).
                transform.Rotate(Vector3.right * -_DeltaPos.y, Space.World); //Rotate around the X-axis
                //(0, 0, 1).
                transform.Rotate(Vector3.down * -_DeltaPos.x , Space.World); //Rotate around the X-axis
            }
                
           
        }

        if (Input.touchCount > 1 && (IsTouchInUi(Input.GetTouch(0).rawPosition)) == true)
        {
            //move
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                // 记录手指触屏的位置  
                _M_Screenpos = Input.touches[0].position;

            }
            // 手指移动  
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {

                // 移动摄像机  
                transform.Translate(new Vector3(-Input.touches[0].deltaPosition.x * Time.deltaTime * 0.1f, -Input.touches[0].deltaPosition.y * Time.deltaTime * 0.1f, 0));
            }

            //zoom in/zoom out
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {

                Vector3 tempPosition1 = Input.GetTouch(0).position;
                Vector3 tempPosition2 = Input.GetTouch(1).position;

                if (IsEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                {

                    if (distance > 1F)
                    {
                        distance -= 0.3F;
                    }
                }
                else
                {

                    if (distance < 150F)
                    {
                        distance += 0.3F;
                    }
                }

                //piano.transform.localScale = _Scale;
                myCam.fieldOfView = distance;
                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;
            }
        }

    }


    //通过按钮让物体回到最初的位置
    public void BackPosition()
    {
        //位置回归原点
        Camera.main.transform.position = _OldPosition;
        //旋转归零
        Camera.main.transform.eulerAngles = Vector3.zero;
    }

    //设置单指操作方式 旋转还是移动
    //public void Rotation()
    //{
    //    _bMoveOrRotation = true;
    //}
    //public void Move()
    //{
    //    _bMoveOrRotation = false;
    //}

    //judge whether zoom in or out
    bool IsEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {

        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        if (leng1 < leng2)
        {

            return true;
        }
        else
        {

            return false;
        }
    }

}