using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;



public class CommandLine : MonoBehaviour
{
    //public void StartProcee();

    public void StartProcess()
    {
        print("start deal with video");
        ProcessStartInfo proc = new ProcessStartInfo("C:/Users/koikePiano/Desktop/PianoHand/ffmpeg.exe");
        //proc.WindowStyle = ProcessWindowStyle.Normal;
        //proc.Arguments = "-i ./Hand_Midi_Align/1.mp4 -vf fps=30 ./Assets/Resources/outputImg/1/%d.png";
        //Process.Start(proc);
        //proc = new ProcessStartInfo("C:/Users/koikePiano/Desktop/PianoHand/ffmpeg.exe");
        proc.WindowStyle = ProcessWindowStyle.Normal;
        proc.Arguments = "-i ./Assets/Resources/Raw_Data/task1/2.mp4 -vf fps=30 ./outputImg/task1/2/%d.png";
        Process.Start(proc);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartProcess();

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
