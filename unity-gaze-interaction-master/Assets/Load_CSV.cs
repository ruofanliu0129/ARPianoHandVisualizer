using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JimmysUnityUtilities;
using System;
using UnityEngine.UI;

public class Load_CSV : MonoBehaviour
{
    private Dictionary<int, (int, int)> bones_dict = new Dictionary<int, (int, int)>
    {
            { 0, (0, 2) },
            { 1, (0, 2) },
            { 2, (2, 3) },
            { 3, (3, 4) },
            { 4, (0, 5) },
            { 5, (5, 6) },
            { 6, (6, 7) },
            { 7, (7, 8) },
            { 8, (0, 9) },
            { 9, (9, 10) },
            { 10, (10, 11) },
            { 11, (11, 12) },
            { 12, (0, 13) },
            { 13, (13, 14) },
            { 14, (14, 15) },
            { 15, (15, 16) },
            { 16, (0, 17) },
            { 17, (17, 18) },
            { 18, (18, 19) },
            { 19, (19, 20) }
    };

    private float sigma = 0.5f;// Gaussian Filter sigma

    public int showWhichHandOnTimeline;

    public GameObject handR;
    public GameObject handL;
    public int hand_num;
    public string CSVname;

    public GameObject[] joint;
    public GameObject[] bone;

    public Image VideoLeft;
    private Sprite ImgLeft;
    private string ImgLeftPath;
    public Image VideoRight;
    private Sprite ImgRight;
    private string ImgRightPath;

    public bool autoPlay = false;
    private TextAsset csvFile;
    public List<string[]> csvDatas = new List<string[]>(); // 

    private int bone_num = 20;
    public int joint_num = 21;

    //当前帧的s
    public Vector3[] currentFramePos;
    public Quaternion[] currentFramePosBone;
    public Vector3[] currentFramePosBone_vec;
    public List<Vector3[]> VecList = new List<Vector3[]>(); //to store every frame data in Start()
    public List<Vector3[]> QuaList = new List<Vector3[]>();
    public Load_CSV other;


    //xxx帧犯错程度数组
    public float[] errorRank0;
    public float[] errorRank1;
    public float[] errorRank2;
    public float[] errorRank0Left;
    public float[] errorRank1Left;
    public float[] errorRank2Left;
    public float[] errorRank0Right;
    public float[] errorRank1Right;
    public float[] errorRank2Right;

    public int csvCount;

    //Piano Key Pressed or Lift
    //public Transform[] PianoKeys;



    //------------------------------------------Gaussian Filter---------------------------------
    private static float[] gaussianKernel1d(int kernelRadius, float sigma)
    {
        float[] kernel = new float[kernelRadius + 1 + kernelRadius];
        for (int xx = -kernelRadius; xx <= kernelRadius; xx++)
            kernel[kernelRadius + xx] = (float)(Math.Exp(-(xx * xx) / (2 * sigma * sigma)) /
                    (Math.PI * 2 * sigma * sigma));
        return kernel;
    }

    private static int clamp(int value, int min, int max)
    {
        if (value < min)
            return min;
        if (value > max)
            return max;
        return value;
    }

    static float[] filter(float[] array, float[] kernel)
    {
        if (kernel.Length % 2 == 1)
        {
            print("kernel size is correct");
        } //kernel size must be odd.
        int kernelRadius = kernel.Length / 2;
        int width = array.GetLength(0);
        float[] result = new float[width];
        for (int x = 0; x < width; x++)
        {
            float sumOfValues = 0;
            float sumOfWeights = 0;
            for (int i = -kernelRadius; i <= kernelRadius; i++)
            {
                float value = array[clamp(x + i, 0, width - 1)];
                float weight = kernel[kernelRadius + i];
                sumOfValues += value * weight;
                sumOfWeights += weight;
            }
            result[x] = sumOfValues / sumOfWeights;
        }
        return result;
    }
    //------------------------------------------Gaussian Filter End---------------------------------


