using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    // Start is called before the first frame update
    void Awake()
    {
        GameManager.Instance.onPause += () => pauseMenu.SetActive(true);
        GameManager.Instance.onResume += () => pauseMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }
}