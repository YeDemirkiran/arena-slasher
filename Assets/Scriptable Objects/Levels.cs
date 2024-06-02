using UnityEngine;

[System.Serializable]
public class Level
{
    public int id;
    public LevelType type = LevelType.Timed;
    public int difficultyID;
    public Vector3 center;
    public Vector2 arenaArea;

    public GameObject ground, column, wall;
    public Material groundMaterial, columnMaterial, wallMaterial;

    public Vector3 spawnPoint;
    public Vector2 spawnArea;
    public float spawnTime;
    public int enemyPerSpawn;
    public int[] enemyIDs;
    public enum LevelType { Timed, Survival }
    public float duration;
}

[CreateAssetMenu(fileName = "Levels", menuName = "Scriptable Objects/Levels")]
public class Levels : ScriptableObject
{
    public Level[] levels;
}