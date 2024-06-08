using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] float healPercentage = 0.25f;
    [SerializeField] AudioClip pickupSound;
    public Rigidbody rb { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("drop test 2");
            BotController bot = other.GetComponent<BotController>();
            bot.health += bot.maxHealth * healPercentage;
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("drop test 3");
        }
    }
}