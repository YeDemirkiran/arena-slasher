using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[System.Serializable]
public class Prop
{
    public GameObject[] prefabs;
    public int count;
    public Vector3 sizeA, sizeB;
}

[System.Serializable]
public class Level
{
    public int id;
    public string name, description;
    public Sprite thumbnail;
    public bool playable;
    public AudioClip soundtrack;

    public LevelType type = LevelType.Timed;
    

    public Vector3 center;
    public Vector2 arenaArea;

    public GameObject ground, column, wall;
    public Material groundMaterial, columnMaterial, wallMaterial;

    public Prop[] props;
    public VolumeProfile volume;

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
    public static Levels instance;
    public Level[] levels;
}