using UnityEngine;
using UnityEngine.UIElements;

public class DisplayFaceCamera : MonoBehaviour
{
    //Canvas canvas;
    Transform cam;

    void Start()
    {
        //canvas = GetComponent<Canvas>();
        cam = Camera.main.transform;
    }

    
    void Update()
    {
        transform.LookAt(transform.position - (cam.position - transform.position));
        //transform.LookAt(cam.position, -Vector3.up);
    }
}
