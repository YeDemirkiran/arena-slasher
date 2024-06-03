using System.Linq;
using UnityEngine;

[System.Serializable]
public class Gear
{
    public int id;
    public string name;
    [TextArea] public string description;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "Gears", menuName = "Scriptable Objects/Gears")]
public class Gears : ScriptableObject
{
    public Gear[] gears;

    public Gear this[int i]
    {
        get { return gears.First(x => x.id == i); }
        set { gears[i] = value; }
    }
}