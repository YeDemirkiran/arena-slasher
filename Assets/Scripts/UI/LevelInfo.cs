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
    public bool unlocked { get; private set; }

    public void Initialize(Level level)
    {
        button = GetComponent<Button>();
        id = level.id;

        playable = level.playable;
        unlocked = GameManager.Instance.gameData.CheckLevelStatus(id);
        
        levelName.text = _levelName = playable ? level.name : "Coming Soon";
        levelDescription = playable ? level.description : "Stay tuned for the updates!";
        imageSlot.sprite = playable ? level.thumbnail : null;

        comingSoonMarker.SetActive(!playable);
        lockedMarker.SetActive(playable && !unlocked);    
    }
}