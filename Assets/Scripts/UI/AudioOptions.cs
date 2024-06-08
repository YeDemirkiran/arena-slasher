using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    [SerializeField] Slider sfxSlider, musicSlider;

    GameManager gameManager;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (GameManager.Instance == null) { yield return null; }
        gameManager = GameManager.Instance;
        sfxSlider.value = gameManager.gameData.sfxLevel;
        musicSlider.value = gameManager.gameData.musicLevel;

        sfxSlider.onValueChanged.AddListener(x => gameManager.gameData.sfxLevel = sfxSlider.value);
        musicSlider.onValueChanged.AddListener(y => gameManager.gameData.musicLevel = musicSlider.value);
    }
}