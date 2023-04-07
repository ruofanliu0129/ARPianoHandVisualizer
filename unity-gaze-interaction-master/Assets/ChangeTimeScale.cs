using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTimeScale : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;

    //public GameObject handVisualizer;
    //public GameObject coachHandVisualizer;
    //private Load_CSV userCSV;
    //private Load_CSV coachCSV;
    void Start()
    {
        //userCSV = handVisualizer.GetComponent<Load_CSV>();
        //coachCSV = coachHandVisualizer.GetComponent<Load_CSV>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = slider.value;
        //print(slider.value);
    }
}

