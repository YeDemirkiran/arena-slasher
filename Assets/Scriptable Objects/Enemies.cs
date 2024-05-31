using UnityEngine;

[System.Serializable]
public class Enemy
{
    public int id;
    public GameObject prefab;
    public int weaponID;
    public GameObject headGear;
    public GameObject torsoGear;
    public GameObject pantsGear;
    public GameObject feetGear;
}

[CreateAssetMenu(fileName = "Enemies", menuName = "Scriptable Objects/Enemies")]
public class Enemies : ScriptableObject
{
    public Enemy[] enemies;
}