using UnityEngine;
using UnityEngine.UI;

public class ParryBar : MonoBehaviour
{
    public BotController controller;
    [SerializeField] Image bar;

    void Update()
    {
        if (controller == null && GameManager.Instance.state != GameManager.GameState.Running) { return; }

        bar.fillAmount = Mathf.Clamp01(controller.parryCooldownTimer / controller.parryCooldown);

        //if (controller == null)
        //{
        //    if (PlayerController.Instance == null)
        //    {
        //        return;
        //    }

        //    controller = PlayerController.Instance.controller;
        //}
    }
}