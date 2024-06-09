using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] int levelId;
    [SerializeField] TMP_Text levelName;
    [SerializeField] Image imageSlot;
    [SerializeField] GameObject lockedMarker, comingSoonMarker;
    [SerializeField] Levels levelsObject;

    public Level level {  get; private set; }
    public string _levelName {  get; private set; }
    public string levelDescription {  get; private set; }
    public bool playable { get; private set; }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        try
        {
            level = levelsObject.levels.First(x => x.id == levelId);
        }
        catch (System.InvalidOperationException)
        {
            levelName.text = "Coming Soon";
            _levelName = "Coming Soon!";
            levelDescription = "Stay tuned for the updates!";
            playable = false;
            comingSoonMarker.SetActive(true);
            yield break;
            throw;
        }

        levelName.text = _levelName = level.name;
        levelDescription = level.description;
        imageSlot.sprite = level.thumbnail;

        playable = level.playable;
        lockedMarker.SetActive(!playable);

        comingSoonMarker.SetActive(false);
    }
}