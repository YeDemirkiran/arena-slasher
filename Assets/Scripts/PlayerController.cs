using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BotController))]
public class PlayerController : MonoBehaviour
{
    BotController controller;

    public float health { get { return controller.health; } }
    public float maxHealth { get { return controller.maxHealth; } }    

    void Awake()
    {
        controller = GetComponent<BotController>();
    }

    void Update()
    {
        controller.Move(Input.GetAxis("Vertical"));
        controller.Rotate(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            controller.Attack();
        }

        //controller.ApplyGravity();
        //controller.ApplyMovement();   
    } 
}