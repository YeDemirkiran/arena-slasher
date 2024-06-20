using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Item item { get; private set; }

    public Button button {  get; private set; }
    [SerializeField] Image bannerSlot;
    [SerializeField] TMP_Text textSlot;
    [SerializeField] GameObject lockedImage;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Initialize(Item item)
    {
        this.item = item;
        bannerSlot.sprite = item.icon;
        textSlot.text = item.name;

        lockedImage.SetActive(item.Locked()); 
    }
}