using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class sendPoints : MonoBehaviour
{
    public static Vector3[] positions = new Vector3[42];
    public static Vector3[] angles = new Vector3[40];
    public static string[] positions2 = new string[42];

    //[joint0Pos, joint1Pos, ... , joint41Pos,
    //joint0Pos, joint1Pos, ... , joint41Pos,
    //...]
    public static List<string[]> strArr = new List<string[]>();

    private HelloRequester _helloRequester;
    private int i = 0;

    private int bone_num = 21;
    public int hand_num;
    public GameObject left_bone_layer;
    public GameObject right_bone_layer;
    private GameObject[] left_bones;
    private GameObject[] right_bones;

    public GameObject piano;
    public static int frame_c = 0;
    private int r = 0;

    void Awake()
    {
        left_bones = new GameObject[bone_num];
        for (int i = 0; i < 21; i++)
        {
            left_bones[i] = left_bone_layer.transform.Find("Bone (" + i.ToString() + ")").gameObject;
        }

        right_bones = new GameObject[bone_num];
        for (int i = 0; i < 21; i++)
        {
            right_bones[i] = right_bone_layer.transform.Find("Bone (" + i.ToString() + ")").gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _helloRequester = new HelloRequester();
        _helloRequester.Start();
    }

    public void writeArr()
    {
        r = 1;
    }
    public void stopwriteArr()
    {
        r = 0;
    }

    // Update is called once per frame
    void Update()
    {   
        i = 0;
        frame_c++;
        //print("fr_c" + frame_c);
        //calculate positions[] (joint positions vector array)
        // transform.GetChild(0) = Attachment Hand (left)
        foreach (Transform g in transform.GetChild(0).GetComponentsInChildren<Transform>())
        {
            if (g.name.Contains("Attachment") || g.name.Contains("Capsule") )
            {
                g.gameObject.SetActive(false);
                continue;
            }
            positions2[i] = g.position.ToString("F6").Replace(")", "").Replace("(", ""); 

            //get joint i position
            positions[i]=g.position;
            //Debug.Log("Left" +g.position+ g.name + i);
            //set corresponding bones by g.position
            int end_index;
            if (i == 2 || i==5 || i==9 || i==13 || i==17)
            {
                end_index = 0;
            }
            else if (i == 0)//wrist joint
            {
                end_index = 0;
            }
            else if (i == 1)//palm joint
            {
                end_index = 1;
            }
            else
            {
                end_index = i - 1;
            }
            Vector3 start = g.position;
            Vector3 end = positions[end_index];
            Vector3 pos = Vector3.Lerp(start, end, (float)0.5);
            if (i < 21)
            {
                left_bones[i].transform.position = pos;
                left_bones[i].transform.up = start - end;
                left_bones[i].transform.localScale = new Vector3(left_bones[i].transform.localScale.x, Vector3.Distance(start, end) / 2f, left_bones[i].transform.localScale.z);
            }
            i++;
        }
        // transform.GetChild(1) = Attachment Hand (right)
        foreach (Transform g in transform.GetChild(1).GetComponentsInChildren<Transform>())
        {
            if (g.name.Contains("Attachment") || g.name.Contains("Capsule") )
            {
                g.gameObject.SetActive(false);
                continue;
            }
            positions2[i] = g.position.ToString("F6").Replace(")", "").Replace("(", "");
            positions[i] = g.position;
            //Debug.Log("Right"+g.position+g.name+i);
            int end_index;
            if (i == 23 || i == 26 || i == 30 || i == 34 || i == 38)
            {
                end_index = 21;
            }
            else if (i == 21) //wrist joint
            {
                end_index = 21;
            }
            else if (i == 22) //palm joint
            {
                end_index = 22;
            }
            else
            {
                end_index = i - 1;
            }
            Vector3 start = g.position;
            Vector3 end = positions[end_index];
            Vector3 pos = Vector3.Lerp(start, end, (float)0.5);
            int new_i = i - 21;
            if (new_i < 21)
            {
                right_bones[new_i].transform.position = pos;
                right_bones[new_i].transform.up = start - end;
                right_bones[new_i].transform.localScale = new Vector3(right_bones[new_i].transform.localScale.x, Vector3.Distance(start, end) / 2f, right_bones[new_i].transform.localScale.z);
            }

            i++;
        }

        if (r == 1)
        {
            string[] x = new string[positions2.Length];
            for (int z = 0; z < positions2.Length; z++)
            {
                x[z] = positions2[z];
            }
            strArr.Add(x);
            print("arr"+strArr.Count);
        }


        //calculate angles[] (bone angles vector list)
        for (int j = 0; j < 40; j++)
        {
            int s;
            int e;
            if (j < 20)
            {
                if (j % 4 == 0)
                {
                    s = 0;
                    if (j == 0)
                    {
                        e = 2;
                    }
                    else
                    {
                        e = j + 1;
                    }                   
                }
                else
                {
                    if(j==1)
                    {
                        s = 0;e = 2;
                    }
                    else
                    {
                        s = j;e = j + 1;
                    }                   
                }                
            }
            else
            {
                if (j % 4 == 0)
                {
                    s = 21;
                    if (j == 20)
                    {
                        e = 23;
                    }
                    else
                    {
                        e = j + 2;
                    }
                }
                else
                {
                    if (j == 21)
                    {
                        s = 21; e = 23;
                    }
                    else
                    {
                        s = j+1; e = j + 2;
                    }
                }
            }
            Vector3 start_v = positions[s];
            Vector3 end_v = positions[e];
            Vector3 pos_v = end_v - start_v;
            pos_v = pos_v.normalized;
            angles[j] = pos_v;
        }
    }

    private void OnDestroy()
    {
        _helloRequester.Stop();
    }


    //calibrate the piano position based on right index tip's pos
    public void calibrate()
    {
        foreach (Transform g in transform.GetChild(1).GetComponentsInChildren<Transform>())
        {
            if (g.name.Contains("IndexTip"))
            {
                //print("Set piano position to" + g.position);
                piano.transform.position = g.position;
            }
        }
        
    }
}
