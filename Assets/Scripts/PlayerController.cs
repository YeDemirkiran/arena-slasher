using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BotController))]
public class PlayerController : MonoBehaviour
{
    public float health {  get; set; }
    public float maxHealth = 100f;
    public UnityAction onDeath;

    BotController controller;

    void Awake()
    {
        controller = GetComponent<BotController>();
    }

    void Start()
    {
        health = maxHealth;   
    }

    void Update()
    {
        controller.Move(Input.GetAxis("Vertical"));
        controller.Rotate(Input.GetAxis("Horizontal"));

        controller.ApplyGravity();
        controller.ApplyMovement();   

        if (health <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        onDeath?.Invoke();
    }
}