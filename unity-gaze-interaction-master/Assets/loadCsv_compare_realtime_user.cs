using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JimmysUnityUtilities;
using System;

public class loadCsv_compare_realtime_user : MonoBehaviour
{

    public enum DataType
    {
        coach,
        player,
    }

    private Dictionary<int, (int, int)> bones_dict = new Dictionary<int, (int, int)>
    {
            { 0, (0, 2) },
            { 1, (0, 2) },
            { 2, (2, 3) },
            { 3, (3, 4) },
            { 4, (0, 5) },
            { 5, (5, 6) },
            { 6, (6, 7) },
            { 7, (7, 8) },
            { 8, (0, 9) },
            { 9, (9, 10) },
            { 10, (10, 11) },
            { 11, (11, 12) },
            { 12, (0, 13) },
            { 13, (13, 14) },
            { 14, (14, 15) },
            { 15, (15, 16) },
            { 16, (0, 17) },
            { 17, (17, 18) },
            { 18, (18, 19) },
            { 19, (19, 20) }
    };

    //[Header("玩家类型")] public DataType dataType;


    public GameObject handR;
    public GameObject handL;

    public int hand_num;

    private string CSVname="Processed_Data/hand1_pro";
    public float z_offset;
    public float x_offset;
    public int frame_offset;
    public float speed = 1f;

    public GameObject[] joint;
    public GameObject[] bone;
    public bool autoPlay = false;
    private TextAsset csvFile;
    private List<string[]> csvDatas = new List<string[]>(); // 

    private int bone_num = 20;
    private float frame_float = 0;
    public int joint_num = 21;

    public static Vector3[] currentFramePos;
    public static Quaternion[] currentFramePosBone;
    public static Vector3[] currentFramePosBone_vec;

    public List<Vector3[]> VecList = new List<Vector3[]>();

    public void loadUserData()
    {
        //GameObject.Find("Hand_L").SetActive(false);
        //GameObject.Find("Hand_R").SetActive(false);
        CSVname = "WISS/lrf/1";
        Start();
    }

    void Start()
    {
        var hand = handL;
        var bones = hand.transform.Find("Bones").gameObject;

        currentFramePos = new Vector3[joint_num * hand_num];
        currentFramePosBone = new Quaternion[bone_num * hand_num];
        currentFramePosBone_vec = new Vector3[bone_num * hand_num];

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
        if (hand_num == 2)
        {
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



        csvFile = Resources.Load(CSVname) as TextAsset; // Read CSV 
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() != -1) //
        {
            string line = reader.ReadLine(); // read by line
            csvDatas.Add(line.Split(',')); // 
        }

        int frameSum = (int)(csvDatas.Count);
        float x = 0f; float y = 0f; float z = 0f;
        for (int frameLoc = 0; frameLoc < frameSum; frameLoc++)
        {
            Vector3[] c = new Vector3[joint_num * hand_num];
            //获取第frameLoc帧的joint position
            for (int i = 0; i < joint_num * hand_num; i++)
            {

                x = float.Parse(csvDatas[frameLoc][i * 3]);
                y = float.Parse(csvDatas[frameLoc][i * 3 + 1]);
                z = float.Parse(csvDatas[frameLoc][i * 3 + 2]);
                c[i] = new Vector3(x, y, z);
            }
            VecList.Add(c);

        }

    }

    //void Start()
    //{
    //    int frameSum = csvDatas.Count;
    //}
    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (autoPlay)
    //    {
    //        UpdateHands();
    //    }
    //}

    public void UpdateHands(float rate)
    {
        int frame = (int)(rate * csvDatas.Count);
        if (frame == csvDatas.Count)
        {
            //print("frame" + frame);
        }
        else
        {
            float x = 0f; float y = 0f; float z = 0f;
            for (int i = 0; i < joint_num * hand_num; i++)
            {
                x = float.Parse(csvDatas[frame][i * 3]);
                y = float.Parse(csvDatas[frame][i * 3 + 1]);
                z = float.Parse(csvDatas[frame][i * 3 + 2]);
                joint[i].transform.position = new Vector3(x, y, z);
                currentFramePos[i] = joint[i].transform.position;
            }


            for (int i = 0; i < bone_num * hand_num; i++)
            {
                Vector3 start = joint[bones_dict[i % bone_num].Item1 + i / bone_num * joint_num].transform.position;
                Vector3 end = joint[bones_dict[i % bone_num].Item2 + i / bone_num * joint_num].transform.position;
                Vector3 pos = Vector3.Lerp(start, end, (float)0.5);
                bone[i].transform.position = pos;
                bone[i].transform.up = start - end;
                bone[i].transform.localScale = new Vector3(bone[i].transform.localScale.x, Vector3.Distance(start, end) / 2f, bone[i].transform.localScale.z);
                currentFramePosBone[i] = bone[i].transform.rotation;


                int s = bones_dict[i % bone_num].Item1 + i / bone_num * joint_num;
                int e = bones_dict[i % bone_num].Item2 + i / bone_num * joint_num;
                Vector3 start_v = VecList[frame][s];
                Vector3 end_v = VecList[frame][e];
                Vector3 pos_v = end_v - start_v;
                pos_v = pos_v.normalized;
                currentFramePosBone_vec[i] = pos_v;
            }

            sliderControl.I.AddTime(csvDatas.Count);
        }

    }

}


