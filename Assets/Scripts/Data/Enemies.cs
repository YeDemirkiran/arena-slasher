using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public int id;
    public GameObject prefab;
    public int weaponID;

    [SerializeField] int[] gearIDs;

    public Item[] gears
    {
        get
        {
            List<Item> _gears = new List<Item>();

            foreach (var id in gearIDs)
            {
                foreach (var item in Items.Instance.items)
                {
                    if (item.id == id)
                    {
                        Debug.Log("1: " + item.id);
                        Debug.Log("2: " + id);
                        _gears.Add(item);
                    }
                }
            }

            return _gears.ToArray();
        }
    }

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