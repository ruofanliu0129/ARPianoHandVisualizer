using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SliderColor : MonoBehaviour
{
    // Start is called before the first frame update
    public Color[] colors = new Color[] { Color.red, Color.yellow, Color.green };
    Slider slider;
    public Load_CSV csv;
    void Start()
    {
        int frameSum = csv.csvDatas.Count;
        //print(frameSum);
        slider = GetComponent<Slider>();
        slider.fillRect.transform.GetComponent<Image>().color = Color.green;
    }
    void Update()
    {
        float val = slider.value;
        print(val);
        val *= (colors.Length - 1);
        //Mathf.FloorToInt(val) : 向下舍位取整
        int startIndex = Mathf.FloorToInt(val);

        Color color = colors[0];

        if (startIndex >= 0)
        {
            if (startIndex + 1 < colors.Length)
            {
                //提供过渡颜色
                float factor = (val - startIndex);
                color = Color.Lerp(colors[startIndex], colors[startIndex + 1], factor);
            }
            else if (startIndex < colors.Length)
            {
                //索引为2时为绿色
                color = colors[startIndex];
            }
            //当大于数组长度直接等于绿色
            else color = colors[colors.Length - 1];
        }
        color.a = slider.fillRect.transform.GetComponent<Image>().color.a;
        slider.fillRect.transform.GetComponent<Image>().color = color;
    }
}
