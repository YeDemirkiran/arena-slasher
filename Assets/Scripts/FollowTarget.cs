using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
