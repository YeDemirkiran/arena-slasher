using UnityEngine;
using UnityEngine.Events;

public class BotController : MonoBehaviour
{
    CharacterController controller;
    BotOutfit outfit;

    [SerializeField] Animator animator;

    public float runningSpeed = 10f;
    public float rotationSpeed = 250f;
    public float gravity = -9.81f;

    public float health { get; set; }
    public bool destroyOnDeath { get; set; } = true;
    public float maxHealth = 100f;
    public UnityAction onDeath, onAttack;

    public Weapons weaponsList;
    public Weapon currentWeapon { get; set; }
    [SerializeField] AudioSource audioSource;
    public float parryCooldown;
    [HideInInspector] public float parryCooldownTimer;

    Vector3 horizontalVelocity, verticalVelocity;

    AttackBox attackBox;

    bool moveCalledThisFrame = false;

    bool _stunned;
    public bool stunned { get { return _stunned; } private set { _stunned = value; stunIcon.gameObject.SetActive(value); animator.SetBool("Stunned", value); } }


    bool _parrying;
    bool isParrying { get { return _parrying; } set { _parrying = value; animator.SetBool("Parrying", value); } }

    [SerializeField] float stunTime = 2f;
    [SerializeField] RotatingObject stunIcon;
    [SerializeField] GameObject bloodParticles;
    float attackTimer, parryTimer, stunTimer;

    [SerializeField] AudioClip[] warriorScreams;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        outfit = GetComponent<BotOutfit>();
        attackBox = GetComponentInChildren<AttackBox>();
    }

    void Start()
    {
        health = maxHealth;
        currentWeapon = weaponsList.weapons[0];
    }

    void Update()
    {
        if (GameManager.Instance.state != GameManager.GameState.Running) { return; }

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
            attackTimer = 0f;
            parryTimer = 0f;
            isParrying = false;

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
        else if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (!moveCalledThisFrame) { horizontalVelocity = Vector3.zero; animator.SetBool("Running", false); }
        moveCalledThisFrame = false;

        //Debug.Log("outfit null: " + outfit == null);
        //Debug.Log("animator null: " + animator == null);

        //outfit?.UpdateOutfit(animator);
    }

    public void Move(float input)
    {
        if (!isParrying && !stunned && verticalVelocity.y >= -0.1f) // Since vertical velocity is negative
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
        if (stunned) return;

        transform.eulerAngles += Vector3.up * inputX * rotationSpeed * Time.deltaTime;
    }

    public void ApplyGravity()
    {
        if (controller.isGrounded) verticalVelocity.y = 0f;
        else verticalVelocity.y += gravity * Time.deltaTime * Time.deltaTime;

        animator.SetBool("Falling", verticalVelocity.y < -0.05f);

        if (verticalVelocity.y < -0.05f && !audioSource.isPlaying && warriorScreams.Length > 0)
        {
            audioSource.PlayOneShot(warriorScreams[Random.Range(0, warriorScreams.Length)]);
        }
    }

    public void ApplyMovement()
    {
        controller.Move(horizontalVelocity + verticalVelocity);
    }

    public bool Attack()
    {
        if (stunned || attackTimer < currentWeapon.attackCooldown) return false;

        isParrying = false;
        animator.SetTrigger("Slash");
        attackTimer = 0f;
        audioSource.PlayOneShot(currentWeapon.attackSoundClips[Random.Range(0, currentWeapon.attackSoundClips.Length)]);

        if (attackBox.enemies.Count == 0) return false;

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
                return false;
            }
            else
            {
                enemy.health -= currentWeapon.damagePerHit;
                GameObject blood = Instantiate(bloodParticles, enemy.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 2f), Random.Range(-1f, 1f)), Random.rotation);
            }
        }
        
        onAttack?.Invoke();
        audioSource.PlayOneShot(currentWeapon.hitSoundClips[Random.Range(0, currentWeapon.attackSoundClips.Length)]);

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

        if (destroyOnDeath) Destroy(gameObject);      
    }

    public void ResetBot()
    {
        health = maxHealth;
        isParrying = stunned = moveCalledThisFrame = false;
        parryCooldownTimer = parryCooldown;
        parryTimer = attackTimer = stunTimer = 0f;
        horizontalVelocity = verticalVelocity = Vector3.zero;
    }
}