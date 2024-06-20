using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemTypeBanner
{
    public ItemType type;
    public Sprite banner;
}

[System.Serializable]
public class ItemTypeCamera
{
    public ItemType type;
    public Transform targetTransform;
}

public class ItemManager : MonoBehaviour
{
    [SerializeField] Transform selectorOutline;

    [Header("General")]
    [SerializeField] PlayerMenuController playerMenuController;
    [SerializeField] TMP_Text itemInfoName;
    
    [SerializeField] ItemTypeBanner[] itemTypeBanners;
    [SerializeField] Button buyButton;
    [SerializeField] GameObject itemStripPrefab;
    [SerializeField] Transform contentTransform;
    [SerializeField] GameObject cantBuyMessage;
    [SerializeField] TMP_Text itemInfoPrice;
    [SerializeField] TMP_Text itemInfoDescription;

    [SerializeField] Button equipButton;
    [SerializeField] BotOutfit botOufit;

    [Header("Animation")]
    [SerializeField] CameraEffects camEffects;
    [SerializeField] ItemTypeCamera[] cameraPositions;

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

            if (_selectedSlot != null)
            {
                MoveCameraToItemTarget(value.item.type);

                selectorOutline.gameObject.SetActive(true);
                selectorOutline.SetParent(value.transform.parent);
                selectorOutline.SetAsFirstSibling();
                selectorOutline.position = value.transform.position;

                
            }
            else
            {
                selectorOutline.gameObject.SetActive(false);
            }

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

        data = GameManager.Instance.gameData;

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
    }

    private void OnEnable()
    {
        selectedSlot = null;
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
        for (int id = data.equippedItemIDs.Count; id >= 0; id--)
        {
            if (Items.Instance[id].type == selectedSlot.item.type)
            {
                data.EquipItem(selectedSlot.item, id);
                playerMenuController.UpdatePlayerOutfit();
            }
        }

        DrawInfoScreen(); 
    }

    public void DrawInfoScreen()
    {
        var value = selectedSlot;

        itemInfoName.text = value.item.name;

        itemInfoDescription.text = value.item.description;
        itemInfoPrice.text = "$" + value.item.price;

        bool noMoney = value.item.price > data.currency;
        bool alreadyHave = data.boughtItemIDs.Contains(value.item.id);
        bool alreadyEquipped = data.equippedItemIDs.Contains(value.item.id);

        cantBuyMessage.SetActive(noMoney && !alreadyHave);
        itemInfoPrice.gameObject.SetActive(!alreadyHave);       

        buyButton.gameObject.SetActive(!alreadyHave);
        buyButton.interactable = !noMoney;

        equipButton.gameObject.SetActive(alreadyHave);
        equipButton.interactable = !alreadyEquipped;
    }

    public void MoveCameraToItemTarget(ItemType type)
    {
        foreach (var item in cameraPositions)
        {
            if (item.type == type)
            {
                camEffects.MoveToTransform(item.targetTransform, 0.25f, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f));
                break;
            }
        }
    }
}