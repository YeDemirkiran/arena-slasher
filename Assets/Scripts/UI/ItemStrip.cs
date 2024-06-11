using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStrip : MonoBehaviour
{
    public Item.ItemType Type { get; set; }
    public Sprite Banner { get; set; }

    public ItemManager manager { get; set; }

    [SerializeField] Transform content;
    [SerializeField] Items items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Image banner;

    public void UpdateContents()
    {
        foreach (Transform item in content)
        {
            if (item != content)
            {
                Destroy(item.gameObject);
            }
        }

        for (int i = 0; i < items.items.Length; i++)
        {
            if (items[i].type == Type)
            {
                ItemSlot slot = Instantiate(itemPrefab, content).GetComponent<ItemSlot>();
                slot.Initialize(items[i], manager);
            }
        }

        banner.sprite = Banner;
    }
}