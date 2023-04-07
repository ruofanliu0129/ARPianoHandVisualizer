using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JimmysUnityUtilities;
using System;

public class Load_CSV_singleKey : MonoBehaviour
{
    public GameObject handR;
    public GameObject handL;

    public int hand_num = 1;

    public string CSVname;
    public float z_offset;
    public float x_offset;
    public int frame_offset;
    public float speed = 1f;

    public GameObject[] joint;
    public GameObject[] bone;
    public bool autoPlay = false;
    private TextAsset csvFile;
    private List<string[]> csvDatas = new List<string[]>(); // 

    private Dictionary<int, (int, int)> bones_dict;
    
    private int bone_num = 20;
    private int frame_count = 0;
    private float frame_float = 0;
    private int joint_num = 21;
    // Start is called before the first frame update
    void Start()
    {
        frame_count = frame_count + frame_offset;
        var hand = handL;
        var bones = hand.transform.Find("Bones").gameObject;
        loadDict();
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

        //if (hand_num == 2)
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
        if (!autoPlay)
        {
            UpdateHands();
        }

    }
    
   
    // Update is called once per frame
    void FixedUpdate()
    {
        if (autoPlay)
        {
            UpdateHands();
        }
        

        
    }

    void UpdateHands()
    {
        float x = 0f; float y = 0f; float z = 0f;
        //Calculate palmCenter
        //for (int i = 0; i < 3; i++)
        //{
        //    x += float.Parse(csvDatas[frame_count][i * 3 + 2]) + x_offset;
        //    y += float.Parse(csvDatas[frame_count][i * 3 + 3]);
        //    z += float.Parse(csvDatas[frame_count][i * 3 + 4]) + z_offset;
        //}
        //joint[0].transform.position = new Vector3(x / 3, y / 3, -z / 3); //fix reverse z-axis of motive

        for (int i = 0; i < joint_num * hand_num; i++)
        {
            x = float.Parse(csvDatas[frame_count][i * 3 ]) + x_offset;
            y = float.Parse(csvDatas[frame_count][i * 3 + 1]);
            z = float.Parse(csvDatas[frame_count][i * 3 + 2]) + z_offset;
            if (i == 20 || i == 41)
            {
                joint[i].transform.position = new Vector3(x + 0.02f, y, -z); //fix reverse z-axis of motive
            }
            else
            {
                joint[i].transform.position = new Vector3(x, y, -z); //fix reverse z-axis of motive
            }

        }
        //print(csvDatas[frame_count].Length);
        for (int i = 0; i < bone_num * hand_num; i++)
        {
            Vector3 start = joint[bones_dict[i % bone_num].Item1 + i / bone_num * joint_num].transform.position;
            Vector3 end = joint[bones_dict[i % bone_num].Item2 + i / bone_num * joint_num].transform.position;
            Vector3 pos = Vector3.Lerp(start, end, (float)0.5);
            bone[i].transform.position = pos;
            bone[i].transform.up = start - end;
            bone[i].transform.localScale = new Vector3(bone[i].transform.localScale.x, Vector3.Distance(start, end) / 2f, bone[i].transform.localScale.z);
            //print(bone[i].name);
        }
        frame_float += speed;
        frame_count = (int)Math.Round(frame_float);
    }
   

    //Dictionary for bones and joints
    void loadDict()
    {
        bones_dict = new Dictionary<int, (int , int )>()
        {
            { 0, (0, 4) },
            { 1, (0, 8) },
            { 2, (0, 12) },
            { 3, (0, 16) },
            { 4, (0, 20) },
            { 5, (1, 2) },
            { 6, (2, 3) },
            { 7, (3, 4) }, 
            { 8, (5, 6) },
            { 9, (6, 7) },
            { 10, (7, 8) },
            { 11, (9, 10) },
            { 12, (10, 11) },
            { 13, (11, 12) },
            { 14, (13, 14) },
            { 15, (14, 15) },
            { 16, (15, 16) },
            { 17, (17, 18) },
            { 18, (18, 19) },
            { 19, (19, 20) }
        };
    }
}
