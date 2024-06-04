using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumbleweed : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Vector3 startForce, startTorque;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(startForce, ForceMode.Impulse);
        rb.AddRelativeTorque(startTorque, ForceMode.Impulse);
    }
}