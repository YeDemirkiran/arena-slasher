using System;
using UnityEngine;

[Serializable]
public class Weapon
{
    public int id;
    public string name;
    public float damagePerHit;
    public float attackCooldown;
    public float parryDuration;
    public GameObject prefab;
    public AudioClip[] attackSoundClips;
    public AudioClip[] hitSoundClips;
}

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptable Objects/Weapons")]
public class Weapons : ScriptableObject
{
    public Weapon[] weapons;
}
