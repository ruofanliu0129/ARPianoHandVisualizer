using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularButton : MonoBehaviour
{
    public GameObject timeLine;
    public GameObject colorPartition;
    public GameObject speedController;
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void WithTimeLine()
    {
        timeLine.SetActive(true);
    }
    public void WithoutTimeLine()
    {
        timeLine.SetActive(false);
    }
    public void WithColorPartition()
    {
        colorPartition.SetActive(true);
    }
    public void WithoutColorPartition()
    {
        colorPartition.SetActive(false);
    }
    public void WithSpeedController()
    {
        speedController.SetActive(true);
    }
    public void WithoutSpeedController()
    {
        speedController.SetActive(false);
    }
}
