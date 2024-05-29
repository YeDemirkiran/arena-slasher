using UnityEngine;

[RequireComponent(typeof(BotController))]
public class PlayerController : MonoBehaviour
{
    BotController controller;

    void Awake()
    {
        controller = GetComponent<BotController>();
    }

    void Update()
    {
        controller.Move(Input.GetAxis("Vertical"));
        controller.Rotate(Input.GetAxis("Horizontal"));

        controller.ApplyGravity();
        controller.ApplyMovement();   
    }
}