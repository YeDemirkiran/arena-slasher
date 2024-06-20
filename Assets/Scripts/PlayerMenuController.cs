using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuController : MonoBehaviour
{
    BotOutfit outfit;
    GameData data;
    Vector3 euler;

    IEnumerator Start()
    {
        outfit = GetComponent<BotOutfit>();

        while (GameManager.Instance == null)
        {
            yield return null;
        }

        data = GameManager.Instance.gameData;

        UpdatePlayerOutfit();

        euler = transform.localEulerAngles;
    }

    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            euler += Vector3.up * Input.GetAxis("Mouse X") * -10f;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameManager.Instance.SetMouse(false);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            GameManager.Instance.SetMouse(true);
        }

        transform.localEulerAngles = euler;
    }

    public void UpdatePlayerOutfit()
    {
        foreach (var id in data.equippedItemIDs)
        {
            outfit.SetGear(Items.Instance[id].prefab, outfit.transform);
        }

        outfit.SetWeapon(Items.Instance.weapons[data.equippedWeaponID].prefab, outfit.transform);
    }
}
