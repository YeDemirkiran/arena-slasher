using UnityEngine;
using UnityEngine.Events;

public class BotController : MonoBehaviour
{
    CharacterController controller;

    [SerializeField] Animator animator;

    public float runningSpeed = 10f;
    public float rotationSpeed = 250f;
    public float gravity = -9.81f;

    public float health { get; set; }
    public float maxHealth = 100f;
    public UnityAction onDeath, onAttack;

    public Weapons weaponsList;
    public Weapon currentWeapon { get; private set; }
    [SerializeField] AudioSource audioSource;
    public float parryCooldown, parryCooldownTimer;

    Vector3 horizontalVelocity, verticalVelocity;

    AttackBox attackBox;

    bool moveCalledThisFrame = false;
    bool stunned;

    bool _parrying;
    bool isParrying { get { return _parrying; } set { _parrying = value; animator.SetBool("Parrying", value); } }

    [SerializeField] float stunTime = 2f;
    [SerializeField] RotatingObject stunIcon;
    float attackTimer, parryTimer, stunTimer;

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

        if (isParrying)
        {
            if (parryTimer < currentWeapon.parryDuration)
            {
                parryTimer += Time.deltaTime;
            }
            else
            {
                isParrying = false;
                //parryTimer = 0f;
                //parryCooldownTimer = 0f;
            }
        }
        else
        {
            parryCooldownTimer += Time.deltaTime;
        }

        if (stunned)
        {
            if (stunTimer < stunTime)
            {
                stunTimer += Time.deltaTime;
            }
            else
            {
                stunned = false;
                stunIcon.gameObject.SetActive(false);
                stunTimer = 0f;
            }
        }
        

        if (health <= 0)
        {
            OnDeath();
        }

        if (!moveCalledThisFrame) { horizontalVelocity = Vector3.zero; animator.SetBool("Running", false); }
        moveCalledThisFrame = false;
    }

    public void Move(float input)
    {
        if (!isParrying)
        {
            horizontalVelocity = transform.forward * input * runningSpeed * Time.deltaTime;
            animator.SetBool("Running", input > 0.1f);
        }
        else
        {
            horizontalVelocity = Vector3.zero;
            animator.SetBool("Running", false);
        }

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

    public bool Attack()
    {
        if (stunned || attackTimer < currentWeapon.attackCooldown || attackBox.enemies.Count == 0) return false;

        for (int i = attackBox.enemies.Count - 1; i >= 0; i--)
        {
            BotController enemy = attackBox.enemies[i];

            if (enemy == null)
            {
                //Debug.Log("Enemy is already dead, removing from list");
                attackBox.enemies.Remove(attackBox.enemies[i]);

                if (attackBox.enemies.Count == 0) return false;
                else continue;
            }

            if (enemy.isParrying)
            {
                stunned = true;
                stunIcon.gameObject.SetActive(true);
                return false;
            }
            else
            {
                enemy.health -= currentWeapon.damagePerHit;
            }
        }

        attackTimer = 0f;
        onAttack?.Invoke();
        audioSource.PlayOneShot(currentWeapon.attackSoundClips[Random.Range(0, currentWeapon.attackSoundClips.Length)]);
        return true;
    }

    public void Parry()
    {
        if (parryCooldownTimer >= parryCooldown && !isParrying)
        {
            isParrying = true;
            parryTimer = parryCooldownTimer = 0f; 
        }
    }

    public void OnDeath()
    {
        onDeath?.Invoke();

        Destroy(gameObject);
    }
}