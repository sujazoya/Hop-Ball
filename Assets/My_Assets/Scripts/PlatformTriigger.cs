using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTriigger : MonoBehaviour
{
    [SerializeField] Transform platform;
    bool triggerCalled;
   
    // Start is called before the first frame update
    void Start()
    {
        platform = transform.parent.transform;     

    }   
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player")&&!triggerCalled)
        {
            //Debug.Log("Platform Reseted");
           triggerCalled = true;          
           platform.localPosition = new Vector3(
           platform.position.x,
           platform.position.y,
            PlayerManager.Instance.NextZ());
            Invoke("DeactiveTrig", 10f);
        }
    }
    void DeactiveTrig()
    {
        triggerCalled = true;
    }
}
