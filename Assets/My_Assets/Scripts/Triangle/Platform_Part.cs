using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Part : MonoBehaviour
{
    Transform myTransform;
    float speed=5;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myTransform.position += new Vector3(speed * Time.deltaTime, myTransform.position.y, transform.position.z);
    }
}
