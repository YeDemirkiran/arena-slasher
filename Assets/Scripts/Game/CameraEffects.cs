using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    Coroutine currentMovement;

    public void MoveToTransformUI(Transform target)
    {
        MoveToTransform(target, 0.5f, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f));
    }

    public void MoveToTransform(Transform target, float duration, AnimationCurve easeType, bool unscaled = true)
    {
        if (currentMovement != null) StopCoroutine(currentMovement);
        
        currentMovement = StartCoroutine(MoveIE(target, duration, easeType, unscaled));
    }

    IEnumerator MoveIE(Transform target, float duration, AnimationCurve easeType, bool unscaled)
    {
        float timer = 0f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (timer < 1f)
        {
            timer += (unscaled ? Time.unscaledDeltaTime : Time.deltaTime) / duration;

            transform.position = Vector3.Lerp(startPos, target.position, easeType.Evaluate(timer));
            transform.rotation = Quaternion.Slerp(startRot, target.rotation, easeType.Evaluate(timer));

            yield return null;
        }

        // To make sure

        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
