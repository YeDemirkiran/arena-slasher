using System.Linq;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public bool excludeFromStore;
    public ItemType type;
    public string name;
    [TextArea] public string description;
    public int price;
    public int arenaID;
    
    public GameObject prefab;
    public Sprite banner;

    public bool Locked(GameData data)
    {
        foreach (var item in data.unlockedLevelIDs)
        {
            if (item == arenaID)
            {
                return false;
            }
        }

        return true;
    }

    public bool Purchased(GameData data)
    {
        foreach (var item in data.boughtItemIDs)
        {
            if (item == id)
            {
                return true;
            }
        }

        return false;
    }

    public enum ItemType { Helmet, Armor, Pants, Boots }
}

[CreateAssetMenu(fileName = "Items", menuName = "Scriptable Objects/Items")]
public class Items : ScriptableObject
{
    static Items _instance;

    public static Items Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<Items>("Items");
            }

            return _instance;
        }
    }

    public Item[] items;

    public Item this[int i]
    {
        get { return items.First(x => x.id == i); }
        set { items[i] = value; }
    }
}