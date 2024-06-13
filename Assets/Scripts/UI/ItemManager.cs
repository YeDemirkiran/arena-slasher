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
    [SerializeField] Transform selectorOutline;

    [Header("General")]
    [SerializeField] Type type;
    [SerializeField] TMP_Text itemInfoName;
    
    [Header("Store")]
    [SerializeField] ItemTypeBanner[] itemTypeBanners;
    [SerializeField] Button buyButton;
    [SerializeField] GameObject itemStripPrefab;
    [SerializeField] Transform contentTransform;
    [SerializeField] GameObject cantBuyMessage, alreadyHaveMessage;
    [SerializeField] TMP_Text itemInfoPrice;
    [SerializeField] TMP_Text itemInfoDescription;

    [Header("Inventory")]
    [SerializeField] Button equipButton;

    GameData data;
    
    public enum Type { Store, Inventory }

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

            selectorOutline.gameObject.SetActive(true);
            selectorOutline.SetParent(value.transform.parent);
            selectorOutline.SetAsFirstSibling();
            selectorOutline.position = value.transform.position;

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
                EquipSelectedItem();
            }
        }
    }

    public void EquipSelectedItem()
    {

    }

    public void DrawInfoScreen()
    {
        var value = selectedSlot;

        itemInfoName.text = value.item.name;

        switch (type)
        {
            case Type.Store:
                itemInfoDescription.text = value.item.description;
                itemInfoPrice.text = "$" + value.item.price;

                bool noMoney = value.item.price > data.currency;
                bool alreadyHave = data.boughtItemIDs.Contains(value.item.id);

                cantBuyMessage.SetActive(noMoney);
                itemInfoPrice.gameObject.SetActive(!alreadyHave);
                alreadyHaveMessage.SetActive(alreadyHave);

                buyButton.interactable = !(noMoney || alreadyHave);
                break;

            case Type.Inventory:
                bool alreadyEquipped = data.equippedItemIDs.Contains(value.item.id);
                equipButton.interactable = !alreadyEquipped;
                break;

            default:
                break;
        }
    }
}