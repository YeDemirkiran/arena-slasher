using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    void LateUpdate()
    {
        //transform.position = target.position + (target.forward * offset.z) + (target.right * offset.x) + (target.up * offset.y);
        transform.position = target.position + offset;
    }
}
