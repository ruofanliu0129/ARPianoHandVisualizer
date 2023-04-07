using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class sliderControl : UnitySingleton<sliderControl>
{
    [Header("TimeLine")] public Slider timeSlide;
    [Header("SpeedLine")] public Slider speedSlide;


    private int frame_count = 0;
    private float frame_float = 0;
    private int _totalFrame = 0;

    [Header("AllCSVController")] public List<loadCsv_compare_realtime> load_CSVs = new List<loadCsv_compare_realtime>();

    List<Image> imgs = new List<Image>();
    private void Start()
    {
        timeSlide.value = 0;
    }


    void FixedUpdate()
    {
        foreach (var load_CSV in load_CSVs)
        {
            if (load_CSV.autoPlay && (int)timeSlide.value != 1)
            {
                load_CSV.UpdateHands(timeSlide.value);
            }
            else
            {
                timeSlide.value = 0;
            }
            //print("csv_timeslide:" + timeSlide.value);
        }
    }

    public void ChangeTimeValue()
    {
        frame_float = (timeSlide.value * (float)_totalFrame) + (frame_float - frame_count);
        frame_count = (int)Mathf.Round(frame_float);
    }

    public void AddTime(int totalFrame)
    {
        _totalFrame = totalFrame;
        frame_float += (float)speedSlide.value;
        frame_count = (int)Mathf.Round(frame_float);
        timeSlide.value = (float)frame_count / (float)totalFrame;
    }

    public void ChangeCompareValue(int i)
    {
        compareCSVwithREALTIME.I.type = i;
    }

    public void Restart()
    {
        timeSlide.value = 0;
        //speedSlide.value = 1;
    }
}
