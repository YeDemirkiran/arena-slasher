using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] PlayerController player;
    Image bar;

    void Awake()
    {
        bar = GetComponent<Image>();
    }

    void Update()
    {
        bar.fillAmount = player.health / player.maxHealth;
    }
}