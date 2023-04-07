using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.PXR;

public class HandTracking : MonoBehaviour
{
    //private PXR_HandTracking a = new PXR_HandTracking();
    // Start is called before the first frame update
    void Start()
    {
        //PXR_HandTracking a;
        //a = new PXR_HandTracking;
        ///PXR_HandTracking a = new PXR_HandTracking();
        bool output = PXR_HandTracking.GetSettingState();
        ActiveInputDevice b = PXR_HandTracking.GetActiveInputDevice();

        print("getsetstate"+output);
        print("inputDevice" + b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
