using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JimmysUnityUtilities;
using System;

public class LinearAlign : MonoBehaviour
{
    private int csvCount;
    private TextAsset csvMidi1;
    private TextAsset csvMidi2;
    private TextAsset csvHand1;
    private TextAsset csvHand2;

    public List<string[]> MidiData1 = new List<string[]>();
    public List<string[]> MidiData2 = new List<string[]>();
    public List<string[]> HandData1 = new List<string[]>();
    public List<string[]> HandData2 = new List<string[]>();

    public string Midi1;
    public string Midi2;
    public string Hand1;
    public string Hand2;

    private string Head_path = "Raw_Data/task1/";
    private string alignCsv_path = "u6-t1";
    private string method_number_path = "-B-1/";

    public void StartDealCSV()
    {
        Start();
    }

    public void CopyFolder(string sourcePath, string destPath,int fromFrame,int toFrame)
    {
        if (Directory.Exists(sourcePath))
        {
            if (!Directory.Exists(destPath))
            {
                try
                {
                    Directory.CreateDirectory(destPath);
                }
                catch (Exception ex)
                {
                    throw new Exception("创建目标目录失败：" + ex.Message);
                }
            }


            string sourceFile= Path.Combine(new string[] { sourcePath, fromFrame.ToString()+".png" });
            string destFile = Path.Combine(new string[] { destPath, toFrame.ToString()+".png" });
            File.Copy(sourceFile, destFile, true);


        }
    }

    void Start()
    {
        string _Midi1 = "Raw_Data/task1/midi1";
        string _Midi2 = Head_path + alignCsv_path+ "/" + alignCsv_path + method_number_path+"midi2";
        string _Hand1 = "Raw_Data/task1/hand1";
        string _Hand2 = Head_path + alignCsv_path + "/" + alignCsv_path + method_number_path + "hand2";

        CopyFolder("./outputImg/1/", "./outputImg/2/",1,1);
        //Midi data
        //key, press or lift, timestamp
        csvMidi1 = Resources.Load(_Midi1) as TextAsset; // Read Midi1
        StringReader reader = new StringReader(csvMidi1.text);
        while (reader.Peek() != -1) 
        {
            string line = reader.ReadLine(); // read by line
            MidiData1.Add(line.Split(',')); 
        }


        csvMidi2 = Resources.Load(_Midi2) as TextAsset; // Read Midi2
        reader = new StringReader(csvMidi2.text);
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine(); // read by line
            MidiData2.Add(line.Split(','));
        }

        csvHand1 = Resources.Load(_Hand1) as TextAsset; // Read Hand1
        reader = new StringReader(csvHand1.text);
        while (reader.Peek() != -1) 
        {
            string line = reader.ReadLine(); // read by line
            HandData1.Add(line.Split(','));  
        }

