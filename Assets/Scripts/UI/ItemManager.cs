using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ItemTypeBanner
{
    public Item.ItemType type;
    public Sprite banner;
}

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject itemStripPrefab;
    [SerializeField] Transform contentTransform;

    [SerializeField] ItemTypeBanner[] itemTypeBanners;

    // Update is called once per frame
    void Awake()
    {
        int i = 0;

        foreach (Transform item in contentTransform)
        {
            if (item != contentTransform)
            {
                Destroy(item.gameObject);
            }
        }

        foreach (var item in itemTypeBanners)
        {
            ItemStrip strip = Instantiate(itemStripPrefab, contentTransform).GetComponent<ItemStrip>();
            strip.Type = itemTypeBanners[i].type;
            strip.Banner = itemTypeBanners[i].banner;
            strip.UpdateContents();

            i++;
        }
    }
}