using System.Linq;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public string name;
    [TextArea] public string description;
    public bool locked, purchased;
    public GameObject prefab;

    public enum ItemType { Helmet, Armor, Pants, Boots }
}

[CreateAssetMenu(fileName = "Items", menuName = "Scriptable Objects/Items")]
public class Items : ScriptableObject
{
    public Item[] items;

    public Item this[int i]
    {
        get { return items.First(x => x.id == i); }
        set { items[i] = value; }
    }
}