        csvHand2 = Resources.Load(_Hand2) as TextAsset; // Read Hand2
        reader = new StringReader(csvHand2.text);
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine(); // read by line
            HandData2.Add(line.Split(','));
        }

        int[,] MidiData1_Processed = ProcessMidi(1);
        int[,] MidiData2_Processed = ProcessMidi(2);
        double[] TimeStamp1 = new double[MidiData1_Processed.GetLength(0)];
        double[] TimeStamp2 = new double[MidiData1_Processed.GetLength(0)];

        int c = 0;
        
        for (int i=0;i< MidiData1_Processed.GetLength(0); i++)
        {
            if (MidiData1_Processed[i, 1] != 0)
            {
                for (int j = 0; j < MidiData2_Processed.GetLength(0); j++)
                {
                    if (MidiData2_Processed[j, 1] != 0)
                    {
                        if (MidiData1_Processed[i, 0] == MidiData2_Processed[j, 0])
                        {
                            TimeStamp1[c] = MidiData1_Processed[i, 2];
                            TimeStamp2[c] = MidiData2_Processed[j, 2];
                            MidiData2_Processed[j, 1] = 0;
                            c = c + 1;
                            break;
                        }
                    }
                }
            }
            else { continue; }
            
        }

        double[] TimeStampFrame1 = new double[c];
        double[] TimeStampFrame2 = new double[c];
        //int[] TimeStamp stores time corresponds to reference MIDI
        //TimeStamp1    TimeStamp2     Key
        //1000          2000           52
        //1010          2030           60
        //print("time1"+c);
        for(int x = 0; x < c; x++)
        {
            TimeStampFrame1[x] = TimeStamp1[x] / 1000.0 * 30.0;

            print(TimeStampFrame1[x]);
        }

        for (int x = 0; x < c; x++)
        {
            TimeStampFrame2[x] = TimeStamp2[x] / 1000.0 * 30.0;

        }

        string hand2_align_file_name = "./Assets/Resources/Processed_Data/task1/hand1_align.csv";
        ExportAlignCSV(hand2_align_file_name, TimeStampFrame1, TimeStampFrame2);
            
        ExportOnlyHandCSV("./Assets/Resources/Raw_Data/task1/"+ alignCsv_path+"/"+alignCsv_path+ method_number_path+"hand1_pro.csv", TimeStampFrame1);
        ExportAlignOnlyHandCSV("./Assets/Resources/Raw_Data/task1/" + alignCsv_path + "/" + alignCsv_path + method_number_path + "hand2_pro.csv", hand2_align_file_name, TimeStampFrame1); 
    }

    int[,] ProcessMidi(int Mn)
    {
        if (Mn == 1)
        {

            int[,] a = new int[MidiData1.Count, MidiData1[0].Length];
            for (int i = 0; i < MidiData1.Count; i++)
            {
                if ((int)float.Parse(MidiData1[i][1]) == 0)
                {
                    continue;
                }
                for (int j = 0; j < MidiData1[i].Length; j++)
                {
                    a[i, j] = (int)float.Parse(MidiData1[i][j]);                
                }
            }
            return a;
        }
        else
        {

            int[,] a = new int[MidiData2.Count, MidiData2[0].Length];
            for (int i = 0; i < MidiData2.Count; i++)
            {
                if ((int)float.Parse(MidiData2[i][1]) == 0)
                {
                    continue;
                }
                for (int j = 0; j < MidiData2[i].Length; j++)
                {
                    a[i, j] = (int)float.Parse(MidiData2[i][j]);

                }
            }
            return a;
        }
    }

    //delete (reference hand.csv) before begin and after end based on refer midi
    //and write in new csv
    public void ExportOnlyHandCSV(string fileName, double[] timeStamp1)
    {
        if (fileName.Length > 0)
        {
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

            int Hand1Begin = (int)timeStamp1[0];
            int Hand1End = (int)timeStamp1[timeStamp1.Length-1];
            int count = 1;

            for (int i = Hand1Begin; i <= Hand1End; i++)
            {
                CopyFolder("./outputImg/task1/1/", "./Assets/Resources/outputImg/task1/1/", i, count);
                count = count + 1;
                string dataStr = string.Empty;

                for (int j = 0; j < 126; j++)
                {
                    if (j < 125)
                    {
                        dataStr += HandData1[i][j] + ",";
                    }
                    else
                    {
                        dataStr += HandData1[i][j];
                    }
                }

                sw.WriteLine(dataStr);
            }

            sw.Close();
            fs.Close();
        }
    }


    public void ExportAlignOnlyHandCSV(string outputFileName, string inputFileName, double[] timeStamp1)
    {

        List<string[]> HandData2Align = new List<string[]>();

        var fileData  = System.IO.File.ReadAllText(inputFileName);
        var lines = fileData.Split(new string[] {"\n","\r\n"}, StringSplitOptions.None);
        foreach (var line in lines)
        {
            HandData2Align.Add(line.Split(','));
        }


        if (outputFileName.Length > 0)
        {
            FileStream fs = new FileStream(outputFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

            int Hand1Begin = (int)timeStamp1[0];
            int Hand1End = (int)timeStamp1[timeStamp1.Length - 1];

            int count = 1;
            for (int i = Hand1Begin; i <= Hand1End; i++)
            {
                CopyFolder("./outputImg/task1/2raw/", "./Assets/Resources/outputImg/task1/2/", i, count);
                count = count + 1;

                string dataStr = string.Empty;

                for (int j = 0; j < 126; j++)
                {
                    if (j < 125)
                    {
                        dataStr += HandData2Align[i][j] + ",";
                    }
                    else
                    {
                        dataStr += HandData2Align[i][j];
                    }
                }

                sw.WriteLine(dataStr);
            }
            
            sw.Close();
            fs.Close();
        }
    }

    //write CSV
    public void ExportAlignCSV(string fileName, double[] timeStamp1, double[] timeStamp2)
    {
        if (fileName.Length > 0)
        {
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

            int timeRow = 0;
            for (int i = 0; i < HandData1.Count; i++)
            {

                string dataStr = string.Empty;
                
                if (timeRow < timeStamp1.Length && i > (int)timeStamp1[timeRow])
                {
                    timeRow = timeRow + 1;
                }

                int Hand2Row;
                if (timeRow == 0)
                {
                    Hand2Row = (int)(i * timeStamp2[0] / timeStamp1[0]);
                }
                else if (timeRow == timeStamp1.Length)
                {
                    Hand2Row = (int)(((i - timeStamp1[timeRow - 1]) * (HandData2.Count - timeStamp2[timeRow - 1]) / (HandData1.Count - timeStamp1[timeRow - 1])) + timeStamp2[timeRow - 1]);
                }
                else
                {
                    Hand2Row = (int)(((i - timeStamp1[timeRow - 1]) * (timeStamp2[timeRow] - timeStamp2[timeRow-1])/(timeStamp1[timeRow] - timeStamp1[timeRow - 1])) + timeStamp2[timeRow-1]);
                }

                for (int j = 0; j < 126; j++)
                {
                    if (j < 125)
                    {
                        dataStr += HandData2[Hand2Row][j] + ",";
                    }
                    else
                    {
                        dataStr += HandData2[Hand2Row][j];
                    }
                }

                CopyFolder("./outputImg/task1/2/", "./outputImg/task1/2raw/", Hand2Row+1, i+1);
                print(i + 1);

                sw.WriteLine(dataStr);
            }
            sw.Close();
            fs.Close();
        }
    }

    void Update()
    {
        
    }
}
