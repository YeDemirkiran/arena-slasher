using UnityEngine;

[System.Serializable]
public class EntityStyle
{
    public GameObject baseModel;
    public Transform headGearPosition;
    public Transform torsoGearPosition;
    public Transform pantsGearPosition;
    public Transform feetGearPosition;
    public Transform weaponPosition;
}

[System.Serializable]
public class Enemy
{
    public int id;
    public EntityStyle style;
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