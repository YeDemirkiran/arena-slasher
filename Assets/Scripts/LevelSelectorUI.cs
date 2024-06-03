using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorUI : MonoBehaviour
{
    [SerializeField] TMP_Text infoLevelName, infoLevelDescription;
    [SerializeField] RectTransform selectedOutline;
    [SerializeField] PlayButton playButton;

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
                selectedOutline.gameObject.SetActive(true);
                selectedOutline.position = value.transform.position;

                playButton.button.interactable = value.playable;
                playButton.levelID = value.level.id;
                
                infoLevelName.text = value._levelName;
                infoLevelDescription.text = value.levelDescription;
            }
            else
            {
                selectedOutline.gameObject.SetActive(false);
                playButton.button.interactable = false;
            }
        } 
    }

    void Start()
    {
        selectedLevel = null;
    }

    void OnDisable()
    {
        selectedLevel = null;
    }
}