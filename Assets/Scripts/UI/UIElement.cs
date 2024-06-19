using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIElement : MonoBehaviour
{
    public float appearDuration = 0.1f;

    CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        canvasGroup.alpha = 0f;

        StopAllCoroutines();
        StartCoroutine(FadeUI(1f, appearDuration, true));
    }

    public void Disable()
    {
        StopAllCoroutines();
        StartCoroutine(FadeUI(0f, appearDuration, false));
    }

    IEnumerator FadeUI(float targetAlpha, float duration, bool endState)
    {
        canvasGroup.blocksRaycasts = false;
        float timer = 0f;
        float startAlpha = canvasGroup.alpha;

        while (timer < 1f) 
        { 
            timer += Time.unscaledDeltaTime / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer);
            yield return null;
        }

        canvasGroup.blocksRaycasts = true;
        gameObject.SetActive(endState);
    }
}