    void Awake()
    {
        var hand = handL;
        var bones = hand.transform.Find("Bones").gameObject;
        //loadDict();

        //assign joints
        joint = new GameObject[joint_num * hand_num];
        for (int i = 0; i < joint_num; i++)
        {
            joint[i] = hand.transform.Find("Joint (" + i.ToString() + ")").gameObject;
        }
        //assign bones
        bone = new GameObject[bone_num * hand_num];
        for (int i = 0; i < bone_num; i++)
        {
            bone[i] = bones.transform.Find("Bone (" + i.ToString() + ")").gameObject;
        }


        currentFramePos = new Vector3[joint_num * hand_num];
        currentFramePosBone = new Quaternion[bone_num * hand_num];
        currentFramePosBone_vec = new Vector3[bone_num * hand_num];


        if (hand_num == 2)
        {
            //handR.transform.Rotate(90, 0, 0);
            hand = handR;
            bones = hand.transform.Find("Bones").gameObject;
            for (int i = 0; i < joint_num; i++)
            {
                joint[i + joint_num] = hand.transform.Find("Joint (" + i.ToString() + ")").gameObject;
            }
            for (int i = 0; i < bone_num; i++)
            {
                bone[i + bone_num] = bones.transform.Find("Bone (" + i.ToString() + ")").gameObject;
            }
        }
        csvFile = Resources.Load(CSVname) as TextAsset; // Read CSV 
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() != -1) //
        {
            string line = reader.ReadLine(); // read by line
            csvDatas.Add(line.Split(',')); // 
        }

        int frameSum = (int)(csvDatas.Count);
        csvCount = frameSum;
        //print(csvCount);

        float x = 0f; float y = 0f; float z = 0f;

        for (int frameLoc = 0; frameLoc < frameSum; frameLoc++)
        {
            Vector3[] c = new Vector3[joint_num * hand_num];
            //获取第frameLoc帧的joint position
            for (int i = 0; i < joint_num * hand_num; i++)
            {

                x = float.Parse(csvDatas[frameLoc][i * 3]);
                y = float.Parse(csvDatas[frameLoc][i * 3 + 1]);
                z = float.Parse(csvDatas[frameLoc][i * 3 + 2]);
                c[i] = new Vector3(x, y, z);
            }
            VecList.Add(c);
            
        }

