using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GaussianFilter : MonoBehaviour
{
    private static double[] gaussianKernel1d(int kernelRadius, double sigma)
    {
        double[] kernel = new double[kernelRadius + 1 + kernelRadius];
        for (int xx = -kernelRadius; xx <= kernelRadius; xx++)
            kernel[kernelRadius + xx] = Math.Exp(-(xx * xx) / (2 * sigma * sigma)) /
                    (Math.PI * 2 * sigma * sigma);
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


    static double[] filter(double[] array, double[] kernel)
    {
        if(kernel.Length % 2 == 1)
        {
            print("kernel size is correct");
        } //kernel size must be odd.
        //else
        //{
        //    return;
        //}
        int kernelRadius = kernel.Length / 2;
        int width = array.GetLength(0);
        double[] result = new double[width];
        for (int x = 0; x < width; x++)
        {
            double sumOfValues = 0;
            double sumOfWeights = 0;
            for (int i = -kernelRadius; i <= kernelRadius; i++)
            {
                double value = array[clamp(x + i, 0, width - 1)];
                double weight = kernel[kernelRadius + i];
                sumOfValues += value * weight;
                sumOfWeights += weight;
            }
            result[x] = sumOfValues / sumOfWeights;
        }
        return result;
    }
    // Start is called before the first frame update
    void Start()
    {
        double[] array = {0,0,1,5,1,0,1,2,3,4,5,4,3,2,1,0 };
        double sigma = 1;
        int kernelRadius = (int)Math.Ceiling(sigma * 2.57); // significant radius
        double[] kernel = gaussianKernel1d(kernelRadius, sigma);
        double[] result = filter(array, kernel);
        string str = "";
        for (int i = 0; i < result.Length; i++)
        {
            str = str + ", "+result[i].ToString();
            
        }
        print(str);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
