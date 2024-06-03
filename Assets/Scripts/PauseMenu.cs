using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.Instance.onPause += () => gameObject.SetActive(true);
        GameManager.Instance.onResume += () => gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}