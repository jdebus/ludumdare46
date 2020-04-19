using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = initialScale * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = initialScale;
    }

    Vector3 initialScale;
    public Item Item { get; set; }

    public Image ItemImage;

    public int Count { get; set; } = 0;

    public TMPro.TextMeshProUGUI text;
    public bool IsEmpty => Item == null && Count == 0;

    public Action<Item> OnSlotClick;
    void Start()
    {
        initialScale = transform.localScale;
    }

    public void SetItem(Item item)
    {
        this.Item = item;
        
        this.Count = 1;
        if (this.Item != null)
            ItemImage.sprite = Item.sprite;
        else
            ItemImage.sprite = null;
    }

    private void Update()
    {
        text.text = Count.ToString();
        text.enabled = Count > 0 && Item != null && Item.isStackable;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Item != null)
            this.OnSlotClick?.Invoke(this.Item);
    }

    internal void Clear()
    {
        ItemImage.sprite = null;
        Count = 0;
        //Destroy(Item.gameObject);
        Item = null;
    }

    public void ItemUsed()
    {
        if (Item.isStackable)
        {
            Count--;
            if (Count == 0)
                Clear();
        }
        else
            Clear();
    }
}
