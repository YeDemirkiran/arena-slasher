using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    PlayerController player;

    public BotController controller { get; private set; }

    [SerializeField] float followSafeDistance = 3f;
    [SerializeField] float parryChance = 10f;
    [SerializeField] Vector2 actionTimeMinMax;

    [SerializeField] int maxCombo = 5;
    [SerializeField] AnimationCurve comboChanceDecrease;

    float currentActionTime = 0f, actionTimer = -1f;
    int currentHit = 0;
    bool comboAllowed = false;

    public LevelManager level { get; set; }
    public Drop[] drops { get; set; }

    void Awake()
    {
        controller = GetComponent<BotController>();
    }

    void Start()
    {
        player = PlayerController.Instance;
        controller.onDeath += () => level?.EnemyDeathReport(this);
        controller.onDeath += () => Drop();
    }

    void Update()
    {
        if (GameManager.Instance.state != GameManager.GameState.Running || controller.stunned) { return; }

        RotateTowards(player.transform);

        if (Vector3.Distance(player.transform.position, transform.position) > followSafeDistance)
        {
            controller.Move(1f);

            actionTimer = -1f;
        }
        else
        {
            

            if (actionTimer == -1f)
            {
                currentActionTime = Random.Range(actionTimeMinMax.x, actionTimeMinMax.y);
                //actionTimer = currentActionTime;
                actionTimer = 0f;
            }

            if (!comboAllowed && actionTimer < currentActionTime)
            {
                actionTimer += Time.deltaTime;
            }
            else
            {
                // Parry
                float parry = Random.Range(0f, 100f);

                if (parry <= parryChance)
                {
                    controller.Parry();
                }
                else
                {
                    if (controller.Attack())
                    {
                        if (currentHit < maxCombo)
                        {
                            currentHit++;

                            float comboProgressChance = Random.Range(0f, 100f);
                            float eval = (comboChanceDecrease.Evaluate((float)(currentHit - 1) / (float)maxCombo)) * 100f;

                            //Debug.Log(eval);
                            //Debug.Log(currentHit);

                            if (comboProgressChance < eval)
                            {
                                comboAllowed = true;
                            }
                            else
                            {
                                actionTimer = 0f;
                                comboAllowed = false;
                                currentHit = 0;
                            }
                        }
                    }
                }           
            }    
        }
    }

    public void RotateTowards(Transform target)
    {
        Vector3 playerPos = target.position;

        Vector3 dir = playerPos - transform.position;
        float singleStep = controller.rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, dir, singleStep, 0f));
    }

    public void Drop()
    {
        float chance = Random.Range(0f, 100f);
        Drop selectedDrop = drops[Random.Range(0, drops.Length)];

        if (chance < selectedDrop.rarity)
        {
            GameObject drop = Instantiate(selectedDrop.prefab, transform.position, Quaternion.identity);

            if (drop.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(-transform.forward * 10f, ForceMode.Impulse);
            }
        }
    }
}