using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] Vector2 yClamp;

    public Vector2 sensitivity;

    Coroutine currentShake;

    Vector3 euler;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        euler = transform.localEulerAngles;
    }

    void Update()
    {
        euler.y += sensitivity.x * Input.GetAxis("Mouse X") * Time.deltaTime;
        euler.x += sensitivity.y * Input.GetAxis("Mouse Y") * Time.deltaTime;

        euler.x = Mathf.Clamp(euler.x, yClamp.x, yClamp.y);

        transform.localEulerAngles = euler;
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