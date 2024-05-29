using UnityEngine;
using UnityEngine.Events;

public class BotController : MonoBehaviour
{
    CharacterController controller;

    public float runningSpeed;
    public float rotationSpeed;
    public float gravity = -9.81f;

    public float health { get; set; }
    public float maxHealth = 100f;
    public UnityAction onDeath;

    public float damage = 10f;
    public float attackCooldown = 1f;
    float attackTimer = 0f;

    Vector3 horizontalVelocity, verticalVelocity;

    AttackBox attackBox;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        attackBox = GetComponentInChildren<AttackBox>();
    }

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        ApplyGravity();
        ApplyMovement();

        attackTimer += Time.deltaTime;

        if (health <= 0)
        {
            OnDeath();
        }
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

    public void Attack()
    {
        if (attackTimer < attackCooldown) return;

        attackTimer = 0f;

        for (int i = attackBox.enemies.Count - 1; i >= 0; i--)
        {
            BotController enemy = attackBox.enemies[i];

            if (enemy == null)
            {
                //Debug.Log("Enemy is already dead, removing from list");
                attackBox.enemies.Remove(attackBox.enemies[i]);
                continue;
            }

            enemy.health -= damage;
        }
    }

    public void OnDeath()
    {
        onDeath?.Invoke();

        Destroy(gameObject);
    }
}