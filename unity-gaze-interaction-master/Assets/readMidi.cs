using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JimmysUnityUtilities;
using System;
using UnityEngine.UI;
public class readMidi : MonoBehaviour
{
    public string CSVname;
    private TextAsset csvFile;
    public List<string[]> MidiData = new List<string[]>();
    public int frame_num = 0;
    [Header("ObtainFrameSum")] public Load_CSV reMidi_load_csv = new Load_CSV();

    void Start()
    {
        csvFile = Resources.Load(CSVname) as TextAsset; // Read CSV 
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() != -1) //
        {
            string line = reader.ReadLine(); // read by line
            MidiData.Add(line.Split(',')); // 
        }

        frame_num = reMidi_load_csv.csvCount;
        print("frame_num:"+ frame_num);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
