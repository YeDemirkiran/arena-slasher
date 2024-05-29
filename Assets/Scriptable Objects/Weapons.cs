using System;
using UnityEngine;

[Serializable]
public class Weapon
{
    public string name;
    public float damagePerHit;
    public float cooldown;
    public GameObject mesh;
    public AudioClip[] attackSoundClips;
}

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptable Objects/Weapons")]
public class Weapons : ScriptableObject
{
    public Weapon[] weapons;
}
