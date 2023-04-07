using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class createLine : UnitySingleton<createLine>
{
    public Image img;

    public Load_CSV csv;
    public List<Image> imgList0 = new List<Image>();
    public List<Image> imgList0Left = new List<Image>();
    public List<Image> imgList0Right = new List<Image>();
    public List<Image> imgList1 = new List<Image>();
    public List<Image> imgList1Left = new List<Image>();
    public List<Image> imgList1Right = new List<Image>();
    public List<Image> imgList2 = new List<Image>();
    public List<Image> imgList2Left = new List<Image>();
    public List<Image> imgList2Right = new List<Image>();

    //private Color32 green32 = Color.green;
    private Color32 green32 = new Color(0.0f, 1.0f, 0.0f, 0.0f);
    //private Color32 red32 = Color.red;
    private Color32 red32 = new Color(1.0f, 0.0f, 0.0f, 1.0f);

    //First initialize the yuzhiti
    void Start()
    {
        StartCoroutine(Delay());
    }
    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        float[] errorRank0, errorRank1, errorRank2, errorRank0Left, errorRank1Left, errorRank2Left, errorRank0Right, errorRank1Right, errorRank2Right;
        errorRank0 = csv.errorRank0;
        errorRank0Left = csv.errorRank0Left;
        errorRank0Right = csv.errorRank0Right;
        errorRank1 = csv.errorRank1;
        errorRank1Left = csv.errorRank1Left;
        errorRank1Right = csv.errorRank1Right;
        errorRank2 = csv.errorRank2;
        errorRank2Left = csv.errorRank2Left;
        errorRank2Right = csv.errorRank2Right;


        int count = errorRank0.Length;
        float timesliderLength = 1000f;

        print("linesCount"+count);
        for (int i = 0; i < count; i++)
        {
            if (errorRank0[i] > 0)
            {
                Image NewArraw;
                float x;
                x =  (timesliderLength / (float)count) * (i + 1);
                img.color = Color32.Lerp(green32, red32, errorRank0[i]);
                NewArraw = (Image)Instantiate(img, new Vector3(x, -500, 0), Quaternion.identity);
                NewArraw.transform.SetParent(GameObject.Find("ColorPartition").transform, false);      
                imgList0.Add(NewArraw);
            }
            if (errorRank0Left[i] > 0)
            {
                Image NewArraw;
                float x;
                x = (timesliderLength / (float)count) * (i + 1);
                img.color = Color32.Lerp(green32, red32, errorRank0Left[i]);
                NewArraw = (Image)Instantiate(img, new Vector3(x, -500, 0), Quaternion.identity);
                NewArraw.transform.SetParent(GameObject.Find("ColorPartition").transform, false);
                imgList0Left.Add(NewArraw);
            }
            if (errorRank0Right[i] > 0)
            {
                Image NewArraw;
                float x;
                x = (timesliderLength / (float)count) * (i + 1);
                img.color = Color32.Lerp(green32, red32, errorRank0Right[i]);
                NewArraw = (Image)Instantiate(img, new Vector3(x, -500, 0), Quaternion.identity);
                NewArraw.transform.SetParent(GameObject.Find("ColorPartition").transform, false);
                imgList0Right.Add(NewArraw);
            }
            if (errorRank1[i] > 0)
            {
                Image NewArraw;
                float x;
                x = (timesliderLength / (float)count) * (i + 1);
                img.color = Color32.Lerp(green32, red32, errorRank1[i]);
                NewArraw = (Image)Instantiate(img, new Vector3(x, -500, 0), Quaternion.identity);
                NewArraw.transform.SetParent(GameObject.Find("ColorPartition").transform, false);
                imgList1.Add(NewArraw);
            }
            if (errorRank1Left[i] > 0)
            {
                Image NewArraw;
                float x;
                x = (timesliderLength / (float)count) * (i + 1);
                img.color = Color32.Lerp(green32, red32, errorRank1Left[i]);
                NewArraw = (Image)Instantiate(img, new Vector3(x, -500, 0), Quaternion.identity);
                NewArraw.transform.SetParent(GameObject.Find("ColorPartition").transform, false);
                imgList1Left.Add(NewArraw);
            }
            if (errorRank1Right[i] > 0)
            {
                Image NewArraw;
                float x;
                x = (timesliderLength / (float)count) * (i + 1);
                img.color = Color32.Lerp(green32, red32, errorRank1Right[i]);
                NewArraw = (Image)Instantiate(img, new Vector3(x, -500, 0), Quaternion.identity);
                NewArraw.transform.SetParent(GameObject.Find("ColorPartition").transform, false);
                imgList1Right.Add(NewArraw);
            }
            if (errorRank2[i] > 0)
            {
                Image NewArraw;
                float x;
                x = (timesliderLength / (float)count) * (i + 1);
                img.color = Color32.Lerp(green32, red32, errorRank2[i]);
                NewArraw = (Image)Instantiate(img, new Vector3(x, -500, 0), Quaternion.identity);
                NewArraw.transform.SetParent(GameObject.Find("ColorPartition").transform, false);
                imgList2.Add(NewArraw);
            }
            if (errorRank2Left[i] > 0)
            {
                Image NewArraw;
                float x;
                x = (timesliderLength / (float)count) * (i + 1);
                img.color = Color32.Lerp(green32, red32, errorRank2Left[i]);
                NewArraw = (Image)Instantiate(img, new Vector3(x, -500, 0), Quaternion.identity);
                NewArraw.transform.SetParent(GameObject.Find("ColorPartition").transform, false);
                imgList2Left.Add(NewArraw);
            }
            if (errorRank2Right[i] > 0)
            {
                Image NewArraw;
                float x;
                x = (timesliderLength / (float)count) * (i + 1);
                img.color = Color32.Lerp(green32, red32, errorRank2Right[i]);
                NewArraw = (Image)Instantiate(img, new Vector3(x, -500, 0), Quaternion.identity);
                NewArraw.transform.SetParent(GameObject.Find("ColorPartition").transform, false);
                imgList2Right.Add(NewArraw);
            }
        }
    }

    public void changeImg(int type)
    {
        if (type == 0) //joint distance both hands
        {
            foreach (var item in imgList0)
            {
                item.enabled = true;
            }
            foreach (var item in imgList0Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Right)
            {
                item.enabled = false;
            }
        }
        else if (type == 1) //joint distance left hand
        {
            foreach (var item in imgList0)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Left)
            {
                item.enabled = true;
            }
            foreach (var item in imgList0Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Right)
            {
                item.enabled = false;
            }
        }
        else if (type == 2) //joint distance right hand
        {
            foreach (var item in imgList0)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Right)
            {
                item.enabled = true;
            }
            foreach (var item in imgList1)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Right)
            {
                item.enabled = false;
            }
        }
        else if (type == 3) //bone both hands
        {
            foreach (var item in imgList0)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1)
            {
                item.enabled = true;
            }
            foreach (var item in imgList1Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Right)
            {
                item.enabled = false;
            }
        }
        else if (type == 4) //bone left hand
        {
            foreach (var item in imgList0)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Left)
            {
                item.enabled = true;
            }
            foreach (var item in imgList1Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Right)
            {
                item.enabled = false;
            }
        }
        else if (type == 5) //bone right hand
        {
            foreach (var item in imgList0)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Right)
            {
                item.enabled = true;
            }
            foreach (var item in imgList2)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Right)
            {
                item.enabled = false;
            }
        }
        else if (type == 6) //adjacent bone both hands
        {
            foreach (var item in imgList0)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2)
            {
                item.enabled = true;
            }
            foreach (var item in imgList2Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Right)
            {
                item.enabled = false;
            }
        }
        else if (type == 7) //adjacent bone left hand
        {
            foreach (var item in imgList0)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Left)
            {
                item.enabled = true;
            }
            foreach (var item in imgList2Right)
            {
                item.enabled = false;
            }
        }
        else
        {
            foreach (var item in imgList0)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList0Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList1Right)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Left)
            {
                item.enabled = false;
            }
            foreach (var item in imgList2Right)
            {
                item.enabled = true;
            }
        }
    }

}