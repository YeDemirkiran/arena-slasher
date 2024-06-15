using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    Transform player { get { return PlayerController.Instance.transform; } }
    [SerializeField] Vector3 offset;

    void LateUpdate()
    {
        if (player == null) return;
        //transform.position = target.position + (target.forward * offset.z) + (target.right * offset.x) + (target.up * offset.y);
        transform.position = player.position + offset;
    }
}
