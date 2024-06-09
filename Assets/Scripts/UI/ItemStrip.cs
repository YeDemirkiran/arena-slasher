using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStrip : MonoBehaviour
{
    public Item.ItemType Type { get; set; }
    public Sprite Banner { get; set; }

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
                GameObject item = Instantiate(itemPrefab, content);
                item.transform.Find("Image").GetComponent<Image>().sprite = items[i].banner;
                item.transform.GetComponentInChildren<TMP_Text>().text = items[i].name;
            }
        }

        banner.sprite = Banner;
    }
}