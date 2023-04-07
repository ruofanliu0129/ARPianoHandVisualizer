using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorChanger_singleKey : MonoBehaviour
{
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
    private bool firstframe=true;
    private int bone_num = 20;
    private int joint_num = 21;
    private int hand_num;
    private Color color;
    // Start is called before the first frame update
    void Start()
    {
        hand_num = userHand.GetComponent<Load_CSV_singleKey>().hand_num;
        if (boneColor)
        {
            bone = new GameObject[bone_num * hand_num];
            coachBone = new GameObject[bone_num * hand_num];
        }

        if (jointLine)
        {
            Lines.SetActive(true);
            line = new LineRenderer[joint_num * hand_num];
            joint = new GameObject[joint_num * hand_num];
            coachJoint = new GameObject[joint_num * hand_num];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //joint = userHand.GetComponent<Load_CSV>().joint;
        
        if (boneColor)
        {
            if (firstframe)
            {
                Array.Copy(userHand.GetComponent<Load_CSV_singleKey>().bone, 0, bone, 0, bone_num * hand_num);
                Array.Copy(coachHand.GetComponent<Load_CSV_singleKey>().bone, 0, coachBone, 0, bone_num * hand_num);
                
            }
            for (int i = 0; i < bone_num * hand_num; i++)
            {
                float angle = Quaternion.Angle(bone[i].transform.rotation, coachBone[i].transform.rotation);
                angle = angle / 90f;
                if (angle <= 0.33f)
                {
                    bone[i].gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.yellow, angle / 0.33f);
                }
                else
                {
                    bone[i].gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, (angle - 0.33f) / 0.67f);
                }
            }
        }
        
        if (jointLine)
        {
            if (firstframe)
            {
                Array.Copy(userHand.GetComponent<Load_CSV_singleKey>().joint, 0, joint, 0, joint_num * hand_num);
                Array.Copy(coachHand.GetComponent<Load_CSV_singleKey>().joint, 0, coachJoint, 0, joint_num * hand_num);
            }
            for (int i = 0; i < joint_num * hand_num; i++)
            {
                var start_pt = joint[i].transform.position;
                var end_pt = coachJoint[i].transform.position;
                line[i] = Lines.transform.Find("Line (" + i.ToString() + ")").gameObject.GetComponent<LineRenderer>();
                line[i].SetPosition(0, start_pt);
                line[i].SetPosition(1, end_pt);
                line[i].startWidth = 0.003f;
                line[i].endWidth = 0.003f;

                var dist = Vector3.Distance(start_pt, end_pt) / 0.03f;
                if(dist <= 0.33f)
                {
                    color = Color.Lerp(Color.green, Color.yellow, dist / 0.33f);
                }
                else
                {
                    color = Color.Lerp(Color.yellow, Color.red, (dist-0.33f) / 0.67f);
                }
                

                line[i].startColor = color;
                line[i].endColor = color;
            }
        }
        firstframe = false;
    }
}
