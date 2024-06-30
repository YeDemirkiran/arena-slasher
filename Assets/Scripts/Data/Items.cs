using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ItemType
{
    public int id;
    public string name;
}

[Serializable]
public abstract class Item
{
    public int id;

    public bool excludeFromStore;

    public int typeID;
    public ItemType type { get { return Items.Instance.types.First(x => x.id == typeID); } }

    public string name;
    [TextArea] public string description;

    public int arenaID;
    public int price;

    public GameObject prefab;
    public Sprite icon;


    public bool Locked()
    {
        foreach (var item in GameManager.Instance.gameData.unlockedLevelIDs)
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
}

[Serializable]
public class Gear : Item
{

}

[Serializable]
public class Weapon : Item
{
    public RuntimeAnimatorController controller;
    public AudioClip[] attackSoundClips;
    public AudioClip[] hitSoundClips;

    public float damagePerHit;
    public float attackCooldown;
    public float parryDuration;
}

[System.Serializable]
public class ListWithID<T> where T : Item
{
    public List<T> list;

    public int Count { get { return list.Count; } }

    public T this[int id]
    {
        get
        {
            return list.FirstOrDefault(x => x.id == id);
        }
    }
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

    public List<ItemType> types;
    public ListWithID<Gear> gears;
    public ListWithID<Weapon> weapons;

    public Item this[int i]
    {
        get
        {
            Item item = gears[i];

            if (item == null)
            {
                item = weapons[i];
            }

            return item;
        }
    }

    public int Length
    {
        get { return gears.Count + weapons.Count; }
    }
}