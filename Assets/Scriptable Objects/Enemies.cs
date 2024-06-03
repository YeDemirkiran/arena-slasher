using System.Collections.Generic;
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
    public GameObject[] additionalGear;
    public Drop[] drops;
}

[System.Serializable]
public class Drop
{
    public float rarity;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "Enemies", menuName = "Scriptable Objects/Enemies")]
public class Enemies : ScriptableObject
{
    public Enemy[] enemies;
}