        for (int frameLoc = 0; frameLoc < frameSum; frameLoc++)
        {
            Vector3[] q = new Vector3[bone_num * hand_num];
            //Quaternion[] q = new Quaternion[bone_num * hand_num];
            for (int i = 0; i < bone_num * hand_num; i++)
            {
                int s = bones_dict[i % bone_num].Item1 + i / bone_num * joint_num;
                int e = bones_dict[i % bone_num].Item2 + i / bone_num * joint_num;
                Vector3 start = VecList[frameLoc][s];
                Vector3 end = VecList[frameLoc][e];         
                Vector3 pos = end - start;
                pos = pos.normalized;
                q[i] = pos;
            }
            QuaList.Add(q);
        }

    }


    void Start()
    {
        int frameSum = csvDatas.Count;

        errorRank0 = ErrorRank0(frameSum);
        errorRank0Left = ErrorRank0Left(frameSum);
        errorRank0Right = ErrorRank0Right(frameSum);

        errorRank1 = ErrorRank1(frameSum);
        errorRank1Left = ErrorRank1Left(frameSum);
        errorRank1Right = ErrorRank1Right(frameSum);

        errorRank2 = ErrorRank2(frameSum);
        errorRank2Left = ErrorRank2Left(frameSum);
        errorRank2Right = ErrorRank2Right(frameSum);
    }

    float[] ErrorRank0(int frameSum)
    {
        float[] errorRankType0 = new float[frameSum];
        for (int frameLoc = 0; frameLoc < frameSum; frameLoc++)
        {
            float frameError0to1 = 0;
            for (int i = 0 ; i < joint_num * hand_num; i++)
            {
                float distance = Vector3.Distance(VecList[frameLoc][i], other.VecList[frameLoc][i]);
                float jointError0to1 = distance / Compare.I.errorInfos[0].maxError;
                if (jointError0to1 > frameError0to1)
                {
                    frameError0to1 = jointError0to1;
                }
            }
            errorRankType0[frameLoc] = frameError0to1;
        }

        int kernelRadius = (int)Math.Ceiling(sigma * 2.57); // significant radius
        float[] kernel = gaussianKernel1d(kernelRadius, sigma);
        float[] result = filter(errorRankType0, kernel);

        return result;
    }

    float[] ErrorRank0Left(int frameSum)
    {
        float[] errorRankType0 = new float[frameSum];
        for (int frameLoc = 0; frameLoc < frameSum; frameLoc++)
        {
            float frameError0to1 = 0;
            for (int i = 0; i < joint_num ; i++)
            {
                float distance = Vector3.Distance(VecList[frameLoc][i], other.VecList[frameLoc][i]);
                float jointError0to1 = distance / Compare.I.errorInfos[0].maxError;
                if (jointError0to1 > frameError0to1)
                {
                    frameError0to1 = jointError0to1;
                }
            }
            errorRankType0[frameLoc] = frameError0to1;
        }

        int kernelRadius = (int)Math.Ceiling(sigma * 2.57); // significant radius
        float[] kernel = gaussianKernel1d(kernelRadius, sigma);
        float[] result = filter(errorRankType0, kernel);

        return result;
    }

    float[] ErrorRank0Right(int frameSum)
    {
        float[] errorRankType0 = new float[frameSum];
        for (int frameLoc = 0; frameLoc < frameSum; frameLoc++)
        {
            float frameError0to1 = 0;
            for (int i = joint_num; i < joint_num * hand_num; i++)
            {
                float distance = Vector3.Distance(VecList[frameLoc][i], other.VecList[frameLoc][i]);
                float jointError0to1 = distance / Compare.I.errorInfos[0].maxError;
                if (jointError0to1 > frameError0to1)
                {
                    frameError0to1 = jointError0to1;
                }
            }
            errorRankType0[frameLoc] = frameError0to1;
        }

        int kernelRadius = (int)Math.Ceiling(sigma * 2.57); // significant radius
        float[] kernel = gaussianKernel1d(kernelRadius, sigma);
        float[] result = filter(errorRankType0, kernel);

        return result;
    }

    float[] ErrorRank1(int frameSum)
    {
        float[] errorRankType1=new float[frameSum];
        for (int frameLoc = 0; frameLoc < frameSum; frameLoc++)
        {
            float frameError0to1 = 0;
            //int count = 0;
            for (int i = 0; i < (joint_num - 1) * hand_num; i++)
            {
                float angle = Vector3.Angle(QuaList[frameLoc][i], other.QuaList[frameLoc][i]);
                float angleError0to1 = angle / Compare.I.errorInfos[1].maxError;
                if (angleError0to1 > frameError0to1)
                {
                    frameError0to1 = angleError0to1;
                }
            }
            //print("frameError" + frameError0to1);
            errorRankType1[frameLoc] = frameError0to1;
        }
        int kernelRadius = (int)Math.Ceiling(sigma * 2.57); // significant radius
        float[] kernel = gaussianKernel1d(kernelRadius, sigma);
        float[] result = filter(errorRankType1, kernel);
        return result;
    }

    float[] ErrorRank1Left(int frameSum)
    {
        float[] errorRankType1 = new float[frameSum];
        for (int frameLoc = 0; frameLoc < frameSum; frameLoc++)
        {
            float frameError0to1 = 0;
            //int count = 0;
            for (int i = 0; i < joint_num - 1; i++)
            {
                float angle = Vector3.Angle(QuaList[frameLoc][i], other.QuaList[frameLoc][i]);
                float angleError0to1 = angle / Compare.I.errorInfos[1].maxError;
                if (angleError0to1 > frameError0to1)
                {
                    frameError0to1 = angleError0to1;
                }
            }
            //print("frameError" + frameError0to1);
            errorRankType1[frameLoc] = frameError0to1;
        }
        int kernelRadius = (int)Math.Ceiling(sigma * 2.57); // significant radius
        float[] kernel = gaussianKernel1d(kernelRadius, sigma);
        float[] result = filter(errorRankType1, kernel);
        return result;
    }

    float[] ErrorRank1Right(int frameSum)
    {
        float[] errorRankType1 = new float[frameSum];
        for (int frameLoc = 0; frameLoc < frameSum; frameLoc++)
        {
            float frameError0to1 = 0;
            //int count = 0;
            for (int i = joint_num-1; i < (joint_num - 1) * hand_num; i++)
            {
                float angle = Vector3.Angle(QuaList[frameLoc][i], other.QuaList[frameLoc][i]);
                float angleError0to1 = angle / Compare.I.errorInfos[1].maxError;
                if (angleError0to1 > frameError0to1)
                {
                    frameError0to1 = angleError0to1;
                }
            }
            //print("frameError" + frameError0to1);
            errorRankType1[frameLoc] = frameError0to1;
        }
        int kernelRadius = (int)Math.Ceiling(sigma * 2.57); // significant radius
        float[] kernel = gaussianKernel1d(kernelRadius, sigma);
        float[] result = filter(errorRankType1, kernel);
        return result;
    }

    float[] ErrorRank2(int frameSum)
    {
        float[] errorRankType2 = new float[frameSum];

        for (int frameLoc = 0; frameLoc < frameSum; frameLoc++)
        {
            float ErrorAngle = 0;
            float PlayerAngle = 0;
            float CoachAngle = 0;
            float frameError0to1 = 0;

            for (int i = 0; i < (joint_num - 1) * hand_num; i++)
            {
                if (i == 3 || i == 3 + joint_num - 1)
                {
                    Vector3.Angle(QuaList[frameLoc][i], other.QuaList[frameLoc][i]);
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i + 16]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i + 16]);
                    ErrorAngle= Mathf.Abs(PlayerAngle - CoachAngle);
                }
                else if (i == 7 || i == 7 + joint_num - 1)
                {
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i - 4]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i - 4]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }
                else if (i == 15 || i == 15 + joint_num - 1)
                {
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i -8]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i -8]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }
                else if (i == 11 || i == 11 + joint_num - 1)
                {
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i + 4]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i + 4]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }
                else if (i == 19 || i == 19 + joint_num - 1)
                {                
                }
                else
                {
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i + 1]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i + 1]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }


                float adAngleError0to1 = ErrorAngle / Compare.I.errorInfos[2].maxError;
                if (adAngleError0to1 > frameError0to1)
                {
                    frameError0to1 = adAngleError0to1;
                }
            }
            errorRankType2[frameLoc] = frameError0to1;
        }
        int kernelRadius = (int)Math.Ceiling(sigma * 2.57); // significant radius
        float[] kernel = gaussianKernel1d(kernelRadius, sigma);
        float[] result = filter(errorRankType2, kernel);
        return result;
    }

    float[] ErrorRank2Left(int frameSum)
    {
        float[] errorRankType2 = new float[frameSum];

        for (int frameLoc = 0; frameLoc < frameSum; frameLoc++)
        {
            float ErrorAngle = 0;
            float PlayerAngle = 0;
            float CoachAngle = 0;
            float frameError0to1 = 0;

            for (int i = 0; i < joint_num - 1; i++)
            {
                if (i == 3 || i == 3 + joint_num - 1)
                {
                    Vector3.Angle(QuaList[frameLoc][i], other.QuaList[frameLoc][i]);
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i + 16]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i + 16]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }
                else if (i == 7 || i == 7 + joint_num - 1)
                {
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i - 4]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i - 4]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }
                else if (i == 15 || i == 15 + joint_num - 1)
                {
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i - 8]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i - 8]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }
                else if (i == 11 || i == 11 + joint_num - 1)
                {
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i + 4]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i + 4]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }
                else if (i == 19 || i == 19 + joint_num - 1)
                {
                }
                else
                {
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i + 1]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i + 1]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }


                float adAngleError0to1 = ErrorAngle / Compare.I.errorInfos[2].maxError;
                if (adAngleError0to1 > frameError0to1)
                {
                    frameError0to1 = adAngleError0to1;
                }
            }
            errorRankType2[frameLoc] = frameError0to1;
        }
        int kernelRadius = (int)Math.Ceiling(sigma * 2.57); // significant radius
        float[] kernel = gaussianKernel1d(kernelRadius, sigma);
        float[] result = filter(errorRankType2, kernel);
        return result;
    }

    float[] ErrorRank2Right(int frameSum)
    {
        float[] errorRankType2 = new float[frameSum];

        for (int frameLoc = 0; frameLoc < frameSum; frameLoc++)
        {
            float ErrorAngle = 0;
            float PlayerAngle = 0;
            float CoachAngle = 0;
            float frameError0to1 = 0;

            for (int i = joint_num-1; i < (joint_num - 1) * hand_num; i++)
            {
                if (i == 3 || i == 3 + joint_num - 1)
                {
                    Vector3.Angle(QuaList[frameLoc][i], other.QuaList[frameLoc][i]);
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i + 16]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i + 16]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }
                else if (i == 7 || i == 7 + joint_num - 1)
                {
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i - 4]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i - 4]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }
                else if (i == 15 || i == 15 + joint_num - 1)
                {
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i - 8]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i - 8]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }
                else if (i == 11 || i == 11 + joint_num - 1)
                {
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i + 4]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i + 4]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }
                else if (i == 19 || i == 19 + joint_num - 1)
                {
                }
                else
                {
                    PlayerAngle = Vector3.Angle(other.QuaList[frameLoc][i], other.QuaList[frameLoc][i + 1]);
                    CoachAngle = Vector3.Angle(QuaList[frameLoc][i], QuaList[frameLoc][i + 1]);
                    ErrorAngle = Mathf.Abs(PlayerAngle - CoachAngle);
                }


                float adAngleError0to1 = ErrorAngle / Compare.I.errorInfos[2].maxError;
                if (adAngleError0to1 > frameError0to1)
                {
                    frameError0to1 = adAngleError0to1;
                }
            }
            errorRankType2[frameLoc] = frameError0to1;
        }
        int kernelRadius = (int)Math.Ceiling(sigma * 2.57); // significant radius
        float[] kernel = gaussianKernel1d(kernelRadius, sigma);
        float[] result = filter(errorRankType2, kernel);
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rate">播放百分之多少</param>
    public void UpdateHands(float rate)
    {
        //handR.transform.Rotate(90, 0, 0);
        int frame = (int)(rate * csvDatas.Count);
        if (frame == csvDatas.Count)
        {
            //print("frame" + frame);
        }
        else
        {
            //create above align videos by a series of imgs
            ImgLeftPath = "outputImg/task1/1/" + frame.ToString();
            ImgLeft = Resources.Load(ImgLeftPath, typeof(Sprite)) as Sprite;
            VideoLeft.overrideSprite = ImgLeft;

            ImgRightPath = "outputImg/task1/2/" + frame.ToString();
            ImgRight = Resources.Load(ImgRightPath, typeof(Sprite)) as Sprite;
            VideoRight.overrideSprite = ImgRight;

            float x = 0f; float y = 0f; float z = 0f;

            for (int i = 0; i < joint_num * hand_num; i++)
            {
                //print("f"+frame);
                x = float.Parse(csvDatas[frame][i * 3]);
                y = float.Parse(csvDatas[frame][i * 3 + 1]);
                z = float.Parse(csvDatas[frame][i * 3 + 2]);

                if (i == 20 || i == 41)
                {
                    joint[i].transform.position = new Vector3(x, y, z);
                }
                else
                {
                    joint[i].transform.position = new Vector3(x, y, z);
                }

                currentFramePos[i] = joint[i].transform.position;
            }

            for (int i = 0; i < bone_num * hand_num; i++)
            {
                Vector3 start = joint[bones_dict[i % bone_num].Item1 + i / bone_num * joint_num].transform.position;
                Vector3 end = joint[bones_dict[i % bone_num].Item2 + i / bone_num * joint_num].transform.position;
                Vector3 pos = Vector3.Lerp(start, end, (float)0.5);
                bone[i].transform.position = pos;
                bone[i].transform.up = start - end;
                bone[i].transform.localScale = new Vector3(bone[i].transform.localScale.x, Vector3.Distance(start, end) / 2f, bone[i].transform.localScale.z);
                currentFramePosBone[i] = bone[i].transform.rotation;


                int s = bones_dict[i % bone_num].Item1 + i / bone_num * joint_num;
                int e = bones_dict[i % bone_num].Item2 + i / bone_num * joint_num;
                Vector3 start_v = VecList[frame][s];
                Vector3 end_v = VecList[frame][e];
                Vector3 pos_v = end_v - start_v;
                pos_v = pos_v.normalized;
                currentFramePosBone_vec[i] = pos_v;
            }

            PlayTimeLine.I.AddTime(csvDatas.Count);
        }
    }



    public void ShowErrorPosition(int index, bool hasError, Color color)
    {
        joint[index].SetActive(hasError);

        if (hasError)
        {
            joint[index].GetComponent<Renderer>().material.color = color;
            //if (index <= 39) bone[index].GetComponent<Renderer>().material.color = color;
            if (index>0&&index <= 20) bone[index-1].GetComponent<Renderer>().material.color = color;
            if (index > 21 && index <= 41) bone[index - 2].GetComponent<Renderer>().material.color = color;
        }
    }
    public void ShowErrorAngle(int index, bool hasError, Color color)
    {


        if (hasError)
        {
            joint[index].GetComponent<Renderer>().material.color = color;
            if (index <= 39) bone[index].GetComponent<Renderer>().material.color = color;
        }
    }

    public void ShowErrorAdjacentAngle(int i, bool hasError, Color color)
    {
        bone[i].SetActive(hasError);
        bone[i].GetComponent<Renderer>().material.color = new Color(0.18f, 0.72f, 0.13f, 1);

        joint[i + 1].SetActive(hasError);

        if (i == 3 || i == 3 + joint_num - 1)
        {
            if (bone[i].GetComponent<Renderer>().material.color == Color.white)
            {
                bone[i].GetComponent<Renderer>().material.color = color;
            }
            if (bone[i + 16].GetComponent<Renderer>().material.color == Color.white)
            {
                bone[i + 16].GetComponent<Renderer>().material.color = color;
            }
        }
        else if (i == 7 || i == 7 + joint_num - 1)
        {
            if (bone[i].GetComponent<Renderer>().material.color == new Color(0.18f,0.72f,0.13f,1))
            {
                bone[i].GetComponent<Renderer>().material.color = color;
            }
            if (bone[i - 4].GetComponent<Renderer>().material.color == new Color(0.18f,0.72f,0.13f,1))
            {
                bone[i - 4].GetComponent<Renderer>().material.color = color;
            }
        }
        else if (i == 15 || i == 15 + joint_num - 1)
        {
            if (bone[i].GetComponent<Renderer>().material.color == new Color(0.18f,0.72f,0.13f,1))
            {
                bone[i].GetComponent<Renderer>().material.color = color;
            }
            if (bone[i - 8].GetComponent<Renderer>().material.color == new Color(0.18f,0.72f,0.13f,1))
            {
                bone[i - 8].GetComponent<Renderer>().material.color = color;
            }
        }
        else if (i == 11 || i == 11 + joint_num - 1)
        {
            if (bone[i].GetComponent<Renderer>().material.color == new Color(0.18f,0.72f,0.13f,1))
            {
                bone[i].GetComponent<Renderer>().material.color = color;
            }
            if (bone[i + 4].GetComponent<Renderer>().material.color == new Color(0.18f,0.72f,0.13f,1))
            {
                bone[i + 4].GetComponent<Renderer>().material.color = color;
            }
        }
        else if (i == 19 || i == 19 + joint_num - 1) { }
        else
        {
            if (bone[i].GetComponent<Renderer>().material.color == new Color(0.18f,0.72f,0.13f,1))
            {
                bone[i].GetComponent<Renderer>().material.color = color;
            }
            if (bone[i + 1].GetComponent<Renderer>().material.color == new Color(0.18f,0.72f,0.13f,1))
            {
                bone[i + 1].GetComponent<Renderer>().material.color = color;
            }
        }

    }
}


