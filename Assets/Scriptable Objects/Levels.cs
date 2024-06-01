using UnityEngine;

[System.Serializable]
public class Level
{
    public int id;
    public LevelType type = LevelType.Timed;
    public int difficultyID;
    public Vector3 spawnPoint;
    public Vector2 spawnArea;
    public float spawnTime;
    public int enemyPerSpawn;
    public int[] enemyIDs;

    public enum LevelType { Timed, Survival }
}

[CreateAssetMenu(fileName = "Levels", menuName = "Scriptable Objects/Levels")]
public class Levels : ScriptableObject
{
    public Level[] levels;
}