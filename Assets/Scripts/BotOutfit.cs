using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BotOutfit : MonoBehaviour
{
    BotController controller;

    public Transform headGearRoot;
    public Transform torsoGearRoot;
    public Transform pantsGearRoot;
    public Transform feetGearRoot;
    public Transform weaponRoot;

    Animator headGear, torsoGear, pantsGear, feetGear;
    Animator weapon;

    void Awake()
    {
        controller = GetComponent<BotController>();
    }

    // Must call every frame to update the outfits
    // I will work on a better system later
    public void UpdateOutfit(Animator baseAnimator)
    {
        Debug.Log(torsoGear == null);

        for (int i = 0; i < baseAnimator.parameters.Length; i++)
        {
            Debug.Log("param " + i + " " + baseAnimator.parameters[i]);
            torsoGear.parameters[i] = baseAnimator.parameters[i];
        }
    }

    public void SetHeadGear(GameObject gear)
    {
        if (headGear != null) Destroy(headGear);

        headGear = SetGear(gear, headGearRoot).GetComponent<Animator>();
    }

    public void SetTorsoGear(GameObject gear)
    {
        if (torsoGear != null) Destroy(torsoGear);
        headGear = SetGear(gear, torsoGearRoot).GetComponent<Animator>();
    }

    public void SetPantsGear(GameObject gear)
    {
        if (pantsGear != null) Destroy(pantsGear);
        pantsGear = SetGear(gear, pantsGearRoot).GetComponent<Animator>();
    }

    public void SetFeetGear(GameObject gear)
    {
        if (feetGear != null) Destroy(feetGear);
        feetGear = SetGear(gear, feetGearRoot).GetComponent<Animator>();
    }

    public void SetWeapon(Weapon weapon)
    {
        if (this.weapon != null) Destroy(this.weapon);
        this.weapon = SetGear(weapon.prefab, weaponRoot).GetComponent<Animator>();
        controller.currentWeapon = weapon;
    }

    GameObject SetGear(GameObject gearPrefab, Transform parent)
    {
        if (gearPrefab != null)
        {
            GameObject gear = Instantiate(gearPrefab, parent, false);
            return gear;
        }

        return null;
    }
}