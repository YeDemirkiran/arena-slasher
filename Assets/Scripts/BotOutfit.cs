using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BotOutfit : MonoBehaviour
{
    BotController controller;

    [SerializeField] Bone[] bones;

    Outfit headGear, torsoGear, pantsGear, feetGear;
    Outfit weapon;

    void Awake()
    {
        controller = GetComponent<BotController>();
    }

    public void SetHeadGear(GameObject gear)
    {
        if (headGear != null) Destroy(headGear.gameObject);

        headGear = SetGear(gear, transform);
        headGear.SetPairs(bones);
    }

    public void SetTorsoGear(GameObject gear)
    {
        if (torsoGear != null) Destroy(torsoGear);
        torsoGear = SetGear(gear, transform);
        torsoGear.SetPairs(bones);
    }

    public void SetPantsGear(GameObject gear)
    {
        if (pantsGear != null) Destroy(pantsGear);
        pantsGear = SetGear(gear, transform);
        pantsGear.SetPairs(bones);
    }

    public void SetFeetGear(GameObject gear)
    {
        if (feetGear != null) Destroy(feetGear);
        feetGear = SetGear(gear, transform);
        feetGear.SetPairs(bones);
    }

    public void SetWeapon(Weapon weapon)
    {
        if (this.weapon != null) Destroy(this.weapon);
        this.weapon = SetGear(weapon.prefab, transform);
        controller.currentWeapon = weapon;
    }

    //GameObject SetGear(GameObject gearPrefab, Transform parent)
    //{
    //    if (gearPrefab != null)
    //    {
    //        GameObject gear = Instantiate(gearPrefab, parent, false);
    //        return gear;
    //    }

    //    return null;
    //}

    Outfit SetGear(GameObject gearPrefab, Transform parent)
    {
        if (gearPrefab != null)
        {
            GameObject gear = Instantiate(gearPrefab, parent, false);
            return gear.GetComponent<Outfit>();
        }

        return null;
    }
}