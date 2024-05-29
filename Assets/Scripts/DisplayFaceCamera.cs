using UnityEngine;

public class DisplayFaceCamera : MonoBehaviour
{
    Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    
    void Update()
    {
        transform.LookAt(canvas.worldCamera.transform.position, -Vector3.up);
    }
}
