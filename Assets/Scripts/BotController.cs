using UnityEngine;
using UnityEngine.Events;

public class BotController : MonoBehaviour
{
    CharacterController controller;

    public float runningSpeed = 10f;
    public float rotationSpeed = 250f;
    public float gravity = -9.81f;

    public float health { get; set; }
    public float maxHealth = 100f;
    public UnityAction onDeath, onAttack;

    public Weapons weaponsList;
    public Weapon currentWeapon { get; private set; }
    [SerializeField] AudioSource audioSource;
    float attackTimer = 0f;

    Vector3 horizontalVelocity, verticalVelocity;

    AttackBox attackBox;

    bool moveCalledThisFrame = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        attackBox = GetComponentInChildren<AttackBox>();
    }

    void Start()
    {
        health = maxHealth;
        currentWeapon = weaponsList.weapons[0];
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

        if (!moveCalledThisFrame) { horizontalVelocity = Vector3.zero; }
        moveCalledThisFrame = false;
    }

    public void Move(float input)
    {
        horizontalVelocity = transform.forward * input * runningSpeed * Time.deltaTime;
        moveCalledThisFrame = true;
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
        if (attackTimer < currentWeapon.cooldown || attackBox.enemies.Count == 0) return;

        for (int i = attackBox.enemies.Count - 1; i >= 0; i--)
        {
            BotController enemy = attackBox.enemies[i];

            if (enemy == null)
            {
                //Debug.Log("Enemy is already dead, removing from list");
                attackBox.enemies.Remove(attackBox.enemies[i]);

                if (attackBox.enemies.Count == 0) return;
                else continue;
            }

            enemy.health -= currentWeapon.damagePerHit;
        }

        attackTimer = 0f;
        onAttack?.Invoke();
        audioSource.PlayOneShot(currentWeapon.attackSoundClips[Random.Range(0, currentWeapon.attackSoundClips.Length)]);
    }

    public void OnDeath()
    {
        onDeath?.Invoke();

        Destroy(gameObject);
    }
}