using UnityEngine;
using UnityEngine.UI;

public class ParryBar : MonoBehaviour
{
    [SerializeField] BotController controller;
    [SerializeField] Image bar;

    void Update()
    {
        bar.fillAmount = Mathf.Clamp01(controller.parryCooldownTimer / controller.parryCooldown);
    }
}