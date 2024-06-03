using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    IEnumerator Start()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }

        GameManager.Instance.onMainMenu += () => Destroy(gameObject);
    }
}