using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDifferent : UnitySingleton<ShowDifferent>
{
    [Header("原钢琴")]public GameObject piano1;
    [Header("新钢琴")] public GameObject piano2;

    [Header("新的钢琴和原钢琴的偏移")] public Vector3 bias = new Vector3(0,0,0);
    [Header("学生左手")] public GameObject student_left;
    [Header("学生右手")] public GameObject student_right;
    [Header("教练左手")] public GameObject coach_left;
    [Header("教练右手")] public GameObject coach_right;
    [Header("是否显示原钢琴上的教练手")] public bool show_origin_coach = false;

    
    private GameObject new_hand_L;
    private GameObject new_hand_R;
    private GameObject new_hand_L_c;
    private GameObject new_hand_R_c;

    private void Start()
    {

        //创建一个新的钢琴
        //newPiano = GameObject.Instantiate(piano, piano.transform.position, piano.transform.rotation);

        //newPiano.transform.position += bias;
        //newPiano.layer = LayerMask.NameToLayer("player");

        //foreach (Transform tran in piano1.GetComponentsInChildren<Transform>(true))
        //{//遍历当前物体及其所有子物体
        //    tran.gameObject.layer = LayerMask.NameToLayer("coach");//更改物体的Layer层
        //}
        //foreach (Transform child in piano2.GetComponentsInChildren<Transform>(true))
        //{
        //    child.gameObject.layer = LayerMask.NameToLayer("player");
        //}

        //关闭/开启原钢琴上的的教练手
        //newPiano.SetActive(true);
        student_left.SetActive(show_origin_coach);
        student_right.SetActive(show_origin_coach);
    }

    private void Update()
    {
        if (new_hand_L !=null )
        {
            new_hand_L.SetActive(false);
            GameObject.Destroy(new_hand_L);
            new_hand_R.SetActive(false);
            GameObject.Destroy(new_hand_R);
        }

        if (new_hand_L_c != null)
        {
            new_hand_L_c.SetActive(false);
            GameObject.Destroy(new_hand_L_c);
            new_hand_R_c.SetActive(false);
            GameObject.Destroy(new_hand_R_c);
        }

        //create student hands
        new_hand_L = GameObject.Instantiate(student_left, student_left.transform.position, student_left.transform.rotation);
        new_hand_R = GameObject.Instantiate(student_right, student_right.transform.position, student_right.transform.rotation);
        new_hand_L.transform.Rotate(90, 0, 0);
        new_hand_R.transform.Rotate(90, 0, 0);
        new_hand_L.transform.position = new Vector3(0.0f, 1.2f, 0.4f);
        new_hand_R.transform.position = new Vector3(0.0f, 1.2f, 0.4f);
        //new_hand_L.transform.position += bias;
        //new_hand_R.transform.position += bias;
        new_hand_L.SetActive(true);
        new_hand_R.SetActive(true);

        //create coach hands
        new_hand_L_c = GameObject.Instantiate(coach_left, coach_left.transform.position, coach_left.transform.rotation);
        new_hand_R_c = GameObject.Instantiate(coach_right, coach_right.transform.position, coach_right.transform.rotation);
        new_hand_L_c.transform.Rotate(90, 0, 0);
        new_hand_R_c.transform.Rotate(90, 0, 0);
        new_hand_L_c.transform.position = new Vector3(0.0f, 1.2f, 0.4f);
        new_hand_R_c.transform.position = new Vector3(0.0f, 1.2f, 0.4f);
        new_hand_L_c.SetActive(true);
        new_hand_R_c.SetActive(true);
    }
}
