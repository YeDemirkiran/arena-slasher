using UnityEngine;
using Unity.Mathematics;

public class ChildOf : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] bool3 positionLock, rotationLock;
    [SerializeField] bool automaticPositionOffset;
    [SerializeField] Vector3 manualPositionOffset;

    Vector3 autoOffset;

    void Start()
    {
        autoOffset = transform.position - parent.transform.position;
        //Debug.Log("pos: " + transform.position);
        //Debug.Log("parpos: " + parent.transform.position);
        //Debug.Log("Auto Offset: " + autoOffset);
    }

    void Update()
    {
        if (GameManager.Instance.state != GameManager.GameState.Running) { return; }

        Vector3 position, rotation;

        position.x = positionLock.x ? parent.position.x : transform.position.x;
        position.y = positionLock.y ? parent.position.y : transform.position.y;
        position.z = positionLock.z ? parent.position.z : transform.position.z;

        position += parent.transform.right * (automaticPositionOffset ? autoOffset.x : manualPositionOffset.x);
        position += parent.transform.up * (automaticPositionOffset ? autoOffset.y : manualPositionOffset.y);
        position += parent.transform.forward * (automaticPositionOffset ? autoOffset.z : manualPositionOffset.z);

        rotation.x = rotationLock.x ? parent.eulerAngles.x : transform.eulerAngles.x;
        rotation.y = rotationLock.y ? parent.eulerAngles.y : transform.eulerAngles.y;
        rotation.z = rotationLock.z ? parent.eulerAngles.z : transform.eulerAngles.z;

        transform.position = position;
        transform.eulerAngles = rotation;
    }
}