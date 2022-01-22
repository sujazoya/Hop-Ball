using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Free_CameraLook : MonoBehaviour
{
    public Transform target;

    [SerializeField] float smoothSpeed;
    [SerializeField] Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = new Vector3(transform.position.x, transform.position.y, smoothedPos.z);
    }
}
