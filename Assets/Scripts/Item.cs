using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite sprite;
    public string itemName = "Item";
    public ItemType type;

    public bool canBePickedUp = true;

    public bool isStackable = true;

    public GameObject ObjectToSpawn;

}

public enum ItemType
{
    Food,
    Log,
    Trap
}
