using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public int levelID {  get; set; }
    public int difficultyID {  get; set; }

    public Button button { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => GameManager.Instance.CreateSession(levelID, difficultyID));
    }
}