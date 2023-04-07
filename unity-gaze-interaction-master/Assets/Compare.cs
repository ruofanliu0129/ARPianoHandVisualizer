using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compare : UnitySingleton<Compare>
{
    public Load_CSV data_coach;
    public Load_CSV data_player;

    public int hand_num;
    public int joint_num;

    public List<ErrorInfo> errorInfos = new List<ErrorInfo>();
    public Camera came1;
    public Camera came2;

    private Color32 green32 = Color.green;
    private Color32 red32 = Color.red;

    [Header("比较类型")] public int type = 0;

    void FixedUpdate()
    {
        if (type == 0)
        {
            Com0();
        }
        if (type == 1)
        {
            Com1();
        }
        if (type == 2)
        {
            Com2();
        }
    }


    private void Com0()
    {
        bool isShow = false;

        for (int i = 0; i < joint_num * hand_num; i++)
        {
            float distance = Vector3.Distance(data_coach.currentFramePos[i], data_player.currentFramePos[i]);
            distance=distance/ errorInfos[0].maxError;
            //print("d"+distance);
            Color color = Color32.Lerp(green32, red32, distance);
            data_coach.ShowErrorPosition(i, true, color);
            //if (distance > errorInfos[type].minError)
            //{

            //    //Color color = CompareError(type, distance);
            //    Color color = Color32.Lerp(green32, red32, distance);
            //    data_coach.ShowErrorPosition(i, true, color);
            //    isShow = true;
            //}
            //else
            //{
            //    data_coach.ShowErrorPosition(i, false, Color.green);
            //}
        }
        //came1.enabled=true;
        //came1.rect = new Rect(new Vector2(0,0), new Vector2(200,200));
        //came2.SetActive(!isShow);
        //came2.enabled = true;
        //ShowDifferent.I.newPiano.SetActive(isShow);
    }

    //Angle between the corresponding bones
    private void Com1()
    {
        bool isShow = false;

        for (int i = 0; i < (joint_num - 1) * hand_num; i++)
        {
            //float angle = 0;
            float angle = Vector3.Angle(data_coach.currentFramePosBone_vec[i], data_player.currentFramePosBone_vec[i]);
            //angle = Quaternion.Dot(data_coach.currentFramePosBone[i], data_player.currentFramePosBone[i]);
            //angle = Quaternion.Angle(data_coach.currentFramePosBone[i], data_player.currentFramePosBone[i]) * (angle > 0 ? 1 : -1);
            //angle = Mathf.Abs(angle);
            angle = angle / errorInfos[1].maxError;

            Color color = Color32.Lerp(green32, red32, angle);
            data_coach.ShowErrorAngle(i, true, color);
        }
        //came1.SetActive(isShow);
        //came2.SetActive(!isShow);
        //ShowDifferent.I.newPiano.SetActive(isShow);

    }

    //Angle between adjacent bones
    private void Com2()
    {
        bool isShow = false;
        float ErrorAngle = 0;
        float PlayerAngle = 0;
        float CoachAngle = 0;
        for (int i = 0; i < (joint_num - 1) * hand_num; i++)
        {
            if (i == 3 || i == 3 + joint_num - 1)
            {
                PlayerAngle = Vector3.Angle(data_player.currentFramePosBone_vec[i], data_player.currentFramePosBone_vec[i + 16]);
                CoachAngle = Vector3.Angle(data_coach.currentFramePosBone_vec[i], data_coach.currentFramePosBone_vec[i + 16]);
                ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
            }
            else if (i == 7 || i == 7 + joint_num - 1)
            {
                PlayerAngle = Vector3.Angle(data_player.currentFramePosBone_vec[i], data_player.currentFramePosBone_vec[i -4]);
                CoachAngle = Vector3.Angle(data_coach.currentFramePosBone_vec[i], data_coach.currentFramePosBone_vec[i -4]);
                ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
            }
            else if (i == 15 || i == 15 + joint_num - 1)
            {
                PlayerAngle = Vector3.Angle(data_player.currentFramePosBone_vec[i], data_player.currentFramePosBone_vec[i -8]);
                CoachAngle = Vector3.Angle(data_coach.currentFramePosBone_vec[i], data_coach.currentFramePosBone_vec[i -8]);
                ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
            }
            else if (i == 11 || i == 11 + joint_num - 1)
            {
                PlayerAngle = Vector3.Angle(data_player.currentFramePosBone_vec[i], data_player.currentFramePosBone_vec[i + 4]);
                CoachAngle = Vector3.Angle(data_coach.currentFramePosBone_vec[i], data_coach.currentFramePosBone_vec[i + 4]);
                ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
            }
            else if (i == 19 || i == 19 + joint_num - 1) 
            {  

            }
            else
            {
                PlayerAngle = Vector3.Angle(data_player.currentFramePosBone_vec[i], data_player.currentFramePosBone_vec[i + 1]);
                CoachAngle = Vector3.Angle(data_coach.currentFramePosBone_vec[i], data_coach.currentFramePosBone_vec[i + 1]);
                ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
            }

            ErrorAngle = ErrorAngle / errorInfos[2].maxError;

            Color color = Color32.Lerp(green32, red32, ErrorAngle);
            data_coach.ShowErrorAngle(i, true, color);
            data_coach.ShowErrorAdjacentAngle(i, true, color);
            isShow = true;

        }


    }



    public Color CompareError(int comType, float error)
    {
        if (error > errorInfos[comType].redError) return new Color(1, 0, 0, 1);
        else if (error > errorInfos[comType].orangeError) return new Color(1, 0.5f, 0, 1);
        else if (error > errorInfos[comType].yellowError) return new Color(1, 1, 0, 1);
        //else if (error > errorInfos[comType].yellowgreenError) return new Color(0.78f, 1, 0, 1);
        else return new Color(0.18f,0.72f,0.13f,1);
    }

    public List<Color> ShowErrorPos()
    {
        List<Color> colorByFrame = new List<Color>();
        for (int i = 0; i < data_coach.csvDatas.Count; i++)
        {
            //后面要输入当前帧的误差大小，先随机一个
            colorByFrame.Add(CompareError(type, Random.Range(0f, 1f)));
        }

        return colorByFrame;
    }
}

[System.Serializable]
public struct ErrorInfo
{
    public float minError;
    //public float yellowgreenError;
    public float yellowError;
    public float orangeError;
    public float redError;
    public float maxError;
}
