using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System;
using System.Data;
using System.Text;

public class beginButton : MonoBehaviour
{
    public static bool flag;
    public Button bButton;
    public string userName;
    private string userNameText;
    public string recordTime;
    private string recordTimeText;
    public static string tempath;
    public static List<string[]> strArr = new List<string[]>();

    // Start is called before the first frame update
    void Start()
    {
        flag = false;
        Button btn = bButton.GetComponent<Button>();
        btn.onClick.AddListener(Change);
    }

    void Change()
    {
        flag = !flag;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

	public void WriteCsv()
	{
        //print("frame_c" + sendPoints.frame_c);
        userNameText = userName;
        recordTimeText = recordTime;
        tempath = "WISS/"+ userNameText + "/" + recordTimeText;
        string csvpath = "./Assets/Resources/"+tempath + ".csv";
        print(csvpath);


        FileInfo fi = new FileInfo(csvpath);
        if (!fi.Directory.Exists)
        {
            fi.Directory.Create();
        }

        StreamWriter stream = new StreamWriter(csvpath);
		for (int i = 0; i < sendPoints.strArr.Count; i++)
		{
            if (sendPoints.strArr[i] != null)
            {
                string line="";
                for (int j = 0; j < 42; j++)
                {
                    if (j == 0)
                    {
                        line = sendPoints.strArr[i][j];
                    }
                    else
                    {
                        line = line +","+ sendPoints.strArr[i][j];
                    }
                    
                }
                stream.WriteLine(line);
            }
            //stream.WriteLine($"{sendPoints.strArr[i][0]},{sendPoints.strArr[i][1]}");
		}
        print("final_frame_c" + sendPoints.frame_c);
        stream.Close();
		stream.Dispose();
        print("finish write");
	}
	
}
