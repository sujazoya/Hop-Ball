using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotator : MonoBehaviour
{
    [Range(-100, 100)]
    [SerializeField] float speed;
    [SerializeField] bool x;
    [SerializeField] bool y;
    [SerializeField] bool z;
    [SerializeField]int index;
    // Start is called before the first frame update
    void Awake()
    {
        index = Random.Range(0, 2);
        if (index == 0)
        {
            speed = 1;
        }
        else
        {
            speed = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (x)
        {
            transform.Rotate(speed, 0, 0);
        }
        else if (y)
        {
            transform.Rotate(0, speed, 0);
        }
        else if (z)
        {
            transform.Rotate(0, 0, speed);
        }

    }
}
