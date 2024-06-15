using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public BotController controller;
    [SerializeField] Image bar;

    void Update()
    {
        if (controller == null && GameManager.Instance.state != GameManager.GameState.Running) { return; }

        //if (controller == null)
        //{
        //    if (PlayerController.Instance == null)
        //    {
        //        return;
        //    }

        //    controller = PlayerController.Instance.controller;
        //}

        bar.fillAmount = Mathf.Clamp01(controller.health / controller.maxHealth);
    }
}