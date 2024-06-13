using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BotOutfit : MonoBehaviour
{
    [SerializeField] Bone[] bones;

    public Outfit SetGear(GameObject gearPrefab, Transform parent)
    {
        if (gearPrefab != null)
        {
            GameObject gear = Instantiate(gearPrefab, parent, false);
            Outfit outfit = gear.GetComponent<Outfit>();
            outfit.SetPairs(bones);
            return outfit;
        }

        return null;
    }
}