using System.Collections.Generic;
using UnityEngine;


public class BotOutfit : MonoBehaviour
{
    [SerializeField] Bone[] bones;

    public GameObject SetGear(GameObject gearPrefab, Transform parent)
    {
        if (gearPrefab != null)
        {
            GameObject gear = Instantiate(gearPrefab, parent, false);

            Outfit[] outfits = gear.GetComponentsInChildren<Outfit>();

            foreach (Outfit outfit in outfits)
            {
                outfit.SetPairs(bones);
            }

            return gear;
        }

        return null;
    }

    public WeaponController[] SetWeapon(GameObject weaponPrefab, Transform parent)
    {
        WeaponController[] weapons;

        if (weaponPrefab != null)
        {
            GameObject weapon = Instantiate(weaponPrefab, parent, false);

            weapons = weapon.GetComponentsInChildren<WeaponController>();

            Outfit[] outfits = weapon.GetComponentsInChildren<Outfit>();

            foreach (Outfit outfit in outfits)
            {
                outfit.SetPairs(bones);
            }

            return weapons;
        }

        return null;
    }
}