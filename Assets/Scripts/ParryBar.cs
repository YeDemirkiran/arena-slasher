using UnityEngine;
using UnityEngine.UI;

public class ParryBar : MonoBehaviour
{
    [SerializeField] BotController controller;
    [SerializeField] Image bar;

    void Update()
    {
        if (GameManager.Instance.state != GameManager.GameState.Running) { return; }

        bar.fillAmount = Mathf.Clamp01(controller.parryCooldownTimer / controller.parryCooldown);
    }
}