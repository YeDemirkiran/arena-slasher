using UnityEngine;

public class AttackAnimationEvent : MonoBehaviour
{
    [SerializeField] BotController controller;

    public void SetAttackMode(AnimationEvent myEvent)
    {
        controller.SetWeaponAttack(myEvent.intParameter == 1 ? true : false);
    }
}