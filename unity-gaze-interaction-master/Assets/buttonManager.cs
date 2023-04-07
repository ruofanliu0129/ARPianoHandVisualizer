using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class buttonManager : MonoBehaviour
{
    public GameObject timeLine;
    public GameObject speedController;

    public void WithTimeLine()
    {
        timeLine.SetActive(true);
    }
    public void WithoutTimeLine()
    {
        timeLine.SetActive(false);
    }
    public void WithSpeedController()
    {
        speedController.SetActive(true);
    }
    public void WithoutSpeedController()
    {
        speedController.SetActive(false);
    }

    public void ChangeCompareValue(int i)
    {
        compareCSVwithREALTIME.I.type = i;
    }
}
