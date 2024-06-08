using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 speed;
    [SerializeField] bool unscaled = false;

    void Update()
    {
        if (target == null) transform.Rotate(speed * (unscaled ? Time.unscaledDeltaTime : Time.deltaTime));
        else 
        { 
            transform.RotateAround(target.position, Vector3.right, speed.x * (unscaled ? Time.unscaledDeltaTime : Time.deltaTime)); 
            transform.RotateAround(target.position, Vector3.up, speed.y * (unscaled ? Time.unscaledDeltaTime : Time.deltaTime)); 
            transform.RotateAround(target.position, Vector3.forward, speed.z * (unscaled ? Time.unscaledDeltaTime : Time.deltaTime)); 
            transform.LookAt(target.position);
        }
    }
}
