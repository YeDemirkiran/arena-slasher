using System;
using UnityEngine;
using System.Linq;

[Serializable]
public class Weapon
{
    public int id;
    public string name;

    public RuntimeAnimatorController controller;
    public GameObject prefab;
    public AudioClip[] attackSoundClips;
    public AudioClip[] hitSoundClips;

    public float damagePerHit;
    public float attackCooldown;
    public float parryDuration;
    
}

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptable Objects/Weapons")]
public class Weapons : ScriptableObject
{
    static Weapons instance;
    public static Weapons Instance 
    { 
        get 
        { 
            if (instance == null)
            {
                instance = Resources.Load<Weapons>("Weapons");
            }

            return instance;
        } 
    }

    public Weapon[] weapons;

    public Weapon this[int i]
    {
        get
        {
            return weapons.First(x => x.id == i);
        }

        set 
        {
            weapons[i] = value;
        }
    }
}