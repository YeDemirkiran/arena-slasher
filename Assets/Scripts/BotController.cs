using UnityEngine;

public class BotController : MonoBehaviour
{
    CharacterController controller;

    public float runningSpeed;
    public float rotationSpeed;
    public float gravity = -9.81f;

    Vector3 horizontalVelocity, verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Move(float input)
    {
        horizontalVelocity = transform.forward * input * runningSpeed * Time.deltaTime;
    }

    public void Rotate(float inputX)
    {
        transform.eulerAngles += Vector3.up * inputX * rotationSpeed * Time.deltaTime;
    }

    public void ApplyGravity()
    {
        if (controller.isGrounded) verticalVelocity.y = 0f;
        else verticalVelocity.y += gravity * Time.deltaTime * Time.deltaTime;
    }

    public void ApplyMovement()
    {
        controller.Move(horizontalVelocity + verticalVelocity);
    }
}