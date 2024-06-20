using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public int id;
    public GameObject prefab;

    [SerializeField] int[] gearIDs;

    public Item[] items
    {
        get
        {
            List<Item> _gears = new List<Item>();

            foreach (var id in gearIDs)
            {
                foreach (var item in Items.Instance.gears)
                {
                    if (item.id == id)
                    {
                        _gears.Add(item);
                    }
                }
            }

            return _gears.ToArray();
        }
    }

    [SerializeField] int weaponID;

    public Weapon weapon
    {
        get
        {
            return Items.Instance.weapons[weaponID];
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