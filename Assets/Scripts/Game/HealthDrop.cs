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
        if (other.CompareTag("Player"))
        {
            BotController bot = other.GetComponent<BotController>();
            bot.health += bot.maxHealth * healPercentage;
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            Destroy(gameObject);
        }
    }
}