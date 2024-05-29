using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] Vector3 speed;

    void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
    }
}
