using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    Button button;
    LevelManager levelManager;

    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
        levelManager = FindObjectOfType<LevelManager>();

        button.onClick.AddListener(() => { levelManager.RestartLevel(); GameManager.Instance.ContinueGame(); });
    }
}