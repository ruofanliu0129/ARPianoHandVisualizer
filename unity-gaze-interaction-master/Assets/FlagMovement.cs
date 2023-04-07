using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMovement : MonoBehaviour
{
    public GameObject Flag;
    //PassSound;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = Flag.transform.position;
        if (position.z < 0)
        {
            position.z += 30;
            //PassSound.GetComponent<PassSound>().PlayPassSound(position.x);
        }
        position.z -= speed * 1000 / 3600 * Time.deltaTime;
        Flag.transform.position = position;
    }
}
