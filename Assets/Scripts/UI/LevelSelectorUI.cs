using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class LevelSelectorUI : MonoBehaviour
{
    [SerializeField] Levels levels;
    [SerializeField] GameObject levelInfoPrefab;
    [SerializeField] Transform contentTransform;
    [SerializeField] TMP_Text infoLevelName, infoLevelDescription;
    [SerializeField] RectTransform selectedOutline;
    [SerializeField] PlayButton playButton;
    [SerializeField] GameObject difficultySelector, lockedMessage;

    LevelInfo _level;
    public LevelInfo selectedLevel 
    { 
        get 
        { 
            return _level;
        }
        set 
        {
            _level = value;

            if (value != null)
            {
                playButton.button.interactable = value.playable && value.unlocked;
                difficultySelector.SetActive(value.playable && value.unlocked);
                lockedMessage.SetActive(value.playable && !value.unlocked);
                playButton.levelID = value.id;
                
                infoLevelName.text = value._levelName;
                infoLevelDescription.text = value.levelDescription;

                selectedOutline.gameObject.SetActive(true);
                selectedOutline.SetParent(selectedLevel.transform.parent);
                selectedOutline.SetAsFirstSibling();
                selectedOutline.transform.position = selectedLevel.transform.position;
            }
            else
            {
                selectedOutline.gameObject.SetActive(false);
                playButton.button.interactable = false;
            }
        } 
    }

    IEnumerator Start()
    {
        GenerateSelector();
        yield return null;
        selectedLevel = GetComponentInChildren<LevelInfo>();
    }

    void GenerateSelector()
    {
        foreach (Transform item in contentTransform)
        {
            if (item != contentTransform)
            {
                Destroy(item.gameObject);
            }
        }

        foreach (Level level in levels.levels)
        {
            LevelInfo info = Instantiate(levelInfoPrefab, contentTransform).GetComponent<LevelInfo>();
            info.Initialize(level);
            info.button.onClick.AddListener(() => selectedLevel = info);
        }
    }

    private void Update()
    {
        //selectedOutline.position = selectedLevel.transform.position;
    }

    void OnDisable()
    {
        //selectedLevel = null;
    }
}