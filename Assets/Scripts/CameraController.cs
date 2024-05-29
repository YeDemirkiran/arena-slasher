using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    public float sensitivity = 100f;

    Coroutine currentShake;

    void Update()
    {
        transform.eulerAngles += Vector3.up * sensitivity * Input.GetAxis("Mouse X") * Time.deltaTime;
    }

    public void Shake(float duration, float magnitude, float frequency)
    {
        if (currentShake != null) StopCoroutine(currentShake);

        currentShake = StartCoroutine(ShakeIE(duration, magnitude, frequency));
    }

    IEnumerator ShakeIE(float duration, float magnitude, float frequency)
    {
        float timer = 0f;

        CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = magnitude;
        perlin.m_FrequencyGain = frequency;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        perlin.m_AmplitudeGain = 0f;
        perlin.m_FrequencyGain = 0f;

        currentShake = null;
    }
}