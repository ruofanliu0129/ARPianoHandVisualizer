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
        //Mathf.FloorToInt(val) : ������λȡ��
        int startIndex = Mathf.FloorToInt(val);

        Color color = colors[0];

        if (startIndex >= 0)
        {
            if (startIndex + 1 < colors.Length)
            {
                //�ṩ������ɫ
                float factor = (val - startIndex);
                color = Color.Lerp(colors[startIndex], colors[startIndex + 1], factor);
            }
            else if (startIndex < colors.Length)
            {
                //����Ϊ2ʱΪ��ɫ
                color = colors[startIndex];
            }
            //���������鳤��ֱ�ӵ�����ɫ
            else color = colors[colors.Length - 1];
        }
        color.a = slider.fillRect.transform.GetComponent<Image>().color.a;
        slider.fillRect.transform.GetComponent<Image>().color = color;
    }
}
