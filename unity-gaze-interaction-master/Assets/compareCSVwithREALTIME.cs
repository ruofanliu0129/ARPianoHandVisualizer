using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compareCSVwithREALTIME : UnitySingleton<compareCSVwithREALTIME>
{
    private Color32 green32 = Color.green;
    private Color32 red32 = Color.red;
    public GameObject[] joint;
    public GameObject[] bone;
    public GameObject handR;
    public GameObject handL;
    private int bone_num = 20;
    private int joint_num = 21;
    private int hand_num = 2;

    [Header("比较类型")] public int type = 0;


    void Start()
    {
        var hand = handL;
        var bones = hand.transform.Find("Bones").gameObject;
        //assign joints
        joint = new GameObject[joint_num * hand_num];
        for (int i = 0; i < joint_num; i++)
        {
            joint[i] = hand.transform.Find("Joint (" + i.ToString() + ")").gameObject;
        }
        //assign bones
        bone = new GameObject[bone_num * hand_num];
        for (int i = 0; i < bone_num; i++)
        {
            bone[i] = bones.transform.Find("Bone (" + i.ToString() + ")").gameObject;
        }


        hand = handR;
        bones = hand.transform.Find("Bones").gameObject;
        for (int i = 0; i < joint_num; i++)
        {
            joint[i + joint_num] = hand.transform.Find("Joint (" + i.ToString() + ")").gameObject;
        }
        for (int i = 0; i < bone_num; i++)
        {
            bone[i + bone_num] = bones.transform.Find("Bone (" + i.ToString() + ")").gameObject;
        }
        
    }

    void Update()
    {
        var studentJointPos = sendPoints.positions;
        var teacherJointPos = loadCsv_compare_realtime.currentFramePos;
        var studentBoneAng = sendPoints.angles;
        var teacherBoneAng = loadCsv_compare_realtime.currentFramePosBone_vec;

        if (type == 0)
        {
            Com0(studentJointPos, teacherJointPos);
        }
        if (type == 1)
        {
            Com1(studentBoneAng, teacherBoneAng);
        }
    }

    public void ShowErrorPosition(int index, bool hasError, Color color)
    {
        joint[index].SetActive(hasError);

        if (hasError)
        {
            joint[index].GetComponent<Renderer>().material.color = color;
            if (index == 0)
            {
                bone[0].GetComponent<Renderer>().material.color = color;
                bone[4].GetComponent<Renderer>().material.color = color;
                bone[8].GetComponent<Renderer>().material.color = color;
                bone[12].GetComponent<Renderer>().material.color = color;
                bone[16].GetComponent<Renderer>().material.color = color;
            }
            if (index > 1 && index <= 20) bone[index - 1].GetComponent<Renderer>().material.color = color;
            if(index == 21)
            {
                bone[20].GetComponent<Renderer>().material.color = color;
                bone[24].GetComponent<Renderer>().material.color = color;
                bone[28].GetComponent<Renderer>().material.color = color;
                bone[32].GetComponent<Renderer>().material.color = color;
                bone[36].GetComponent<Renderer>().material.color = color;
            }
            if (index > 22 && index <= 41) bone[index - 2].GetComponent<Renderer>().material.color = color;
        }
    }

    public void ShowErrorAngle(int index, bool hasError, Color color)
    {
        if (hasError)
        {
            joint[index].GetComponent<Renderer>().material.color = color;
            if (index <= 39) bone[index].GetComponent<Renderer>().material.color = color;
        }
    }

    private void Com0(Vector3[] studentPos, Vector3[] teacherPos)
    {
        bool isShow = false;

        for (int i = 0; i < 42; i++)
        {
            float distance = Vector3.Distance(studentPos[i], teacherPos[i]);
            distance = distance / (float)0.07;
            Color color = Color32.Lerp(green32, red32, distance);
            ShowErrorPosition(i, true, color);
        }
    }


    private void Com1(Vector3[] studentAng, Vector3[] teacherAng)
    {
        bool isShow = false;

        for (int i = 0; i < (joint_num - 1) * hand_num; i++)
        {
            float angle = Vector3.Angle(studentAng[i], teacherAng[i]);
            angle = angle / (float)90.0;
            Color color = Color32.Lerp(green32, red32, angle);
            ShowErrorAngle(i, true, color);
        }
    }
}
