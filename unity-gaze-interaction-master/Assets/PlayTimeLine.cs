using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// Timeline
/// </summary>
public class PlayTimeLine : UnitySingleton<PlayTimeLine>
{
    public AudioSource m_MyAudioSource;
    [Header("TimeLine")] public Slider timeSlide;
    [Header("SpeedLine")] public Slider speedSlide;


    private int frame_count = 0;
    private float frame_float = 0;
    private int _totalFrame = 0;
    public float speedValue = 1.0f;

    [Header("AllCSVController")] public List<Load_CSV> load_CSVs = new List<Load_CSV>();
    [Header("MIDIController")] public List<Load_Hackkey> load_Hackkeys = new List<Load_Hackkey>();
    public VideoPlayer vi;

    List<Image> imgs = new List<Image>();
    private void Start()
    {
        timeSlide.value = 0;
        m_MyAudioSource = GetComponent<AudioSource>();
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
            print("csv_timeslide:"+timeSlide.value);
        }
        foreach (var load_Hackkey in load_Hackkeys)
        {
            if (load_Hackkey.autoPlay && (int)timeSlide.value != 1)
            {
                //print("load_Hackkey.UpdateMidis(timeSlide.value)");
                load_Hackkey.UpdateMidis(timeSlide.value);
            }
            else
            {
                timeSlide.value = 0;
            }
            print("hackkey_timeslide:" + timeSlide.value);
        }
    }

    public void ChangeTimeValue()
    {
        frame_float = (timeSlide.value * (float)_totalFrame) + (frame_float - frame_count);
        frame_count = (int)Mathf.Round(frame_float);
    }


    public void ChangeCompareValue_1(int i)
    {
        Compare.I.type = i;
        createLine.I.changeImg(i);
        //SetErrorSprite();
    }

    /// <summary>
    /// Add time after playing per frame
    /// </summary>
    public void AddTime(int totalFrame)
    {
        _totalFrame = totalFrame;
        frame_float += speedValue;
        print("speedValue: "+ speedValue);
        //frame_float += 0.01f;
        frame_count = (int)Mathf.Round(frame_float);

        timeSlide.value = (float)frame_count / (float)totalFrame;

        vi.playbackSpeed = 5.0F * (float)speedSlide.value;
    }

    public void speed25()
    {
        speedValue = 0.01f;
        print("success change speed: " + speedValue);
    }
    public void speed50()
    {
        speedValue = 0.50f;
        print("success change speed: " + speedValue);
    }
    public void speed75()
    {
        speedValue = 0.75f;
        print("success change speed: " + speedValue);
    }
    public void speed100()
    {
        speedValue = 1.00f;
        print("success change speed: " + speedValue);
    }
    

    public void Restart()
    {
        timeSlide.value = 0;
        //speedSlide.value = 1;
    }

    public Image prefab;
    public void SetErrorSprite()
    {
        for (int i = imgs.Count - 1; i > 0; i--)
        {
            GameObject.Destroy(imgs[i].gameObject);
        }
        imgs.Clear();
        var colorInfos = Compare.I.ShowErrorPos();
        GameObject canvas = GameObject.Find("Canvas");
        for (int i = 0; i < _totalFrame; i++)
        {

            var obj = GameObject.Instantiate(prefab.gameObject, canvas.transform);
            obj.transform.position = new Vector3(prefab.transform.position.x + (i - _totalFrame / 2) * (1000 / _totalFrame),
                prefab.transform.position.y, prefab.transform.position.z);
            var img = obj.GetComponent<Image>();
            if (i < imgs.Count) img.color = colorInfos[i];
            imgs.Add(img);
        }
    }
}

