
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鼠标控制相机旋转、缩放
/// </summary>
public class MouseRotate_singleKey : MonoBehaviour
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

    float minimumY = -45;
    float maximumY = 45;
    private float rotationY = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("left shift") && Input.GetMouseButton(1))
        {
            Vector3 p0 = Camera.main.transform.position;
            Vector3 p01 = p0 - Camera.main.transform.right * Input.GetAxisRaw("Mouse X") * sensitivityXT * Time.timeScale;
            Vector3 p03 = p01 - Camera.main.transform.up * Input.GetAxisRaw("Mouse Y") * sensitivityYT * Time.timeScale;
            Camera.main.transform.position = p03;
        }
        if (Input.GetMouseButton(1))
        {

            if (axes == RotationAxes.MouseXAndY)
            {
                float rotationX = Camera.main.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                Camera.main.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
            else if (axes == RotationAxes.MouseX)
            {
                Camera.main.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                Camera.main.transform.localEulerAngles = new Vector3(-rotationY, Camera.main.transform.localEulerAngles.y, 0);
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView > 20)
            {
                Camera.main.fieldOfView -= 2;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView < 100)
            {
                Camera.main.fieldOfView += 2;
            }
        }
        if (Input.GetMouseButtonDown(2))
        {
            Camera.main.fieldOfView = 60;
        }
    }
}