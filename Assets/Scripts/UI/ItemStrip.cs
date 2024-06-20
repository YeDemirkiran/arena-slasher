using UnityEngine;
using UnityEngine.UI;

public class ItemStrip : MonoBehaviour
{
    public ItemType Type { get; set; }
    public Sprite Banner { get; set; }

    public ItemManager manager { get; set; }

    Items items { get { return Items.Instance; } }

    [SerializeField] Transform content;
    [SerializeField] GameObject slotPrefab;
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

        for (int i = 0; i < items.Length; i++)
        {
            if (!items[i].excludeFromStore && items[i].type == Type)
            {
                ItemSlot slot = Instantiate(slotPrefab, content).GetComponent<ItemSlot>();
                slot.Initialize(items[i]);
                
                slot.button.onClick.AddListener(() => manager.selectedSlot = slot);
            }
        }

        if (banner != null)
        {
            banner.sprite = Banner;
        }
    }
}