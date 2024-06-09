using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelInfo : MonoBehaviour
{
    [SerializeField] TMP_Text levelName;
    [SerializeField] Image imageSlot;
    [SerializeField] GameObject lockedMarker, comingSoonMarker;
    [SerializeField] Levels levelsObject;

    public Button button {  get; set; }
    public Level level {  get; private set; }
    public int id { get; private set; }
    public string _levelName {  get; private set; }
    public string levelDescription {  get; private set; }
    public bool playable { get; private set; }

    public void Initialize(Level level)
    {
        button = GetComponent<Button>();

        if (!level.playable)
        {
            levelName.text = "Coming Soon";
            _levelName = "Coming Soon!";
            levelDescription = "Stay tuned for the updates!";
            playable = false;
            comingSoonMarker.SetActive(true);
            return;
        }

        comingSoonMarker.SetActive(false);

        id = level.id;
        levelName.text = _levelName = level.name;
        levelDescription = level.description;
        imageSlot.sprite = level.thumbnail;

        playable = level.playable && GameManager.Instance.gameData.CheckLevelStatus(id);
        lockedMarker.SetActive(!playable);
    }
}