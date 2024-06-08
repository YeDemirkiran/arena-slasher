using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField] DifficultyLevels difficultyLevels;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TMP_Text description;
    [SerializeField] PlayButton playButton;
    public int difficultyId {  get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        dropdown.ClearOptions();

        List<string> names = new List<string>();
        foreach (var difficulty in difficultyLevels.difficultyLevels)
        {
            if (!difficulty.excludeFromGame)
            {
                names.Add(difficulty.name);
            } 
        }

        dropdown.AddOptions(names);

        dropdown.onValueChanged.AddListener(x => 
        { 
            difficultyId = GetDifficultyIdByName(dropdown.options[dropdown.value].text);
            
            description.text = difficultyLevels.difficultyLevels.First(x => x.id == difficultyId).description; 
            playButton.difficultyID = difficultyId;
        });

        dropdown.value = -1;
    }

    void Start()
    {
        dropdown.value = 0;
    }

    // Update is called once per frame
    int GetDifficultyIdByName(string difficultyName)
    {
        foreach (var item in difficultyLevels.difficultyLevels)
        {
            if (item.name == difficultyName)
            {
                return item.id;
            }
        }

        return -1;
    }
}