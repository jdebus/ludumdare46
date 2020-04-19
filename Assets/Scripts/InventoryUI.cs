using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; } = null;

    [SerializeField]
    InventorySlot[] slots;

    [SerializeField]
    Image selectedItemImage;

    public Item SelectedItem { get; set; }

    [SerializeField]
    HoverText hoverText;


    public AudioClip pickupSound;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        foreach (var slot in slots)
        {
            slot.OnSlotClick += ItemClicked;
        }
    }
  
    void ItemClicked(Item item)
    {
        if (SelectedItem != null)
            return;

        SelectedItem = item;
        selectedItemImage.sprite = item.sprite;
    }

    private void Update()
    {
        selectedItemImage.enabled = SelectedItem != null;
        selectedItemImage.transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(1))
            SelectedItem = null;

        CheckHoverText();
        var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        world.z = 0;
        if(Input.GetMouseButtonDown(0) && SelectedItem != null && (SelectedItem.type == ItemType.Trap || SelectedItem.type == ItemType.Log))
        {
            if (CanPlaceItemHere(world) && !EventSystem.current.IsPointerOverGameObject())
            {
                PlaceItem(SelectedItem, world);
                
                foreach (var slot in slots)
                {
                    if (slot.Item != null && SelectedItem != null && slot.Item.itemName == SelectedItem.itemName)
                    {
                        slot.ItemUsed();
                        if (slot.IsEmpty)
                            SelectedItem = null;
                    }
                }
            }
            else
            {
                Debug.Log("Trap cannot be placed here!?");
            }
        }
    }

    void PlaceItem(Item item, Vector3 position)
    {
        if(item.type == ItemType.Trap)
        {
            var go = Instantiate(SelectedItem.gameObject, position, Quaternion.identity);
            go.SetActive(true);
            var trap = go.GetComponent<Trap>();
            trap.Place();
            
        }
        else if(item.type == ItemType.Log)
        {
            Instantiate(SelectedItem.ObjectToSpawn, position, Quaternion.identity);
        }
    }

    bool CanPlaceItemHere(Vector2 position)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(position, 0.5f, Vector2.zero);
        foreach (var hit in hits)
        {
            if (hit.collider.attachedRigidbody != null)
                return false;
        }

        return true;
    }

    void CheckHoverText()
    {
        hoverText.Hide();
        if (SelectedItem != null)
        {
            var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            world.z = 0;
            hoverText.transform.position = world + new Vector3(0,1,0);
            RaycastHit2D[] hits = Physics2D.CircleCastAll(world, 0.5f, Vector2.zero);
            
            if (hits.Length == 0)
            {
                if (SelectedItem.type == ItemType.Trap && CanPlaceItemHere(world))
                {
                    hoverText.Show();
                    hoverText.SetSprite(HoverText.HoverTextText.Place);
                }
                else if(SelectedItem.type == ItemType.Log && CanPlaceItemHere(world))
                {
                    hoverText.Show();
                    hoverText.SetSprite(HoverText.HoverTextText.MakeFire);
                }
                
            }

            foreach (var hit in hits)
            {
                if(SelectedItem.type == ItemType.Food)
                {
                    if (hit.collider.attachedRigidbody == null)
                        continue;

                    if(hit.collider.attachedRigidbody.CompareTag("Player"))
                    {
                        hoverText.Show();
                        hoverText.SetSprite(HoverText.HoverTextText.Eat);
                        return;
                    }
                    else if(hit.collider.attachedRigidbody.CompareTag("Nest") && GameManager.Instance.Nest.State == NestState.BabyInside)
                    {
                        hoverText.Show();
                        hoverText.SetSprite(HoverText.HoverTextText.Feed);
                        return;
                    }
                    else
                    {
                        hoverText.Hide(); 
                    }
                }
            }
        }
    }

    public bool PickupItem(Item item)
    {
        foreach (var slot in slots)
        {
            if(slot.Item != null && slot.Item.itemName == item.itemName && slot.Item.isStackable)
            {
                AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
                slot.Count++;
                return true;
            }
        }

        var emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
            emptySlot.SetItem(item);
            return true;
        }

        return false;
    }


    public InventorySlot GetEmptySlot()
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
                return slot;
        }
        return null;
    }

    public void ItemUsed(Item item)
    {
        foreach (var slot in slots)
        {
            if (slot.Item == item)
            {
                slot.ItemUsed();
                if (slot.IsEmpty)
                    SelectedItem = null;
            }
        }
    }
}
