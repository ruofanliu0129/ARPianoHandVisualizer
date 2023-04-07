using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class showDifferentSingleKey : UnitySingleton<showDifferentSingleKey>
{
    [Header("原钢琴")] public GameObject piano;

    [Header("新的钢琴和原钢琴的偏移")] public Vector3 bias = new Vector3(-0.5f, 0, 0);
    [Header("教练左手")] public GameObject coach_left;
    [Header("教练右手")] public GameObject coach_right;
    [Header("是否显示原钢琴上的教练手")] public bool show_origin_coach = true;

    public GameObject newPiano;
    private GameObject new_hand_L;
    private GameObject new_hand_R;

    public GameObject userHand;
    public GameObject coachHand;
    public GameObject Lines;
    public bool jointLine;
    public bool boneColor = true;
    private GameObject[] joint;
    private GameObject[] bone;
    private LineRenderer[] line;
    private GameObject[] coachJoint;
    private GameObject[] coachBone;
    private bool firstframe = true;
    private int bone_num = 20;
    private int joint_num = 21;
    private int hand_num;
    private Color color;

    private void Start()
    {
        //创建一个新的钢琴
        //newPiano = GameObject.Instantiate(piano, piano.transform.position, piano.transform.rotation);
        //newPiano.transform.position += bias;
        //newPiano.layer = LayerMask.NameToLayer("player");
        //foreach (Transform tran in GetComponentsInChildren<Transform>())
        //{//遍历当前物体及其所有子物体
        //    tran.gameObject.layer = LayerMask.NameToLayer("coach");//更改物体的Layer层
        //}
        foreach (Transform child in newPiano.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("player");
        }

        //关闭/开启原钢琴上的的教练手
        coach_left.SetActive(show_origin_coach);
        coach_right.SetActive(show_origin_coach);

        
    }

    private void Update()
    {
        if (new_hand_L != null)
        {
            new_hand_L.SetActive(false);
            GameObject.Destroy(new_hand_L);
            new_hand_R.SetActive(false);
            GameObject.Destroy(new_hand_R);
        }
        new_hand_L = GameObject.Instantiate(coach_left, coach_left.transform.position, coach_left.transform.rotation);
        new_hand_R = GameObject.Instantiate(coach_right, coach_right.transform.position, coach_right.transform.rotation);
        new_hand_L.transform.position += bias;
        new_hand_R.transform.position += bias;
        new_hand_L.SetActive(false);
        new_hand_R.SetActive(false);

        //if (boneColor)
        //{
        //    if (firstframe)
        //    {
        //        //Array.Copy(userHand.GetComponent<Load_CSV_singleKey>().bone, 0, bone, 0, bone_num * hand_num);
        //        //Array.Copy(coachHand.GetComponent<Load_CSV_singleKey>().bone, 0, coachBone, 0, bone_num * hand_num);
        //        Array.Copy(userHand.GetComponent<Load_CSV>().bone, 0, bone, 0, bone_num * hand_num);
        //        Array.Copy(coachHand.GetComponent<Load_CSV>().bone, 0, coachBone, 0, bone_num * hand_num);
        //    }
        //    for (int i = 0; i < bone_num * hand_num; i++)
        //    {
        //        float angle = Quaternion.Angle(bone[i].transform.rotation, coachBone[i].transform.rotation);
        //        angle = angle / 90f;
        //        if (angle <= 0.33f)
        //        {
        //            bone[i].gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.yellow, angle / 0.33f);
        //        }
        //        else
        //        {
        //            bone[i].gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, (angle - 0.33f) / 0.67f);
        //        }
        //    }
        //}

        //if (jointLine)
        //{
        //    if (firstframe)
        //    {
        //        Array.Copy(userHand.GetComponent<Load_CSV>().joint, 0, joint, 0, joint_num * hand_num);
        //        print("1"+userHand);
        //        Array.Copy(coachHand.GetComponent<Load_CSV>().joint, 0, coachJoint, 0, joint_num * hand_num);
        //    }
        //    for (int i = 0; i < joint_num * hand_num; i++)
        //    {
        //        var start_pt = joint[i].transform.position;
        //        var end_pt = coachJoint[i].transform.position;
        //        line[i] = Lines.transform.Find("Line (" + i.ToString() + ")").gameObject.GetComponent<LineRenderer>();
        //        line[i].SetPosition(0, start_pt);
        //        line[i].SetPosition(1, end_pt);
        //        line[i].startWidth = 0.003f;
        //        line[i].endWidth = 0.003f;

        //        var dist = Vector3.Distance(start_pt, end_pt) / 0.03f;
        //        if (dist <= 0.33f)
        //        {
        //            color = Color.Lerp(Color.green, Color.yellow, dist / 0.33f);
        //        }
        //        else
        //        {
        //            color = Color.Lerp(Color.yellow, Color.red, (dist - 0.33f) / 0.67f);
        //        }


        //        line[i].startColor = color;
        //        line[i].endColor = color;
        //    }
        //}
        //firstframe = false;
    }
}
