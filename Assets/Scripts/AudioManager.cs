using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource;
    GameManager gameManager;

    [SerializeField] AudioMixer audioMixer;

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

        gameManager = GameManager.Instance;

        gameManager.onMainMenu += () => Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        audioMixer.SetFloat("SFX Volume", Mathf.Log10(gameManager.gameData.sfxLevel) * 20f);
        audioMixer.SetFloat("Music Volume", Mathf.Log10(gameManager.gameData.musicLevel) * 20f);
    }
}