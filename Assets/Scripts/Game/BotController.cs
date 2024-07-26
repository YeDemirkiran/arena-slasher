using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BotController : MonoBehaviour
{
    CharacterController controller;

    #region GENERAL
    [Header("General")]
    public Animator animator;
    [SerializeField] AudioSource audioSource;

    public float runningSpeed = 10f;
    public float rotationSpeed = 250f;
    public float gravity = -9.81f;

    [SerializeField] float stunTime = 2f;
    public RotatingObject stunIcon;

    Vector3 horizontalVelocity, verticalVelocity;
    bool moveCalledThisFrame = false;

    bool _stunned;
    public bool stunned { get { return _stunned; } private set { _stunned = value; stunIcon.gameObject.SetActive(value); animator.SetBool("Stunned", value); } }

    #endregion

    #region HEALTH
    public float health { get; set; }
    public bool destroyOnDeath { get; set; } = true;

    [Header("Health")]
    public float maxHealth = 100f;
    public UnityAction onDeath, onAttack;
    #endregion

    #region COMBAT
    WeaponController[] _weaponControllers;
    public WeaponController[] weaponControllers
    {
        get
        {
            return _weaponControllers;
        }
        set
        {
            _weaponControllers = value;

            foreach (var item in value)
            {
                item.controller = this;
                item.targetTag = targetTag;
            }
        }
    }

    float attackTimer, parryTimer, stunTimer;

    public Weapon currentWeapon { get; set; }

    [Header("Combat")]
    [SerializeField] string targetTag;
    public float parryCooldown;
    [HideInInspector] public float parryCooldownTimer;

    [SerializeField] int maxAttacksInQueue = 3;
    [SerializeField] float queueAttackCooldown;
    float queueAttackTimer;
    bool queueTimerOnSet = false;

    int _attacksInQueue;
    int currentAttacksInQueue 
    { 
        get 
        { 
            return _attacksInQueue; 
        } 
        set 
        {
            _attacksInQueue = Mathf.Clamp(value, 0, maxAttacksInQueue);
        } 
    }

    Coroutine currentAttackCoroutine;

    bool _parrying;
    bool isParrying { get { return _parrying; } set { _parrying = value; animator.SetBool("Parrying", value); } }
    #endregion 

    #region VFX AND SFX
    [Header("VFX & SFX")]
    [SerializeField] GameObject bloodParticles;
    [SerializeField] AudioClip[] warriorScreams, attackGrunts;
    #endregion


    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        health = maxHealth;

        if (currentAttackCoroutine == null)
        {
            currentAttackCoroutine = StartCoroutine(AttackIE());
        }
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
        if (stunned) return false;

        //Debug.Log("Before: " + currentAttacksInQueue);
        currentAttacksInQueue++;
        //Debug.Log("After: " + currentAttacksInQueue);

        return true;
    }

    IEnumerator AttackIE()
    {
        int performedAttacks = 0;

        while (true)
        {
            if (queueTimerOnSet)
            {
                if (queueAttackTimer < queueAttackCooldown)
                {
                    queueAttackTimer += Time.deltaTime;
                }
                else
                {
                    queueTimerOnSet = false;
                    currentAttacksInQueue = 0;
                    performedAttacks = 0;
                    queueAttackTimer = 0f;
                }
            }
            else
            {
                if ((currentAttacksInQueue > 0) && !GetWeaponAttack())
                {
                    SetWeaponAttack(false);
                    isParrying = false;
                    animator.SetTrigger("Slash");
                    audioSource.PlayOneShot(currentWeapon.attackSoundClips[Random.Range(0, currentWeapon.attackSoundClips.Length)]);

                    performedAttacks++;
                    currentAttacksInQueue--;

                    if (performedAttacks >= maxAttacksInQueue)
                    {
                        queueTimerOnSet = true;  
                    }
                }
            }

            yield return null;
        }
    }

    public void GiveDamage(BotController enemy, WeaponController caller, Vector3 attackPoint)
    {
        if (enemy.isParrying)
        {
            stunned = true;
            return;
        }
        else
        {
            enemy.health -= currentWeapon.damagePerHit;
            GameObject blood = Instantiate(bloodParticles, attackPoint, Quaternion.identity);
            blood.transform.parent = caller.transform;
            caller.particles.Add(blood);
        }

        onAttack?.Invoke();
        audioSource.PlayOneShot(currentWeapon.hitSoundClips[Random.Range(0, currentWeapon.hitSoundClips.Length)]);
        audioSource.PlayOneShot(attackGrunts[Random.Range(0, attackGrunts.Length)]);
    }

    public void SetWeaponAttack(bool value)
    {
        foreach (var item in weaponControllers)
        {
            item.isAttacking = value;
        }
    }

    public bool GetWeaponAttack()
    {
        return weaponControllers[0].isAttacking;
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