using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemTypeBanner
{
    public Item.ItemType type;
    public Sprite banner;
}

public class ItemManager : MonoBehaviour
{
    [SerializeField] Button buyButton;
    [SerializeField] GameObject itemStripPrefab;
    [SerializeField] Transform contentTransform;
    [SerializeField] Transform selectorOutline;

    [SerializeField] GameObject cantBuyMessage, alreadyHaveMessage;
    [SerializeField] TMP_Text itemInfoName, itemInfoDescription, itemInfoPrice;

    [SerializeField] ItemTypeBanner[] itemTypeBanners;

    GameData data;

    ItemSlot _selectedSlot;
    public ItemSlot selectedSlot
    {
        get
        {
            return _selectedSlot;
        }

        set
        {
            _selectedSlot = value;

            DrawInfoScreen();
        }
    }

    // Update is called once per frame
    IEnumerator Start()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }

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
            strip.manager = this;
            strip.Type = itemTypeBanners[i].type;
            strip.Banner = itemTypeBanners[i].banner;
            strip.UpdateContents();

            i++;
        }

        data = GameManager.Instance.gameData;
    }

    public void BuySelectedItem()
    {
        Item item = selectedSlot.item;

        if (item != null)
        {
            if (data.currency >= item.price)
            {
                data.BuyItem(item);
            }
        }
    }

    public void DrawInfoScreen()
    {
        var value = selectedSlot;

        selectorOutline.position = value.transform.position;
        itemInfoName.text = value.item.name;
        itemInfoDescription.text = value.item.description;
        itemInfoPrice.text = "$" + value.item.price;

        bool noMoney = value.item.price > data.currency;
        bool alreadyHave = data.boughtItemIDs.Contains(value.item.id);
        cantBuyMessage.SetActive(noMoney);
        itemInfoPrice.gameObject.SetActive(!alreadyHave);
        alreadyHaveMessage.SetActive(alreadyHave);

        buyButton.interactable = !(noMoney || alreadyHave);
    }
}