
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鼠标控制相机旋转、缩放
/// </summary>
public class MouseRotate : MonoBehaviour
{
    enum RotationAxes
    {
        MouseXAndY,
        MouseX,
        MouseY
    }
    RotationAxes axes = RotationAxes.MouseXAndY;

    float sensitivityX = 1f;
    float sensitivityY = 1f;

    float sensitivityXT = 0.1f;
    float sensitivityYT = 0.1f;

    float minimumY = -90;
    float maximumY = -50;
    private float rotationY = 0;

    public Camera c;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("left shift") && Input.GetMouseButton(1))
        {
            Vector3 p0 = c.transform.position;
            Vector3 p01 = p0 - c.transform.right * Input.GetAxisRaw("Mouse X") * sensitivityXT * Time.timeScale;
            Vector3 p03 = p01 - c.transform.up * Input.GetAxisRaw("Mouse Y") * sensitivityYT * Time.timeScale;
            c.transform.position = p03;
        }
        else if (Input.GetMouseButton(1))
        {

            if (axes == RotationAxes.MouseXAndY)
            {
                float rotationX = c.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                c.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
            else if (axes == RotationAxes.MouseX)
            {
                c.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                c.transform.localEulerAngles = new Vector3(-rotationY, c.transform.localEulerAngles.y, 0);
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (c.fieldOfView > 20)
            {
                c.fieldOfView -= 2;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (c.fieldOfView < 100)
            {
                c.fieldOfView += 2;
            }
        }
        if (Input.GetMouseButtonDown(2))
        {
            c.fieldOfView = 60;
        }
    }
}