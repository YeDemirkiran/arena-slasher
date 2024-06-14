using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public BotController controller { get; set; }
    public string targetTag { get; set; }

    bool _isAttacking;
    public bool isAttacking
    {
        get
        {
            return _isAttacking;
        }

        set
        {
            _isAttacking = value;
            hitEnemies.Clear();
        }
    }

    List<BotController> hitEnemies = new List<BotController>();

    private void OnTriggerEnter(Collider other)
    {
        if (controller == null)
        {
            Debug.Log("Controller is null");
            return;
        }
        else
        {
            Debug.Log("Controller is not null");
        }

        if (!isAttacking)
        {
            Debug.Log("Not attacking");
            return;
        }
        else
        {
            Debug.Log("Attacking");
        }

        if (!other.TryGetComponent(out BotController enemyController)) { Debug.Log("No Enemy controller"); return; }

        if (!hitEnemies.Contains(enemyController) && enemyController != controller && other.CompareTag(targetTag))
        {
            hitEnemies.Add(enemyController);
            controller.GiveDamage(other.GetComponent<BotController>());
            Debug.Log("Hit");
        }
    }
}