using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Item item { get; private set; }

    [SerializeField] Button button;
    [SerializeField] Image bannerSlot;
    [SerializeField] TMP_Text textSlot;
    [SerializeField] GameObject lockedImage;
    

    public void Initialize(Item item, ItemManager manager)
    {
        this.item = item;
        bannerSlot.sprite = item.banner;
        textSlot.name = item.name;

        lockedImage.SetActive(item.Locked(GameManager.Instance.gameData));

        button.onClick.AddListener(() => manager.selectedSlot = this);
    }
}