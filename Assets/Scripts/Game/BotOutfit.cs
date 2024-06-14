using UnityEngine;


public class BotOutfit : MonoBehaviour
{
    [SerializeField] Bone[] bones;

    public void SetGear(GameObject gearPrefab, Transform parent)
    {
        if (gearPrefab != null)
        {
            GameObject gear = Instantiate(gearPrefab, parent, false);

            Outfit[] outfits = gear.GetComponentsInChildren<Outfit>();

            foreach (Outfit outfit in outfits)
            {
                outfit.SetPairs(bones);
            }
        }
    }
}