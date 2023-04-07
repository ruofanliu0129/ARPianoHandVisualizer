//-----------------------------------------Tokyo Tech Midi-------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JimmysUnityUtilities;
using System;
using UnityEngine.UI;


public class Load_Hackkey : MonoBehaviour
{
    public string CSVname;
    private TextAsset csvFile;
    public List<string[]> MidiData_t = new List<string[]>();

    public Transform[] PianoKeys;
    public bool autoPlay = true;
    private int[,] Key_Processed;
    float key_y;
    //private List<string[]> MidiData_t = new List<string[]>();
    private int frame_number;
    //[Header("ObtainFrameSum")] public readMidi readMidis = new readMidi();

    //public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        csvFile = Resources.Load(CSVname) as TextAsset; // Read CSV 
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() != -1) //
        {
            string line = reader.ReadLine(); // read by line
            MidiData_t.Add(line.Split(',')); // 
        }

        PianoKeys = GetComponentsInChildren<Transform>();

        //frame_number = readMidis.frame_num;
        //MidiData_t = MidiData;

        Key_Processed = ProcessMidi();

        key_y = PianoKeys[1].transform.position.y;

    }


    /// <param name="rate">播放百分之多少</param>
    public void UpdateMidis(float rate)
    {
        int frame = (int)(rate * frame_number);
        if (frame == frame_number)
        {
            //print("frame" + frame);
        }
        else
        {
            //print("frame_num" + frame_number);
            for (int i = 0; i < 88; i++)
            {
                //print("fram"+frame+"i"+i);
                if (Key_Processed[frame, i] != 0)
                {
                    Vector3 PianoKeyPos = PianoKeys[i + 1].transform.position;
                    PianoKeys[i + 1].position = new Vector3(PianoKeyPos.x, key_y - 0.01f, PianoKeyPos.z);
                }
                else
                {
                    Vector3 PianoKeyPos = PianoKeys[i + 1].transform.position;
                    PianoKeys[i + 1].position = new Vector3(PianoKeyPos.x, key_y, PianoKeyPos.z);
                }


            }
        }
    }

    int[,] ProcessMidi()
    {
        //MidiData.Count output frames sum (hangshu)
        //MidiData[x].Length output lieshu
        int[,] a = new int[MidiData_t.Count, MidiData_t[0].Length];
        print("count" + MidiData_t.Count);
        print("length" + MidiData_t[0].Length);
        int iMidi = 0; //the number of pressed key
        for (int i = 0; i < MidiData_t.Count; i++)
        {
            if ((int)float.Parse(MidiData_t[i][1]) == 63)
            {
                continue;
            }
            for (int j = 0; j < MidiData_t[i].Length; j++)
            {
                a[iMidi, j] = (int)float.Parse(MidiData_t[i][j]);

            }
            iMidi = iMidi + 1;
        }
        print("imidi"+iMidi);

        //initialize every key at every frame to 0
        int[,] keyAtFrame = new int[frame_number, 88];
        int whichKey, whenFrame;
        for (int m = 0; m < iMidi - 1; m++)
        {
            whichKey = a[m, 0] - 21;
            whenFrame = (int)((float)(a[m, 2] - a[0, 2]) * 0.03f);
            for (int x = 0; x < 10; x++)
            {
                print("whenframe" + whenFrame);
                keyAtFrame[whenFrame + x, whichKey] = 1;
            }
        }
        whichKey = a[iMidi - 1, 0] - 21;
        whenFrame = (int)((float)(a[iMidi - 1, 2] - a[0, 2]) * 0.03f);
        for (int x = 0; x < 10; x++)
        {
            keyAtFrame[whenFrame - x, whichKey] = 1;
        }

        return keyAtFrame;

    }
}


//-----------------------------------------Sony Keystroke-----------------------------
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;
//using JimmysUnityUtilities;
//using System;
//using UnityEngine.UI;

//public class Load_Hackkey : MonoBehaviour
//{
//    public Transform[] PianoKeys;

//    public string CSVname;
//    public int frame_offset;
//    public bool autoPlay = true;
//    private TextAsset csvFile;
//    private List<string[]> MidiData = new List<string[]>();
//    private int frame_num = 0;
//    private float frame_float = 0;
//    //public float speed = 1f;

//    // Start is called before the first frame update
//    void Awake()
//    {
//        PianoKeys = GetComponentsInChildren<Transform>();
//        foreach (Transform child in PianoKeys)
//        {
//            //print(child.name);
//        }


//        //frame_count = frame_count + frame_offset;
//        csvFile = Resources.Load(CSVname) as TextAsset; // Read CSV 
//        StringReader reader = new StringReader(csvFile.text);

//        while (reader.Peek() != -1) //
//        {
//            string line = reader.ReadLine(); // read by line
//            MidiData.Add(line.Split(',')); // 
//        }

//        //int[,] Midi_Processed = ProcessMidi(2);
//        frame_num = MidiData.Count;

//    }


//    /// <param name="rate">播放百分之多少</param>
//    public void UpdateMidis(float rate)
//    {
//        int frame = (int)(rate * frame_num);
//        print("frame" + frame);
//        for (int i = 0; i < 88; i++)
//        {
//            float keystroke = float.Parse(MidiData[frame][i]);
//            float d = 0.015f - (keystroke + 1) / 500f;

//            //key[i].color= new Color(c, 0, 0);
//            Vector3 PianoKeyPos = PianoKeys[i + 1].transform.position;

//            PianoKeys[i + 1].position = new Vector3(PianoKeyPos.x, d, PianoKeyPos.z);


//        }

//    }
//}