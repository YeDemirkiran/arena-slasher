using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] BotController controller;
    [SerializeField] Image bar;

    void Update()
    {
        bar.fillAmount = controller.health / controller.maxHealth;
    }
}