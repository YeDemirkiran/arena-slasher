using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] BotController controller;
    [SerializeField] Image bar;

    void Update()
    {
        bar.fillAmount = controller.health / controller.maxHealth;
    }
}