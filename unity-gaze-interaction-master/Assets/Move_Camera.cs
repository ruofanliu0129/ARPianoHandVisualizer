using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Camera : MonoBehaviour
{
    public GameObject handVisualizer;
    public GameObject center;

    private bool startRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.S)) 
        {
		    handVisualizer.SetActive(false);
            this.transform.position = new Vector3 (center.transform.position.x, this.transform.position.y, center.transform.position.z);
            startRotation = true;
        }
        if(Input.GetKey(KeyCode.C)) 
        {
		    handVisualizer.SetActive(true);
            startRotation = false;    
        }

        if(startRotation)
        {
            this.transform.RotateAround (center.transform.position, Vector3.up, 33f * Time.deltaTime);

        }

    }
}
