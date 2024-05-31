using UnityEngine;

[System.Serializable]
public class Difficulty
{
    public int id;
    public string name;
    [TextArea] public string description;
    public float duration;
    public int maxEnemies;
}

[CreateAssetMenu(fileName = "Difficulty Levels", menuName = "Scriptable Objects/Difficulty Levels")]
public class DifficultyLevels : ScriptableObject
{
    public Difficulty[] difficultyLevels;
}