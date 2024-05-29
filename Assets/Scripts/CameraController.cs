using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 100f;

    void Update()
    {
        transform.eulerAngles += Vector3.up * sensitivity * Input.GetAxis("Mouse X") * Time.deltaTime;
    }
}